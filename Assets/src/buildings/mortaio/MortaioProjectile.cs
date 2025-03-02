using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortaioProjectile : MonoBehaviour {
    private float damage = 1f;
    public float explosionRadius = 5.5f;
    public float explosionForce = 10f;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(Const.ENEMY_TAG)) {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider hit in hitEnemies) {
                if (hit.CompareTag(Const.ENEMY_TAG)) {
                    HealthScript enemy = hit.GetComponentInChildren<HealthScript>();
                    if (enemy != null) {
                        enemy.TakeDamage(damage);
                    }

                    /* La vogliamo implementare?
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null) {
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                    */
                }
            }

            Destroy(gameObject);
        }

        StartCoroutine(DestroyProjectile());
    }

    public void SetDamage(float damage) {
        this.damage = damage;
    }

    private IEnumerator DestroyProjectile() {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}