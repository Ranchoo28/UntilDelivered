using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeTrigger: TargetFinder
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {   // Inflict damage to the enemy
            HealthScript enemy = other.gameObject.GetComponentInChildren<HealthScript>();
            enemy.TakeDamage(100);
        }
    }
}
