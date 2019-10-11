using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }


    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator SpawnAllWaves()
    {
        int waveCount = waveConfigs.Count;
        for (int i = startingWave; i < waveCount; i++)
        {
            var currentWave = waveConfigs[i];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
    {
        int enemyCount = currentWave.GetNumberOfEnemies();
        for (int i = 0; i < enemyCount; i++)
        {
            var newEnemy = Instantiate(
                currentWave.GetEnemyPrefab(),
                currentWave.GetWaypoints()[0].transform.position,
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPath>().SetWaveConfig(currentWave);
            yield return new WaitForSeconds(currentWave.GetTimeBetweenSpawns());
        }
        
    }

}
