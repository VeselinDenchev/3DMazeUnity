using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController charController; // Publicly define our character controller

    public bool canJump; // Enable this in the inspector if you want the player to jump

    public float walkSpeed = 2.5f;
    public float runSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;
    public Transform groundCheck; // The transform of our "Ground Check" object. Used to determine if we are on the ground
    public float groundDistance = 0.05f;
    public LayerMask groundMask; // Set a field for our "Ground" layer
    float speed; // Update-able moving speed

    Vector3 velocity;
    public bool grounded; // Defines is the player is touching the ground

    private void Update() // Called once per frame
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Checks if our player is touching an object with a "Ground" layer and sets the boolean accordingly

        if (grounded && velocity.y < 0) // If the player is grounded
        {
            velocity.y = -2f; // Set the downward velocity to 2
        }

        if (Input.GetKey(KeyCode.LeftShift)) // If the left shift button is being pressed
        {
            speed = runSpeed; // Set our speed to runSpeed
        }
        else
        {
            speed = walkSpeed;// Set our speed to walkSpeed
        }

        float x = Input.GetAxis("Horizontal"); // Define an X variable and set it to Unity's "Horizontal" axis
        float z = Input.GetAxis("Vertical"); // Define a Y variable and set it to Unity's "Vertical" axis

        Vector3 move = transform.right * x + transform.forward * z; // Define our move vector

        charController.Move(move * speed * Time.deltaTime); // Move our character controller based on our set inputs

        if (Input.GetButtonDown("Jump") && grounded && canJump) // Unity's default input for "Jump" is the space key
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Set our jumping gravity
        }

        velocity.y += gravity * Time.deltaTime; // Set our regular gravity

        charController.Move(velocity * Time.deltaTime); // Apply our gravity to our character controller
    }
}