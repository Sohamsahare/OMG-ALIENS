using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float fireRate = 0f;             //for single fire weapons
    public int Damage = 10;
    public LayerMask whatToHit;
    private float timeToFire = 0f;
    private float camShakeAmt = 0.05f;
    private float camShakeLength = 0.1f;
    private CameraShake camShake;
    private Transform firePoint;
    public Transform bulletTrailPrefab;
    public Transform hitPrefab;
    float timeToSpawnEffect = 0f;
    public float effectSpawnRate = 10f;
    public Transform muzzleFlashPrefab;
	// Use this for initialization
	void Awake () {
        firePoint = transform.FindChild("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("No Firepoint? WHAT?!");
        }

	}
    private void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null)
        {
            Debug.LogError("CameraShake component not found!!");
        }
    }
    // Update is called once per frame
    void Update () {
        //if the gun is burst fire or single fire
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }

        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();

            }
        }
	}
    void Shoot()
    {
        //Debug.Log("Test");
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y); // assigning position of mouse from screen coordinates to coordinates of the world.
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition,(mousePosition - firePointPosition),100,whatToHit);      //casts raycast from the first argument in the direction of second argument
       
        Debug.DrawLine(firePointPosition,( mousePosition - firePointPosition)*100,Color.cyan);
        if(hit.collider!= null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(Damage);
                //Debug.Log("We Hit" + hit.collider.name + " and did " + Damage + " Damage");
            }
        }
        //Effect();
        if (Time.time > timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;
            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999f, 9999f, 9999f);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
            Effect(hitPos,hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
        
    }
    void Effect(Vector3 hitPos,Vector3 hitNormal)
    {
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();
        if(lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }
        Destroy(trail.gameObject, 0.04f);
        if(hitNormal != new Vector3(9999f, 9999f, 9999f))
        {
           Transform hitParticle= Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hitParticle, 0.5f);
        }
        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        //to create this for one frame we use " yeild return 0" which skips a frame and executes the next code which would be "Destro(clone);"
        Destroy(clone.gameObject, 0.02f);
        camShake.Shake(camShakeAmt, camShakeLength);

    }
}
