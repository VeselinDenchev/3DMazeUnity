using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotationSensitivity = 750f; // Made public so it can be accessed by other scripts
    public Transform playerBody; // The transform of our player body/mesh
    float xRotation = 0f; // Define our x rotation variable
    [SerializeField] private float clampDegreeUp = 70f; // How far the player can look up. Change to 90 for straight up
    [SerializeField] private float clampDegreeDown = -90f; // How far the player can look down. Change if you feel like it

    private void Start() // Called at game start
    {
        Cursor.visible = false; // Cursor will be hidden during play. Press Escape in Unity editor during play mode to change
        Cursor.lockState = CursorLockMode.Locked; // Cursor will be locked at the center of the screen during play. Press Escape in Unity editor during play mode to change
    }

    private void Update() // Called once per frame
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime; // Gets the horizontal mouse input
        float mouseY = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime; // Gets the vertical mouse input

        xRotation -= mouseY; // Our x rotation variable equals our vertical mouse input multiplied by -1
        xRotation = Mathf.Clamp(xRotation, clampDegreeDown, clampDegreeUp); // Clamp our x rotation by a minimum and maximum value (How far the player can look up and down in degrees)

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Implement the local rotation of the camera using our x rotation variable
        playerBody.Rotate(Vector3.up * mouseX); // Rotate our player body/mesh on the Y axis based on the horizontal mouse input
    }
}
