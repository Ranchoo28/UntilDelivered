using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : AbstractProjectile
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float speed = 10f;
    private float elapsedTime = 0f;
    private bool isInitialized = false;

    void Update()
    {
        if (!isInitialized) return;

        // Option 1: Linear movement using MoveTowards
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // If the projectile reaches the target, destroy it
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }

        // Optional: Make the projectile face its movement direction
        if (transform.position != targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Const.POSTA_TARGET_TAG))
        {
            if (other.name == "Poste")
                other.GetComponent<AbstractPosta>().TakeDamage(damage);
            else
                other.GetComponentInParent<PacksBehaviour>().TakeDamage(damage);
             Destroy(gameObject);
        }
        else if (other.CompareTag("Kamikaze"))
            other.GetComponentInParent<Kamikaze>().TakeDamage(damage);
       
    }

    public void CreateProjectile(GameObject prefab, Vector3 position, Quaternion rotation, float customDamage)
    {
        GameObject projectileObject = Instantiate(prefab, position, rotation);
        ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();

        if (projectile != null)
        {
            projectile.damage = customDamage;
            projectile.startPosition = position;

            // Find the target (posta) and set the target position
            GameObject target = GameObject.FindWithTag(Const.POSTA_TARGET_TAG);
            if (target != null)
            {
                projectile.targetPosition = target.transform.position;
                projectile.isInitialized = true;
            }
        }
    }
}