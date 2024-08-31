using UnityEngine;

public class SnapOnCollision : MonoBehaviour
{
    public LayerMask targetLayer; // The layer containing the objects that can be socketed
    public float maxAttachDistance = 0.1f; // Maximum distance for attachment
    public float snapHeight = 1.0f; // Height above the center of the socket object to snap
    public float springForce = 100.0f; // The force applied by the spring joint
    public float springDamper = 10.0f; // The damping on the spring joint

    private bool isSocketed = false;
    private GameObject attachedObject;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isSocketed && (targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            // Attach to the object if it's on the target layer
            AttachObject(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isSocketed && collision.gameObject == attachedObject)
        {
            // Detach from the object if it's the attached object
            DetachObject();
        }
    }

    private void AttachObject(GameObject objToAttach)
    {
        if (isSocketed) return; // Already socketed

        isSocketed = true;
        attachedObject = objToAttach;

        // Calculate the position relative to the center of the socket object
        Vector3 targetPosition = transform.position + Vector3.up * snapHeight;

        // Apply the calculated position to the attached object
        attachedObject.transform.position = targetPosition;
        attachedObject.transform.rotation = transform.rotation;
        attachedObject.transform.SetParent(transform);

        // Disable the attached object's rigidbody to prevent physics interactions
        Rigidbody attachedRb = attachedObject.GetComponent<Rigidbody>();
        if (attachedRb)
        {
            attachedRb.isKinematic = true;
        }

        // Add a spring joint to create a socket-like behavior
        SpringJoint springJoint = attachedObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = rb;
        springJoint.spring = springForce;
        springJoint.damper = springDamper;
    }

    private void DetachObject()
    {
        if (!isSocketed) return; // Not socketed

        // Re-enable the rigidbody of the attached object
        Rigidbody attachedRb = attachedObject.GetComponent<Rigidbody>();
        if (attachedRb)
        {
            attachedRb.isKinematic = false;
        }

        // Remove the spring joint
        SpringJoint springJoint = attachedObject.GetComponent<SpringJoint>();
        if (springJoint)
        {
            Destroy(springJoint);
        }

        // Unparent the attached object
        attachedObject.transform.SetParent(null);

        isSocketed = false;
        attachedObject = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxAttachDistance);
    }
}
