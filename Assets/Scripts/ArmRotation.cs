
using UnityEngine;

public class ArmRotation : MonoBehaviour {
    public int rotationOffsetZ = 90;
	// Update is called once per frame
	void Update () {
        //subtracting the position of the player from the mouse pointer to get the slope of the line joining them
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
        difference.Normalize();                 //normalizes x y and z proportion down so that when we add them we get 1.(x+y+z = 1)
        //finding the slope of the line

        float rotZ = Mathf.Atan2(difference.y, difference.x )* Mathf.Rad2Deg; //Atan2 returns radians so we use Rad2Deg to return the slope in degrees
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffsetZ);	
	}
}
