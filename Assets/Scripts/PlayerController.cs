using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; // Movement speed of the player
    public float jumpForce; // Force applied when jumping
    public float maxVerticalVelocity; // Maximum vertical velocity to prevent excessive upward movement
    public float shiftSpeedMultiplier; // Speed multiplier when Shift key is pressed
    public int score;
    public TextMeshProUGUI tmp_score;
    private Camera firstPersonCamera;
    private Camera thirdPersonCamera;

    private Rigidbody rb;
    private bool isGrounded; // Flag to track if the player is grounded
    private bool isFirstPerson = true; // Flag to track the active camera view

    private Transform orientation;

    private bool isShooting = false; // Flag to track shooting state
    private readonly float shootInterval = 0.1f; // Interval between shots
    private float lastShootTime = 0f; // Time of the last shot
    private PlayerCam playerCam;
    public Texture2D redCrosshair;
    public Texture2D blackCrosshair;
    private GameObject akFPS;
    private GameObject akTPS;
    private GameObject pauseGameObject;
    private Animator gunAnimator;
    private bool isADSing = false;

    void Start()
    {
        Debug.Log("Game started");
        score = 0;
        tmp_score.text = "Coins: " + score.ToString();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        orientation = GameObject.FindWithTag("Orientation").transform;
        firstPersonCamera = GameObject.FindWithTag("PlayerCam").GetComponent<Camera>();
        thirdPersonCamera = GameObject.FindWithTag("ThirdPersonCam").GetComponent<Camera>();
        playerCam = GameObject.FindWithTag("PlayerCam").GetComponent<PlayerCam>();
        akFPS = GameObject.FindWithTag("AKfps");
        akTPS = GameObject.FindWithTag("AKtps");
        gunAnimator = akFPS.GetComponent<Animator>();
        pauseGameObject = GameObject.FindWithTag("Pause");
        pauseGameObject.SetActive(false);
    }

    void Update()
    {
        CheckHeight();

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCamera();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
        transform.rotation = orientation.rotation;
        CheckForShooting();
    }

    void OnCollisionStay(Collision collision)
    {
        // Check if the player is grounded by iterating through collision contacts
        foreach (ContactPoint contact in collision.contacts)
        {
            // Check if the contact normal is pointing upwards (indicating contact with a ground surface)
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an object tagged as "Coin"
        if (collision.gameObject.CompareTag("Coin"))
        {
            // Destroy the coin object
            Destroy(collision.gameObject);
            ++score;
            tmp_score.text = "Coins: " + score.ToString();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Reset the grounded flag when the player leaves the ground
        isGrounded = false;
    }

    void CheckHeight()
    {
        if (transform.position.y < -6)
        {
            Debug.Log("Player fell below the floor. Resetting scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void ToggleCamera()
    {
        isFirstPerson = !isFirstPerson;
        firstPersonCamera.enabled = isFirstPerson;
        thirdPersonCamera.enabled = !isFirstPerson;
        if (isFirstPerson)
        {
            akFPS.SetActive(true);
            akTPS.SetActive(false);
        }
        else
        {
            akFPS.SetActive(false);
            akTPS.SetActive(true);
        }
    }
    private void CheckForShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true; // Start shooting
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isShooting = false; // Stop shooting
            gunAnimator.SetBool("IsShooting", false);
            if(!Input.GetMouseButtonDown(1) || !Input.GetMouseButtonUp(1))
                playerCam.crosshairTexture = blackCrosshair;
        }

        if (Input.GetMouseButtonDown(1) && !isADSing)
        {
            isADSing = true;
            gunAnimator.SetBool("IsAds", true);
            if(isFirstPerson)
                playerCam.crosshairTexture = null;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isADSing = false;
            gunAnimator.SetBool("IsAds", false);
            playerCam.crosshairTexture = blackCrosshair;
        }

        // Check if shooting is enabled
        if (isShooting)
        {
            if(!Input.GetMouseButtonDown(1))
                gunAnimator.SetBool("IsShooting", true);
            // Check if enough time has passed since the last shot
            if (Time.time - lastShootTime > shootInterval)
            {
                AudioSource audioSource = firstPersonCamera.GetComponent<AudioSource>();
                if (audioSource.mute)
                {
                    audioSource.mute = false;
                }
                audioSource.Play();
                RaycastHit hit;

                // Shoot using the direction of the camera's forward vector
                if (isFirstPerson)
                {
                    // First-person shooting
                    if (Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out hit, Mathf.Infinity))
                    {
                        Debug.Log(hit.collider.name);
                        if (hit.collider.CompareTag("Zombie"))
                        {
                            if (!Input.GetMouseButtonDown(1))
                                playerCam.crosshairTexture = redCrosshair;
                            ZombieHealth zHealth = hit.collider.GetComponentInChildren<ZombieHealth>();
                            zHealth.TakeDamage(10);
                        }
                    }
                }
                else
                {
                    if (Physics.Raycast(thirdPersonCamera.transform.position, thirdPersonCamera.transform.forward, out hit, Mathf.Infinity))
                    {
                        Debug.Log(hit.collider.name);
                        if (hit.collider.CompareTag("Zombie"))
                        {
                            ZombieHealth zHealth = hit.collider.GetComponentInChildren<ZombieHealth>();
                            zHealth.TakeDamage(10);
                        }
                    }
                }


                lastShootTime = Time.time; // Update the last shoot time
            }
        }
    }
    private void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseGameObject.SetActive(true);
    }
}