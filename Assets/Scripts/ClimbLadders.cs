using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 5f;

    private bool isClimbing;
    private Rigidbody playerRigidbody;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isClimbing = true;
            playerRigidbody = other.GetComponent<Rigidbody>();
            playerRigidbody.useGravity = false; // Disable gravity when climbing
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isClimbing = false;
            playerRigidbody.useGravity = true; // Enable gravity when not climbing
            playerRigidbody = null;
        }
    }

    void FixedUpdate()
    {
        if (isClimbing && playerRigidbody != null)
        {
            float verticalInput = Input.GetAxis("Vertical");
            float climbDirection = verticalInput > 0 ? 1 : (verticalInput < 0 ? -1 : 0); // Determine climb direction

            // Move the player vertically based on climb direction
            playerRigidbody.MovePosition(transform.position + Vector3.up * climbDirection * climbSpeed * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        // Check for space key press to exit the ladder
        if (isClimbing && Input.GetKeyDown(KeyCode.Space))
        {
            isClimbing = false;
            playerRigidbody.useGravity = true; // Enable gravity when exiting ladder
            playerRigidbody = null;
        }
    }
}
