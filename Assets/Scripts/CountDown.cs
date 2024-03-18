using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    public TextMeshProUGUI countDownText;
    private int number = 3;
    private float timer = 0f;

    void Start()
    {
        Time.timeScale = 0f;
        countDownText.text = "3";
        number = 3;
        timer = Time.realtimeSinceStartup;
    }

    void Update()
    {
        if (Time.realtimeSinceStartup - timer >= 1f)
        {
            timer = Time.realtimeSinceStartup;
            CountDownMethod();
        }
    }

    private void CountDownMethod()
    {
        if (number > 0)
        {
            countDownText.text = number.ToString();
            number--;
        }
        else if (number == 0)
        {
            Time.timeScale = 1f; // Set time scale to normal
            countDownText.text = "Go!";
            number--;
        }
        else
        {
            countDownText.text = "";
            enabled = false; // Disable this script after countdown is finished
        }
    }
}
