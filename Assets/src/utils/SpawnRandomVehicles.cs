using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 2f; //se non funziona, guarda l'ispector 
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float destroyAfterSeconds = 3f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        if (prefabs.Length == 0) return;

        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject prefabToSpawn = prefabs[randomIndex];

        Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position;
        Quaternion spawnRotation = Quaternion.Euler(0, 180, 0);

        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, spawnRotation);

        // Aggiungi lo script MoveAndDestroy se non è già presente
        MoveAndDestroy moveScript = spawnedObject.AddComponent<MoveAndDestroy>();
        moveScript.SetParameters(moveSpeed, destroyAfterSeconds);
    }
}
