using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PacchiFragiliProjectile : AbstractProjectile {
    [SerializeField] private GameObject detritoPrefab;
    private float debrisSpawnRadius = 1.5f;
    private float speed = 50f;

    void Start() {
        this.damage = 1;
    }

    void Update() {
        if(target == null) {
            Destroy(gameObject);
            return;
        }
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == Const.ENEMY_TAG) {
            other.gameObject.GetComponentInChildren<HealthScript>().TakeDamage(damage);
            SpawnDetriti();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SpawnDetriti() {
        for (short i = 0; i < 4; i++) {
            Vector2 randomPos = Random.insideUnitCircle * debrisSpawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomPos.x, -0.5f, randomPos.y);
            Instantiate(detritoPrefab, spawnPosition, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    IEnumerator DestroyProjectile() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}