﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;
    [SerializeField] float timeBetweenWaves = 5f;
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

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for(int enemyCount=0; enemyCount < waveConfig.getnumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(
            waveConfig.getEnemyPrefab(),
            waveConfig.getWaypoints()[0].transform.position,
            Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);

            yield return new WaitForSeconds(waveConfig.getTimeBetweenSpawns());
        }
        

    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = 0; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];

            yield return new WaitForSeconds(timeBetweenWaves);

            StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }


    }
}
