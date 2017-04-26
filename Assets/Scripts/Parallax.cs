using UnityEngine;

public class Parallax : MonoBehaviour {
    public Transform[] backgrounds;     //List of all back and foregrounds to be parallaxed
    private float[] parallaxScales;     //proportion of camera movement to move the camera backgrounds by
    public float smoothing = 1f;        //Parallaxing amount should be > 0 (How smooth the parallax is going to be)
    private Transform cam ;              //Reference to main camera transform
    private Vector3 previousCamPosition;//position of previous frame
    //Awake is called before Start. It will call the logic before start function and after the game objects are set up. Good for references
    private void Awake(){
        cam = Camera.main.transform;       
    }
    //use this for initialization
	void Start () {
        //previous frame had the position of current camera
        previousCamPosition = cam.position;
        parallaxScales = new float[backgrounds.Length];
        for(int i = 0; i < backgrounds.Length; i++){
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }

	}
	
	//this method is called once per frame
	void Update () {
        for (int i = 0; i < backgrounds.Length; i++) {
            //This creates opposite movement of that of the camera
            float parallax = (previousCamPosition.x - cam.position.x) * parallaxScales[i];
            //set a target x position which is current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            //creates a target position which is the backgrounds current position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            //fade between current position and target position
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        //set the previous cam pos to current cam pos at the end of the frame
        previousCamPosition = cam.position;
	}
}
