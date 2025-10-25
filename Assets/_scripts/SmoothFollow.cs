using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    // The target object for the camera to follow
    public Transform target;

    // The speed at which the camera will follow the target.
    // Smaller values result in slower, smoother movement.
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f;

    public float posY = 10.56f;

    // The offset from the target's position
    public Vector3 offset;

    // We use LateUpdate to ensure the target has completed all its movement
    // for the frame before the camera updates its position.
    void LateUpdate()
    {
        // Check if a target has been assigned
        if (target == null)
        {
            Debug.LogWarning("Camera follow target is not assigned.");
            return;
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;
        //desiredPosition.y = posY; // Keep a fixed height

        // Use Linear Interpolation (Lerp) to smoothly move from the current
        // camera position to the desired position.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the new position to the camera's transform
        transform.position = smoothedPosition;

        // (Optional) Make the camera always look at the target
        // transform.LookAt(target);
    }
}