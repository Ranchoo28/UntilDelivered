using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Kamikaze : AbstractClientCombat
{

    // Collider that will initiate the explosion
    private KamikazeTrigger kamikazeTrigger;
    // Duration of the explosion (Time of Explosion)
    private const float TOE = 1f;

    void Awake(){
        maxHealth = 5f;
        kamikazeTrigger = GetComponentInChildren<KamikazeTrigger>();
        Initialize();
    }

    protected override void Initialize(){
        currentHealth = maxHealth;
        healthScript = GetComponentInChildren<HealthScript>();
    }

    public void TakeDamage(float damage){
        healthScript.TakeDamage(damage);
    }

    protected override IEnumerator Attack(){
        kamikazeTrigger.enableTrigger();
        // Wait for the explosion to finish and destroy the game object
        yield return new WaitForSeconds(TOE);
        Destroy(gameObject);
    }

    /**
     * Wrapper method to trigger the explosion
    */
    public void explode(){
        StartCoroutine(Attack());
    }
}