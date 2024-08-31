using UnityEngine;
using Oculus;

public class HandInteractionHaptics : MonoBehaviour
{
    public OVRInput.Controller targetHand = OVRInput.Controller.RTouch;
    public OVRInput.Button selectButton = OVRInput.Button.PrimaryIndexTrigger;
    public float vibrationDuration = 0.5f;

    private bool isObjectSelected = false;

    private void Start()
    {
        // Empty start method.
    }

    private void Update()
    {
        OVRSkeleton skeleton = GetHandSkeleton();

        // Check if the hand is interacting with an object.
        isObjectSelected = IsHandInteracting(skeleton);

        // Trigger haptic feedback if the select button is pressed and an object is selected.
        if (isObjectSelected && OVRInput.GetDown(selectButton, targetHand))
        {
            TriggerHapticFeedback();
        }
    }

    private OVRSkeleton GetHandSkeleton()
    {
        // Get the OVRSkeleton component of the target hand.
        if (targetHand == OVRInput.Controller.LTouch)
        {
            return GameObject.Find("LeftHand").GetComponent<OVRSkeleton>();
        }
        else if (targetHand == OVRInput.Controller.RTouch)
        {
            return GameObject.Find("RightHand").GetComponent<OVRSkeleton>();
        }
        return null;
    }

    private bool IsHandInteracting(OVRSkeleton skeleton)
    {
        if (skeleton != null)
        {
            // Get the finger bones for index and thumb.
            OVRSkeleton.BoneId indexBone = OVRSkeleton.BoneId.Hand_Index3;
            OVRSkeleton.BoneId thumbBone = OVRSkeleton.BoneId.Hand_Thumb3;

            // Check if the index and thumb fingers are flexed enough to represent pinching.
            float indexFlex = skeleton.Bones[(int)indexBone].Transform.localRotation.eulerAngles.x;
            float thumbFlex = skeleton.Bones[(int)thumbBone].Transform.localRotation.eulerAngles.x;

            // Adjust these threshold values based on your pinch gesture setup.
            float pinchThreshold = 5f; // Adjust this value as needed.

            return (indexFlex < pinchThreshold && thumbFlex < pinchThreshold);
        }

        return false;
    }


    private void TriggerHapticFeedback()
    {
        // Trigger haptic feedback on the specified hand controller.
        OVRInput.SetControllerVibration(1f, 1f, targetHand);

        // Stop the haptic feedback after the specified duration.
        StartCoroutine(StopHapticAfterDuration(targetHand, vibrationDuration));
    }

    private System.Collections.IEnumerator StopHapticAfterDuration(OVRInput.Controller controller, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Stop the haptic feedback on the specified controller.
        OVRInput.SetControllerVibration(0f, 0f, controller);
    }
}
