using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : AbstractProjectile
{
    private float speed = 50f; // Velocità del proiettile

    private void Start()
    {
        damage = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            // Distruggi il proiettile se il bersaglio non esiste più
            Destroy(gameObject);
            return;
        }

        // Muovi il proiettile verso il bersaglio
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HealthScript enemy = other.gameObject.GetComponentInChildren<HealthScript>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
