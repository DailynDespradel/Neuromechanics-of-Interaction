using UnityEngine;

public class VibrateOnCollision : MonoBehaviour
{
    public GameObject targetObject;
    public float vibrationDuration = 1.0f;
    public float vibrationStrength = 0.5f;

    private bool isVibrating = false;
    private float vibrationTimer = 0.0f;

    private void Update()
    {
        if (isVibrating)
        {
            vibrationTimer -= Time.deltaTime;
            if (vibrationTimer <= 0)
            {
                StopVibration();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetObject)
        {
            StartVibration();
        }
    }

    private void StartVibration()
    {
        isVibrating = true;
        vibrationTimer = vibrationDuration;

        // Assuming the target object has a Rigidbody
        Rigidbody targetRigidbody = targetObject.GetComponent<Rigidbody>();
        if (targetRigidbody != null)
        {
            targetRigidbody.AddForce(Random.insideUnitSphere * vibrationStrength, ForceMode.Impulse);
        }
    }

    private void StopVibration()
    {
        isVibrating = false;
        // You can add any additional logic here to stop the vibration effect
    }
}
