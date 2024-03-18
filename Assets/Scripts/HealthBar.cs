using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    private float health;
    private float lerpSpeed = 0.05f;

    public GameObject gameOverObject;
    public GameObject tryAgainObject;
    public GameObject mouseVisibilityObject;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
        easeHealthSlider.value = maxHealth;
        healthSlider.maxValue = maxHealth;
        gameOverObject.SetActive(false);
        tryAgainObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (healthSlider.value > health)
        {
            healthSlider.value = health;
            if(healthSlider.value == 0)
            {
                gameOverObject.SetActive(true);
                tryAgainObject.SetActive(true);
                Destroy(mouseVisibilityObject);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (easeHealthSlider.value > healthSlider.value)
        {
            float targetValue = health - 10f; // Adjust the offset value as needed

            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, targetValue, lerpSpeed);
        }
        
        if (healthSlider.value < health)
        {
            healthSlider.value = health;
        }
    }


    public void takeDamage(float damage)
    {
        health -= damage;
    }

    public void increaseHealth()
    {
        if (health < 100)
        {
            health += 20;
            easeHealthSlider.value = health;
            if (health > 100)
            {
                health = 100;
                easeHealthSlider.value = health;
            }
        }
    }
}