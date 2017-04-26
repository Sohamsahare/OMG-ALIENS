using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int fallBoundary = -20;
    [System.Serializable]
    public class PlayerStats{
        public int maxHealth = 100;
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }
        public void Init()
        {
            curHealth = maxHealth;
        }
    }
    
    public PlayerStats Stats = new PlayerStats();
    [SerializeField]
    private StatusIndicator statusIndicator;
    [SerializeField]
    private float blinkDistance = 100;
    public float cooldownBlink;
    //private float timeOnBlink = 0f;
    private bool isMousePressed = false;
    //private bool justBlinked = false ;
    private void Start()
    {
        Stats.Init();
        if(statusIndicator == null)
        {
            Debug.LogError("STATUS INDICATOR: NONE FOUND!!");
        }
        else
        {
            statusIndicator.SetHealth(Stats.curHealth, Stats.maxHealth);
        }
    }
    private void Update()
    {
        if(transform.position.y <= fallBoundary)
        {
            DamagePlayer(99999);
        }
        if (true)//Time.time - timeOnBlink > 2f )
        {
            if (Input.GetButtonDown("Fire2"))
            {
                //TODO:INSERT CODE FOR AIMING HERE!
                //if (isMousePressed)
                    Blink();
            }
        }
    }
    private void OnMouseDown()
    {
        isMousePressed = true;
    }
    public void DamagePlayer(int damage ){
        Stats.curHealth -= damage;
        if(Stats.curHealth <= 0)
        {
            GameMaster.KillPlayer(this);
        }
        statusIndicator.SetHealth(Stats.curHealth, Stats.maxHealth);
    }
    private void Blink()
    {
        Vector2 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 directionToBlink = (camPos - currentPos);
        if (directionToBlink.x < 10)
        {
            Vector3 setPos1 = new Vector3(directionToBlink.x + currentPos.x, directionToBlink.y + currentPos.y, transform.position.z);
            transform.position = setPos1;
            Debug.Log("I Blinked");
            //timeOnBlink = Time.time;
            //justBlinked = true;
        }
        else
        {
            directionToBlink = directionToBlink.normalized * blinkDistance;
            Vector3 setPos = new Vector3(directionToBlink.x + currentPos.x, directionToBlink.y + currentPos.y, transform.position.z);
            transform.position = setPos;
            Debug.Log("I Blinked");
            //timeOnBlink = Time.time;
            //justBlinked = true;
        }
    }

}
