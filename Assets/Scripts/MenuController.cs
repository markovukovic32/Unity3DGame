using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuController : MonoBehaviour
{
    GameObject pauseGameObject;
    void Start()
    {
        pauseGameObject = GameObject.FindWithTag("Pause");
    }

    void Update()
    {
        
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pauseGameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        InvokeRepeating("CountDown", 0f, 1f);
    }
}
