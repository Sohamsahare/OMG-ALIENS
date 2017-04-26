using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
    public static GameMaster gm;
    [SerializeField]
    private int maxLives;
    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 3.5f;
    public Transform spawnPrefab;
    public Transform enemyDeathParticles;
    public CameraShake camShake;
    [SerializeField]
    private GameObject gameOverUIObj;
    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
        
    }
    void Start()
    {
        if(camShake == null)
        {
            Debug.LogError("No camera shake referenced!");
        }
        _remainingLives = maxLives;
    }

    public IEnumerator RespawnPlayer()
    {
        AudioSource respawnSound = gm.GetComponent<AudioSource>();
        respawnSound.Play();
        //Get how to add sounds to scripts.
        yield return new WaitForSeconds (spawnDelay);
        //Debug.Log("TODO:Add Spawn Particles.");
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform cloneParticles = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(cloneParticles.gameObject, 3);
    }
    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject); // Destroy() expects a gameobject to destroy
        _remainingLives--;
        if(_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm.RespawnPlayer());
        }
        
    }

    public void EndGame()
    {
        Debug.Log("GAME OVER!");
        gameOverUIObj.SetActive(true); 
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }
    public void _KillEnemy(Enemy _enemy)
    {
        if(_enemy == null)
        {
            Debug.LogError("NO ENEMY? WHAT?");
        }
        Transform _clone =  Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 3f); 
        camShake.Shake(_enemy.shakeAmount,_enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
