using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int fallBoundary = -20;

    [System.Serializable]
    public class EnemyStats
    {
        
        public int maxHealth = 100;
        private int _curHealth;
        public int damageAmount=40;
        public int curHealth
        {
            get { return _curHealth; }              //gets current health
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }  //sets current health

        }
        public void Init()
        {
            curHealth = maxHealth;      //matches current health with max health for the enemy.
        }
    }
    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;
    public Transform deathParticles;
    public EnemyStats Stats = new EnemyStats();
    [Header("Optional")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    private void Start()
    {
        Stats.Init();
        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(Stats.curHealth, Stats.maxHealth);
        }
        if(deathParticles == null)
        {
            Debug.LogError("No Death Particles referenced to enemy.");
        }
    }
   
    
    public void DamageEnemy(int damage)
    {
       Stats.curHealth -= damage;
        
        if (Stats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(Stats.curHealth, Stats.maxHealth);
        }
    }
    void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if(_player != null)
        {
            _player.DamagePlayer(Stats.damageAmount);
            DamageEnemy(99999);
        }
        
    }
}
