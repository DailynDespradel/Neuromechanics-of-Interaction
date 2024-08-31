using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReturnToOriginalOnHover : MonoBehaviour

{
    public GameObject physicalButton; // The GameObject representing the physical button
    public Vector3 originalPosition; // The original position of the object
    private Transform currentTransform;
    private bool isHovering = false;
    private float lerpSpeed = 2.0f; // Adjust the speed as needed

    private void Start()
    {
        currentTransform = transform; // Initialize the current transform
    }

    private void Update()
    {
        if (isHovering)
        {
            // Interpolate the object's position towards the original position
            currentTransform.position = Vector3.Lerp(currentTransform.position, originalPosition, Time.deltaTime * lerpSpeed);

            // Check if the object is close enough to the original position
            float threshold = 0.01f; // Adjust the threshold as needed
            if (Vector3.Distance(currentTransform.position, originalPosition) < threshold)
            {
                // Object has returned to the original position
                currentTransform.position = originalPosition; // Ensure the position is exactly the original position
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == physicalButton)
        {
            isHovering = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == physicalButton)
        {
            isHovering = false;
        }
    }
}
