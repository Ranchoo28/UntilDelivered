using System.Linq;
using UnityEngine;

public class LevelOneSpawner : AbstractClientSpawner {
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private GameObject bossPrefab;

    protected override float GetSpawnDelay(int waveIndex, int clientIndex) {
        return Random.Range(1f, 2.5f);
    }

    protected override Vector3 GetSpawnPosition(int waveIndex, int clientIndex) {
        return new Vector3(transform.position.x, transform.position.y+1.01f, transform.position.z + 5);
    }

    protected override void InstantiateWaves()
    {
        timeBetweenWaves = 15f;

        int[] enemies_number = { 5, 10, 0, 20, 30};
        int[] boss_number = { 0, 0, 1, 0, 1 };

        for (int i = 0; i < enemies_number.Length; i++)
        {
            GameObject[] enemies = Enumerable.Repeat(clientPrefab, enemies_number[i]).Concat(Enumerable.Repeat(bossPrefab, boss_number[i])).ToArray();
            AddEnemiesToWave(i, enemies);
        }

          
    }


}