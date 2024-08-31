using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToMainObject : MonoBehaviour
{
    public GameObject mainObject;
    public GameObject triggerObject;
    public float snapDistance = 0.1f;
    public float snapSpeed = 5.0f;

    private bool isSnapped = false;
    private Vector3 originalLocalPosition;

    private void Start()
    {
        originalLocalPosition = triggerObject.transform.localPosition;
    }

    private void Update()
    {
        if (!isSnapped && IsSnapping())
        {
            Snap();
        }
    }

    private bool IsSnapping()
    {
        float distance = Vector3.Distance(triggerObject.transform.position, mainObject.transform.position);
        return distance <= snapDistance;
    }

    private void Snap()
    {
        Vector3 targetPosition = mainObject.transform.position;
        Vector3 newPosition = Vector3.Lerp(triggerObject.transform.position, targetPosition, Time.deltaTime * snapSpeed);
        triggerObject.transform.position = newPosition;

        isSnapped = true;
        Debug.Log("Trigger object snapped to main object!");
    }
}
