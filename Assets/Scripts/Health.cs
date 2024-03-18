using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameObject healthBarObject = GameObject.FindWithTag("HealthBarPlayer");
            GameObject mouseVisibilityObject = GameObject.FindWithTag("MouseVisibility");

            if (healthBarObject != null && mouseVisibilityObject != null)
            {
                HealthBar healthBar = healthBarObject.GetComponent<HealthBar>();
                if (healthBar != null)
                {
                    healthBar.increaseHealth();
                }
                else
                {
                    Debug.LogError("HealthBar script not found on the GameObject with the 'HealthBar' tag!");
                }
            }
            else
            {
                Debug.LogError("GameObject with the 'HealthBar' tag not found!");
            }
        }
    }
}
