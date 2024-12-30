/*
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    // References
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera playerCamera;  // Reference to the player’s own camera

    // Movement parameters
    public float rotationSpeed = 175f;       // Base rotation speed
    public float moveSpeed = 7f;             // Max move speed
    public float moveSpeedDecrease = 4f;
    public float acceleration = 10f;         // Acceleration (how fast the player picks up speed)
    public float rotationSpeedIncrease = 250f; // Rotation speed increase when right-clicking

    private Vector3 currentVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get reference to the Rigidbody

        // Ensure only the local player has control over their camera
        if (IsOwner)
        {
            if (playerCamera != null)
            {
                playerCamera.enabled = true; // Enable this player’s camera
            }
        }
        else
        {
            if (playerCamera != null)
            {
                playerCamera.enabled = false; // Disable camera for non-local players
            }
        }
    }

    private void Update()
    {
        // Only allow input handling if this is the owner (client who owns the player object)
        if (IsOwner)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        // Adjust movement and rotation speeds based on right-click
        if (Input.GetMouseButton(1))  // Right mouse button
        {
            rotationSpeed = rotationSpeedIncrease; // Increase rotation speed
            moveSpeed = moveSpeedDecrease;
        }
        else
        {
            rotationSpeed = 175f; // Base rotation speed
            moveSpeed = 7f;
        }

        // If the mouse is in the game window, calculate and send input
        if (IsMouseInGameWindow())
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            Vector3 targetDirection = (mouseWorldPosition - transform.position).normalized;
            targetDirection.y = 0; // Ignore vertical movement

            // Calculate the target rotation towards the mouse
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Apply the rotation update and forward movement logic on the server
            SubmitInputServerRpc(targetRotation, true);
        }
        else
        {
            // Stop both movement and rotation when the mouse is off-screen
            SubmitInputServerRpc(Quaternion.identity, false);
        }
    }

    // ServerRpc to receive input from the client and handle movement and rotation
    [ServerRpc]
    private void SubmitInputServerRpc(Quaternion targetRotation, bool shouldMove)
    {
        if (!IsServer) return;

        if (shouldMove)
        {
            // Rotate the player smoothly towards the target rotation
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Always move forward based on the current rotation
            Vector3 targetVelocity = transform.forward * moveSpeed;

            // Gradually adjust velocity using smooth acceleration
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
            rb.linearVelocity = currentVelocity; // Apply updated velocity
        }
        else
        {
            // Stop both movement and rotation if the mouse is off-screen
            rb.linearVelocity = Vector3.zero;  // Stop movement
            rb.angularVelocity = Vector3.zero;  // Stop rotation (no angular velocity)
        }

        // Synchronize position and rotation across network
        // This will update every client with the new position and rotation of the player object.
        SubmitMovementToClientsClientRpc(transform.position, rb.rotation);
    }

    // This function is called to update all clients with the player's position and rotation
    [ClientRpc]
    private void SubmitMovementToClientsClientRpc(Vector3 position, Quaternion rotation)
    {
        if (!IsOwner)  // Only apply this update on other clients (not the one who owns the player object)
        {
            rb.MovePosition(position);
            rb.MoveRotation(rotation);
        }
    }

    private bool IsMouseInGameWindow()
    {
        Vector3 mousePosition = Input.mousePosition;

        return mousePosition.x >= 0 && mousePosition.y >= 0 &&
               mousePosition.x <= Screen.width && mousePosition.y <= Screen.height;
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Use the player's own camera to determine mouse position
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point;
        }
        return transform.position;
    }
}
*/

using UnityEngine;
using Unity.Netcode;


public class PlayerController : NetworkBehaviour
{
    // References
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera playerCamera;  // Reference to the player’s own camera

    // Movement parameters
    public float rotationSpeed = 175f;       // Base rotation speed
    public float moveSpeed = 7f;             // Max move speed
    public float moveSpeedDecrease = 4f;
    public float acceleration = 10f;         // Acceleration (how fast the player picks up speed)
    public float rotationSpeedIncrease = 250f; // Rotation speed increase when right-clicking

    private Vector3 currentVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get reference to the Rigidbody

        // Ensure only the local player has control over their camera
        if (IsOwner)
        {
            if (playerCamera != null)
            {
                playerCamera.enabled = true; // Enable this player’s camera
            }
        }
        else
        {
            if (playerCamera != null)
            {
                playerCamera.enabled = false; // Disable camera for non-local players
            }
        }
    }

    private void Update()
    {
        // Only allow input handling if this is the owner (client who owns the player object)
        if (IsOwner)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        // Adjust movement and rotation speeds based on right-click
        if (Input.GetMouseButton(1))  // Right mouse button
        {
            rotationSpeed = rotationSpeedIncrease; // Increase rotation speed
            moveSpeed = moveSpeedDecrease;
        }
        else
        {
            rotationSpeed = 175f; // Base rotation speed
            moveSpeed = 7f;
        }

        // If the mouse is in the game window, calculate and send input
        if (IsMouseInGameWindow())
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            Vector3 targetDirection = (mouseWorldPosition - transform.position).normalized;
            targetDirection.y = 0; // Ignore vertical movement

            // Calculate the target rotation towards the mouse
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Apply the rotation update and forward movement logic on the server
            SubmitInputServerRpc(targetRotation, true);
        }
        else
        {
            // Stop both movement and rotation when the mouse is off-screen
            SubmitInputServerRpc(Quaternion.identity, false);
        }
    }

    // ServerRpc to receive input from the client and handle movement and rotation
    [ServerRpc]
    private void SubmitInputServerRpc(Quaternion targetRotation, bool shouldMove)
    {
        if (!IsServer) return;

        if (shouldMove)
        {
            // Rotate the player smoothly towards the target rotation
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Always move forward based on the current rotation
            Vector3 targetVelocity = transform.forward * moveSpeed;

            // Gradually adjust velocity using smooth acceleration
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
            rb.linearVelocity = currentVelocity; // Apply updated velocity
        }
        else
        {
            // Stop both movement and rotation if the mouse is off-screen
            rb.linearVelocity = Vector3.zero;  // Stop movement
            rb.angularVelocity = Vector3.zero;  // Stop rotation (no angular velocity)
        }
    }

    private bool IsMouseInGameWindow()
    {
        Vector3 mousePosition = Input.mousePosition;

        return mousePosition.x >= 0 && mousePosition.y >= 0 &&
               mousePosition.x <= Screen.width && mousePosition.y <= Screen.height;
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Use the player's own camera to determine mouse position
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point;
        }
        return transform.position;
    }
}
