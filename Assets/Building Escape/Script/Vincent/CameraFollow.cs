using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform

    public float rotationSpeed = 5f; // Speed of camera rotation
    public float lookXLimit = 90f; // Limit of vertical camera rotation

    private float rotationX = 0f;

    void Start()
    {
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }
    void LateUpdate()
    {
        // Get the mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Rotate the player horizontally
        player.Rotate(Vector3.up * mouseX);

        // Handle vertical camera rotation
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        transform.rotation = Quaternion.Euler(rotationX, player.eulerAngles.y, 0f);
    }
}
