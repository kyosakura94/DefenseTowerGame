using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Transform spawnPoint;
    public float timeBetweenWaves = 5f;
    private float countdown = 2f;
    BuildManager buildManager;

    public Wave[] waves;
    private int waveIndex = 0;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    private void Update()
    {
        GameObject[] enemylist = GameObject.FindGameObjectsWithTag("Enemy");
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        switch (buildManager.data.ending)
        {
            case Types.Ending.Firsttoflag:

                if (waveIndex == waves.Length)
                {                 
                    if (enemylist.Length <= 0)
                    {
                        Debug.Log("YOU WIN!");
                    }
                }

                break;
            case Types.Ending.lastunitstanding:

                if (waveIndex >= waves.Length)
                {
                    waveIndex = 0;
                }
                break;
            default:
                break;
        }

    }


    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];
        for (int i = 0; i < wave.count; i++)
        {
            Spawn(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveIndex++;
    }

    void Spawn(GameObject enemy)
    {
        Vector3 pos = spawnPoint.position + new Vector3(0,1,0);

        Instantiate(enemy, pos, spawnPoint.rotation);
    }

    
}
