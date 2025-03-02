using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractClientCombat : MonoBehaviour
{
    public short attackRange { get; set; }

    public bool canAttack { get; set; }
    public float maxHealth { get; set; }

    public float damage { get; set; }

    public int exp { get; set; }
    public int gold { get; set; }

    public float currentHealth { get; set; }
    public float attackSpeed { get; set; }
    protected HealthScript healthScript { get; set; }
    [Header("--- Combat Attributes ---")]
    [SerializeField] protected GameObject projectile;
    protected NavMeshAgent agent { get; set; }
    protected bool isLookingAtPosta { get; set; }

    protected abstract void Initialize();

    protected abstract IEnumerator Attack();
}
