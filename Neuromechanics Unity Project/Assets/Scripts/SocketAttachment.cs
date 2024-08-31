using UnityEngine;

public class SocketAttachment : MonoBehaviour
{
    public GameObject socket; // Reference to the socket where the trigger object will attach
    public GameObject attachedObject; // Reference to the currently attached trigger object

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == socket && attachedObject == null)
        {
            AttachTriggerObject(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == socket && attachedObject != null)
        {
            DetachTriggerObject();
        }
    }

    private void AttachTriggerObject(GameObject triggerObject)
    {
        attachedObject = triggerObject;
        attachedObject.transform.parent = transform; // Attach the trigger object to the main object
        attachedObject.transform.localPosition = Vector3.zero; // Reset its position relative to the socket
        attachedObject.GetComponent<Rigidbody>().isKinematic = true; // Make the trigger object immovable
    }

    private void DetachTriggerObject()
    {
        attachedObject.GetComponent<Rigidbody>().isKinematic = false; // Allow the trigger object to move again
        attachedObject.transform.parent = null; // Detach the trigger object from the main object
        attachedObject = null;
    }
}
