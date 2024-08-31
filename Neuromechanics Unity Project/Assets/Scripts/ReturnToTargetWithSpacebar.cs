using UnityEngine;

public class ReturnToTargetWithSpacebar : MonoBehaviour
{
    public Transform targetTransform; // The target position to move towards
    public Vector3 offset; // The offset from the target position
    public float moveSpeed = 5.0f; // Adjust the movement speed as needed

    private Transform currentTransform;
    private bool isMoving = false;
    private Quaternion initialRotation; // Store the initial rotation

    private void Start()
    {
        currentTransform = transform; // Initialize the current transform
        initialRotation = currentTransform.rotation; // Store the initial rotation
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMoving)
            {
                // Start moving towards the target position with offset
                isMoving = true;
            }
            else
            {
                // Cancel the movement when Spacebar is pressed again
                isMoving = false;
            }
        }

        if (isMoving)
        {
            // Calculate the target position with offset
            Vector3 targetPositionWithOffset = targetTransform.position + offset;

            // Calculate the direction and distance to the target position
            Vector3 directionToTarget = targetPositionWithOffset - currentTransform.position;
            float distanceToTarget = directionToTarget.magnitude;

            // Check if the object is close enough to the target position
            float positionThreshold = 0.01f; // Adjust the position threshold as needed

            if (distanceToTarget > positionThreshold)
            {
                // Move towards the target position with offset
                currentTransform.position += directionToTarget.normalized * Time.deltaTime * moveSpeed;

                // Lock the rotation to the initial rotation
                currentTransform.rotation = initialRotation;
            }
            else
            {
                // Ensure the position is exactly the target position with offset
                currentTransform.position = targetPositionWithOffset;

                // Lock the rotation to the initial rotation
                currentTransform.rotation = initialRotation;

                // Stop the movement
                isMoving = false;
            }
        }
    }
}

