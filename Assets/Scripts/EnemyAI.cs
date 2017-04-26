using UnityEngine;
using System.Collections;
using Pathfinding;  //Imports a* pathfinding into the project.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
    
public class EnemyAI : MonoBehaviour {

    public Transform target;        //what to chase?
    public float updateRate = 2f;   //How fast we will update our path per second
    //Caching
    private Seeker seeker;      
    private Rigidbody2D rb;
    // the calculated path
    public Path path;
    //the AI's speed
    public float speed=300f;
    public ForceMode2D fMode; //way to change from force to impulse. controls how force is applied to the object.
    [HideInInspector]       //public yet not visible in the inspector
    public bool pathIsEnded = false;
    public float nextWayPointDistance = 3f; //max distance from the AI to the waypoint for it to continue to the next waypoint.
    private int currentWaypoint = 0;        //the waypoint we are currently moving towards
    private bool searchingForPlayer = false;  
    void Start(){
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        //Starts a path from the Current position of the enemyAI to the target and returns the result to the method OnPathComplete
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());
    }
    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if(sResult == null)
        {
            yield return new WaitForSeconds(0.5f);

            StartCoroutine(SearchForPlayer());
        }else
        {
            searchingForPlayer = false;
            target = sResult.transform;
            StartCoroutine(UpdatePath());

        }
    }
    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            //return false;
        }
        //Starts a path from the Current position of the enemyAI to the target and returns the result to the method OnPathComplete
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(1f / updateRate);       //defines how many times a second we update our path.
        StartCoroutine(UpdatePath());       //calls itself to check for path again.

    }
        //Write some MORE!!
    public void OnPathComplete(Path p){
            Debug.Log("We got a Path!Did it have an error?" +p.error);
            if (!p.error){
                path = p;
                currentWaypoint = 0;
            }
        
    }
    //updates every fix number of times. Great for physics implementation.
    private void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        //TODO: always look at the player.
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;
            Debug.Log("Path Has Ended!");
            pathIsEnded = true;
            return;

        }
        pathIsEnded = false;
        //Direction to the next waypoint.
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;     // use fixedDeltaTime used because its in FixedUpdate. Updates every fix number of times.
        //Move the enemy.
        rb.AddForce(dir,fMode);
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWayPointDistance)
        {
            currentWaypoint++;
            return;
        }

    }


}
