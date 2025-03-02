using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPosta: MonoBehaviour
{
    public float maxHealth { get; set; } = 100;
    public float currentHealth { get; set; }
    public abstract void TakeDamage(float damage);
    public abstract float GetCurrentHealth();
}
