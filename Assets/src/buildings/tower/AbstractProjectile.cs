using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractProjectile : MonoBehaviour
{
    protected float damage = 2;
    protected Transform target;

    public void SetDamage(float damage) {
        this.damage = damage;
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }

}
