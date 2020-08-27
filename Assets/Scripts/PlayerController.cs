using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player, swordRotPoint, swordObj;

    // Player movement speed.
    public float moveSpeed;

    // Sword rotation speed.
    public float maxRotSpeed;
    private float currentRotSpeed;

    public Rigidbody2D playerRB;
    
    private Vector2 rotation;
    private int leftOrRight;

    // Determines if both buttons are pressed or not.
    public bool bothPressed;
    
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRB.velocity.magnitude > moveSpeed)
        {
            playerRB.velocity = playerRB.velocity.normalized * moveSpeed;
        }

        var playerPos = player.transform.position;
        swordRotPoint.transform.position = playerPos;
        
        // A + D are pressed: move based on rotation.
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            bothPressed = true;
            rotation = swordRotPoint.transform.rotation * Vector3.up;
            Debug.Log(rotation);
            playerRB.AddRelativeForce(rotation, ForceMode2D.Impulse);
        }
        else
        {
            bothPressed = false;
            playerRB.velocity -= playerRB.velocity/15;
        }

        // A pressed: clockwise sword rotation.
        if (Input.GetKey(KeyCode.A))
        {
            leftOrRight = 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            leftOrRight = -1;
        }


        // Increase rotation speed when either A or D are pressed, and not both at the same time.
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && bothPressed == false)
        {
            // Add to currentRotSpeed until max rotation speed reached.
            if (currentRotSpeed < maxRotSpeed)
                currentRotSpeed += maxRotSpeed/15;
            // Correct speed if it goes above max rotation speed limit.
            if (currentRotSpeed > maxRotSpeed)
                currentRotSpeed = maxRotSpeed;
        }
        else if (currentRotSpeed > 0)
        {
            // Subtract from currentRotSpeed until it reaches or goes below 0.
            currentRotSpeed -= maxRotSpeed/15;
            // Correct currentRotSpeed to 0 if it goes below 0.
            if (currentRotSpeed < 0)
                currentRotSpeed = 0;
        }
        
        // Rotate based on if either A or D are pressed separately, and which direction to rotate in based on directional input.
        // A: counterclockwise, D: clockwise
        swordRotPoint.transform.Rotate(0, 0, currentRotSpeed * leftOrRight * Time.deltaTime);


        

    }
}
