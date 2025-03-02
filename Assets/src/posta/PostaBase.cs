using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostaBase : AbstractPosta 
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject loseScreen;
    private float currentHealth;
    void Start()
    {
       healthSlider.value = healthSlider.maxValue = maxHealth = 100;
    }

    private void Update()
    {
        currentHealth = healthSlider.value;
        if (currentHealth <= 0)
        {
            Time.timeScale = 0;
            loseScreen.SetActive(true);
        }
    }

    public override float GetCurrentHealth()
    {
        return healthSlider.value;
    }

    public override void TakeDamage(float damage)
    {
        healthSlider.value -= damage;
        
        if (healthSlider.value < 0)
        {
            healthSlider.value = 0;
        }
    }
}
