using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : AbstractAttackBuilding
{
    
    public GameObject gun;
    public GameObject projectilePrefab; // Prefab del proiettile
    public Transform firePoint; // Punto da cui partono i proiettili

    private List<GameObject> enemiesInRange = new List<GameObject>();
    
    private GameObject currentTarget;

    float turningSpeed = 10f; 
    private float angleTurningAccuracy = 15f;
    private bool isReloading = false;
    [SerializeField] private float attackSpeed = 1.2f;

    [SerializeField] private int price = 50;

    public override int Price {
        get => price;
        set => price = value;
    }


    void Update()
    {
        if (currentTarget != null)
        {
            // Calcola la direzione orizzontale
            Vector3 directionToTarget = (currentTarget.transform.position - gun.transform.position).normalized;

            // Calcola la rotazione desiderata solo in orizzontale
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Applica la rotazione lentamente con Slerp
            gun.transform.rotation = Quaternion.Slerp(
                gun.transform.rotation,
                targetRotation,
                Time.deltaTime * turningSpeed
            );
           
            // Verifica se la gun è abbastanza allineata per sparare
            if (Vector3.Angle(directionToTarget, gun.transform.forward) < angleTurningAccuracy && !isReloading)
            {
                isReloading = true;
                StartCoroutine(Fire());
            }
        }
        else
        {
            UpdateTarget();
        }
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(col.gameObject);
            
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(col.gameObject);
            currentTarget = null;
            UpdateTarget();
        }
    }

    private void UpdateTarget()
    {
        // Rimuovi i riferimenti nulli
        enemiesInRange.RemoveAll(enemy => enemy == null);

        if (enemiesInRange.Count == 0)
        {
            currentTarget = null;
            return;
        }

        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        currentTarget = closestEnemy;
    }

    private IEnumerator Fire()
    {
        if (currentTarget != null) // Continua a sparare finché c'è un bersaglio valido
        {
            // Istanzia il proiettile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Assegna il bersaglio al proiettile
            AbstractProjectile projectileScript = projectile.GetComponent<AbstractProjectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(currentTarget.transform);
            }
            // Aspetta prima di sparare di nuovo
            yield return new WaitForSeconds(attackSpeed); // Cambia 1f con il tuo tempo di ricarica
            isReloading = false;
        }
    }

}
