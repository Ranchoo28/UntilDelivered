using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostaBase : AbstractPosta 
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private int packNumber = 500;
    [SerializeField] private TextMeshProUGUI text;
    private bool packFinished = false;
    private int maxPackNumber;
    private GameObject[] turrets;
    private float currentHealth;
    void Start()
    {
       maxPackNumber = packNumber;
       healthSlider.value = healthSlider.maxValue = maxHealth = 100;
    }

    private void Update()
    {
        text.text = packNumber + " / " + maxPackNumber;
        currentHealth = healthSlider.value;
        if (currentHealth <= 0)
        {
            Time.timeScale = 0;
            loseScreen.SetActive(true);
        }
        if (packFinished && packNumber > 0)
        {
            ReactivateTurrets();
            packFinished = false;
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

    public void packDeliverd()
    {
        packNumber--;
        if (packNumber < 0)
        {
            packNumber = 0;
            packFinished = true;
        }
    }

    public int getRemainingPacks()
    {
        return packNumber;
    }

    private void ReactivateTurrets()
    {
        turrets = GameObject.FindGameObjectsWithTag("Turret");

        foreach (var t in turrets)
        {
            if (t.GetComponent<AbstractAttackBuilding>() != null)
            {
                t.GetComponent<AbstractAttackBuilding>().enabled = true;
            }
        }
    }
}
