using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    private Camera camera;
    private Slider healthSlider;
    private AbstractClientCombat clientCombat;
    private bool isDead = false;
    public static event Action<GameObject> OnEnemyDeath;


    void Start() {
       Initialize();
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + camera.transform.forward);
    }

    private void Initialize() {
        camera = Camera.main;
        clientCombat = GetComponentInParent<AbstractClientCombat>();
        healthSlider = GetComponent<Slider>();
        healthSlider.value = healthSlider.maxValue = clientCombat.maxHealth;
    }

    public void TakeDamage(float damage) {
        if(isDead) return;
        if (clientCombat.currentPhase == 0)
        {

            healthSlider.value = clientCombat.currentHealth -= damage * 0.8f;

        }
        else
        {
            healthSlider.value = clientCombat.currentHealth -= damage;
            if (clientCombat.currentHealth <= 0)
            {
                isDead = true;
                OnEnemyDeath?.Invoke(clientCombat.gameObject);
                PlayerInfo.GetInstance().addGold(clientCombat.gold);
                StartCoroutine(DestroyEnemy());                     // fatta perchè altrimenti non parte l'animazione di morte
            }
        }
    }


    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(1);
        Destroy(clientCombat.gameObject);
    }

    //get the current health of the client
    public float getCurrentHealth()
    {
        return clientCombat.currentHealth;
    }

    public Image getSliderImage()
    {
        return healthSlider.fillRect.GetComponent<Image>();
    }
}