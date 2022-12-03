using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.y = 0f;
        /*Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);*/
        transform.position = desiredPosition;
    }
}

