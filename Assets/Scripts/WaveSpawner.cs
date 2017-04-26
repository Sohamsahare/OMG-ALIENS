using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public enum spawnState { Spawning, Waiting, Counting};  // defines three states for the wave spawner
    [System.Serializable]               //allows us to change values of the instances inside the unity inspector
    public class Wave
    {
        public string name;
        public Transform enemy;    //prefab to be instantiated at the spawnpoints.
        public int count;
        public float spawnRate;


    }
    [SerializeField]
    private GameObject levelCompletedObj;      // obj loads the level completed scene once you clear all waves.
    [SerializeField]
    private GameObject WaveUiObj;
    public Wave[] waves;
    public Transform[] spawnPoints;
    private spawnState state = spawnState.Counting;
    public spawnState State
    {
        get { return state; }
    }
    private int nextWave = 0;
    public int NextWave
    {
        get { return waves[nextWave].count; }
    }
    public float timeBwWaves = 5f;
    private float waveCountdown ;
    public float WaveCountdown
    {
        get { return waveCountdown; }
    }
    private float searchCountdown =1f;      //interval between searching for all enemies if they are dead
    private void Start()
    {
        waveCountdown = timeBwWaves;
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawnpoints!!!!!");
        }

    }
    bool isEnemyAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
            else return true;

        }
        else return true;
    }
    private void Update()
    {
        if(state == spawnState.Waiting)
        {
            //check if enemies are still alive
            if (!isEnemyAlive())
            {
                //begin a new round
                WaveCompleted();

            }else
            {
                return;
            }

        }
        if (waveCountdown <= 0)
        {
            if(state != spawnState.Spawning)
            {
                StartCoroutine( SpawnWave( waves[ nextWave ] ) );
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }
    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        state = spawnState.Counting;
        waveCountdown = timeBwWaves;
        if(nextWave + 1 > waves.Length - 1)
        {
            nextWave = -99;
            //Debug.Log("We've completed all waves! Looping...");
            levelCompletedObj.SetActive(true);
            WaveUiObj.SetActive(false);
            //TODO: Insert Level Completed Scene!
        }
        if (nextWave < -1)
            return;
        else
            nextWave++;
            
    }
    IEnumerator SpawnWave(Wave _wave)
    {
        //spawn waves
        Debug.Log("WAVE: SPAWNING WAVE!");
        state = spawnState.Spawning;
        for(int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1/_wave.spawnRate);     //waits until player kills all the enemies to spawn the new wave.
        }
        state = spawnState.Waiting;

        yield break;
    }
    void SpawnEnemy(Transform _enemy)
    {
        //spawn enemy
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        Instantiate(_enemy, _sp.position, _sp.rotation);
        //Debug.Log("Spawning Enemy" + _enemy.name);
    }
}
