using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Set to PlayerShip
    public Vector3 offsetDirection = new Vector3(0, 1, -3).normalized; // Closer: 1 up, 3 back
    public float offsetMagnitude = 5f; // Tighter default distance
    public float minOffset = 2f;       // Min zoom
    public float maxOffset = 8f;       // Max zoom (reduced)
    public float smoothSpeed = 0.2f;   // Faster position smoothing
    public float rotationSmoothSpeed = 0.1f; // Faster rotation smoothing

    void LateUpdate()
    {
        // Zoom
        if (Input.GetKey(KeyCode.O)) offsetMagnitude += 5f * Time.deltaTime;
        if (Input.GetKey(KeyCode.L)) offsetMagnitude -= 5f * Time.deltaTime;
        offsetMagnitude = Mathf.Clamp(offsetMagnitude, minOffset, maxOffset);

        // Recenter (R key as example)
        if (Input.GetKeyDown(KeyCode.R))
        {
            offsetMagnitude = 5f; // Reset to default distance
            transform.position = target.position + (offsetDirection * offsetMagnitude); // Instant recenter
        }

        // Position: Keep closer
        Vector3 desiredPosition = target.position + (offsetDirection * offsetMagnitude);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Rotation: Match yaw instantly, smooth pitch/roll
        Quaternion targetRotation = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, target.eulerAngles.z);
        Quaternion yawOnlyRotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothSpeed);
        transform.rotation = Quaternion.Euler(smoothedRotation.eulerAngles.x, yawOnlyRotation.eulerAngles.y, smoothedRotation.eulerAngles.z);
    }
}