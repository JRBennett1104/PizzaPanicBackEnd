using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;  // The player transform
    public Vector3 offset;    // The offset from the player to maintain the camera position

    private Quaternion originalCameraRotation;  // To store the original rotation of the camera

    void Start()
    {
        // Store the camera's original rotation
        originalCameraRotation = transform.rotation;
    }

    void Update()
    {
        if (player != null)
        {
            // Update the camera's position instantly to follow the player’s position with the offset
            transform.position = player.position + offset;

            // Keep the camera’s original rotation (no following rotation)
            transform.rotation = originalCameraRotation;
        }
    }
}
