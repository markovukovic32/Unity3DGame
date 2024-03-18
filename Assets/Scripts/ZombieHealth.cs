using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZombieHealth : MonoBehaviour
{
    public Slider healthSlider;
    private float health;
    private float maxHealth = 100f;
    private Animator zombieAnimator;
    public GameObject zombie;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        zombieAnimator = zombie.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (healthSlider.value > health)
        {
            healthSlider.value = health;
            if (healthSlider.value == 0)
            {
                zombieAnimator.Play("Death");
                StartCoroutine(WaitForDeathAnimation());
            }
        }
    }
    IEnumerator WaitForDeathAnimation(){
        yield return new WaitForSeconds(zombieAnimator.GetCurrentAnimatorClipInfo(0).Length);
        zombieAnimator.SetBool("DeathTime", false);
        Destroy(zombie);
    }
}
