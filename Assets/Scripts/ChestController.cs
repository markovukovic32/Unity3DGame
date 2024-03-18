using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestController : MonoBehaviour
{
    public GameObject healthPrefab; // Reference to the health prefab
    public GameObject coinPrefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform[] spawnPoints;
    public GameObject[] spawnItems;
    private bool isPlayerInSphere;
    private Animator chestAnimator;
    private bool chestOpened;
    private float chestOpenTime; // Variable to store the time when the chest is opened
    public TextMeshProUGUI message;

    void Start()
    {
        isPlayerInSphere = false;
        chestAnimator = GetComponent<Animator>();
        chestOpened = false;
        spawnPoints = new Transform[] { spawnPoint1, spawnPoint2, spawnPoint3 };
        spawnItems = new GameObject[] { healthPrefab, coinPrefab };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isPlayerInSphere && !chestOpened)
        {
            message.text = "";
            chestOpened = true;
            chestOpenTime = Time.time; // Store the current time when the chest is opened
            chestAnimator.Play("OpenChest");
            StartCoroutine(CloseChestDelayed());

            // Spawn a health object
            SpawnHealth();
        }
    }

    // Coroutine to close the chest after 5 seconds
    IEnumerator CloseChestDelayed()
    {
        yield return new WaitForSeconds(5f);
        chestAnimator.Play("CloseChest");

        // Wait for the "CloseChest" animation to finish
        yield return new WaitForSeconds(chestAnimator.GetCurrentAnimatorClipInfo(0).Length);

        // Set chestOpened to false after the animation is over
        chestOpened = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!chestOpened)
            {
                message.text = "Press F to open chest.";
            }
            isPlayerInSphere = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            message.text = "";
            isPlayerInSphere = false;
        }
    }

    // Method to retrieve the time when the chest was opened
    public float GetChestOpenTime()
    {
        return chestOpenTime;
    }

    // Method to spawn a health object
    private void SpawnHealth()
    {
        int randomNumber = Random.Range(0, 2);
        GameObject randomObject = spawnItems[randomNumber];
        if (randomObject == null)
        {
            Debug.Log("Health prefab is null");
            return;
        }
        randomNumber = Random.Range(0, 3);
        Transform spawnPoint = spawnPoints[randomNumber];

        // Calculate the direction vector pointing in front of the chest

        Vector3 spawnPosition = spawnPoint.position; // Calculate spawn position

        Instantiate(randomObject, spawnPosition, Quaternion.identity);

    }

}
