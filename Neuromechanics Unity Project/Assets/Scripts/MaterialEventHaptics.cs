using UnityEngine;

public class MaterialEventHaptics : MonoBehaviour
{
    public OVRInput.Controller targetController = OVRInput.Controller.RTouch;
    public float vibrationDuration = 0.5f;

    private bool isEventActive = false;

    private void Start()
    {
        // Example: Initialize your custom material event system.
        // For instance, you can call this method when specific gameplay events occur.
        InitializeMaterialEventSystem();
    }

    private void Update()
    {
        // Listen for a specific input, such as pressing the "Fire1" button (you can change this to any input you want).
        if (Input.GetButtonDown("Fire1") && isEventActive)
        {
            // Trigger haptic feedback (vibration) on the specified controller.
            TriggerHapticFeedback();
        }
    }

    private void InitializeMaterialEventSystem()
    {
        // Example: You might define a method that triggers the custom material event.
        // You can call this method when specific conditions are met in your game.
        TriggerCustomMaterialEvent();
    }

    private void TriggerCustomMaterialEvent()
    {
        // Example: Simulate a material event trigger here.
        // This is where you would change materials or trigger your custom event.
        // For demonstration purposes, we'll just activate the event in this example.
        isEventActive = true;
    }

    private void TriggerHapticFeedback()
    {
        // Trigger haptic feedback on the specified controller.
        OVRInput.SetControllerVibration(1f, 1f, targetController);

        // Stop the haptic feedback after the specified duration.
        StartCoroutine(StopHapticAfterDuration(targetController, vibrationDuration));
    }

    private System.Collections.IEnumerator StopHapticAfterDuration(OVRInput.Controller controller, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Stop the haptic feedback on the specified controller.
        OVRInput.SetControllerVibration(0f, 0f, controller);
    }
}
