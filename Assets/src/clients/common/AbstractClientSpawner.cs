using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class AbstractClientSpawner : MonoBehaviour {
    private Canvas canvas;
    protected float timeBetweenWaves { get; set; } = 10f;
    protected List<List<GameObject>> waves = new List<List<GameObject>>();
    protected short activeEnemies = 0;
    private short currentWaveIndex = 0;
    private AbstractPosta posta;
    [SerializeField] private Text waveTextUI;
    public static event Action<GameObject> OnSpawnerStop;
    public List<GameObject> spawners = new List<GameObject>();
    [SerializeField] private GameObject winScreen;

    void Start() {
        posta = GameObject.FindObjectOfType<PostaBase>();
        Initialize();   
    }

    void OnDestroy() {
        HealthScript.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void Initialize()
    {
        currentWaveIndex++;
        if (waveTextUI != null) waveTextUI.text = "Wave " + currentWaveIndex;
        canvas = GetComponentInChildren<Canvas>();
        HealthScript.OnEnemyDeath += HandleEnemyDeath;
        InstantiateWaves();
        SpawnWaves();
    }

    private void SpawnWaves() {
        foreach (var wave in waves) activeEnemies += (short)wave.Count;

        activeEnemies *= (short)spawners.Count;

        foreach (var spawner in spawners) {
            StartCoroutine(spawnWave(spawner));
        }

    }

    private IEnumerator spawnWave(GameObject spawner)
    {
        for (short waveIndex = 0; waveIndex < waves.Count; waveIndex++) {
            List<GameObject> currentWave = waves[waveIndex];
            currentWaveIndex = waveIndex;
            currentWaveIndex++;
            if (waveTextUI != null) waveTextUI.text = "Wave " + currentWaveIndex;
            for (short i = 0; i < currentWave.Count; i++) {
                yield return new WaitForSeconds(GetSpawnDelay(waveIndex, i));
                Vector3 spawnPosition = new Vector3(spawner.transform.position.x, spawner.transform.position.y + 1.01f, spawner.transform.position.z + 5);
                GameObject clientPrefab = currentWave[i];
                Instantiate(clientPrefab, spawnPosition, transform.rotation);
            }

            yield return new WaitForSeconds(timeBetweenWaves);

        }
    }

    private void HandleEnemyDeath(GameObject enemy) {
        activeEnemies--;
        Debug.Log($"Active enemies: {activeEnemies}");
        if (activeEnemies == 0)
        {
            PlayerInfo.GetInstance().addDiamond(3);
            PlayerInfo.GetInstance().CompleteLevel(SceneManager.GetActiveScene().name);
            winScreen.SetActive(true);
            SaveSystem.GetInstance().SaveGame();
        }
    }

    // ---- Metodi astratti ----
    protected virtual string SetWaveStartText(int waveIndex) {
        return $"Wave {waveIndex + 1}";
    }

    protected abstract float GetSpawnDelay(int waveIndex, int clientIndex);
    protected abstract Vector3 GetSpawnPosition(int waveIndex, int clientIndex);

    /// <summary>
    /// Aggiunge i nemici alla wave. In automatico calcola il numero di nemici e si adatta.
    /// <para> Aggiungere anche il timeBetweenWaves. </para>
    /// Esempi vari di Add:
    /// <para> 1) AddEnemiesToWave(0, Enumerable.Repeat(clientPrefab, 10).ToArray()) </para>
    /// <para> 2) AddEnemiesToWave(1, new GameObject[] { bossPrefab }) </para>
    /// </summary>
    protected abstract void InstantiateWaves();
    protected void AddEnemiesToWave(int waveIndex, GameObject[] enemies) {
        if (waveIndex >= waves.Count) {
            waves.Add(new List<GameObject>(enemies));
        }
        else {
            waves[waveIndex] = new List<GameObject>(enemies);
        }
    }
    }
