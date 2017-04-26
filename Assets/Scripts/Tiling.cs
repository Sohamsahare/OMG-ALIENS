using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour {
    public int offsetX = 2;             //so we don't get an weird errors
    public bool hasARightBuddy = false;//checking if we need to instantiate the right and left backgrounds
    public bool hasALeftBuddy = false;
    public bool reverseScale = false; // for background elements that are not made tilable
    private float spriteWidth = 0f; //width of our elements
    private Camera cam;
    private Transform myTransform;

    private void Awake(){
        cam = Camera.main;
        myTransform = transform;
    }
    // Use this for initialization
    void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
        //to check if we still need buddies. If not do nothing
		if(hasALeftBuddy == false || hasARightBuddy == false){
            //calculate the camera's extent. Half the width of what camera can see in world coordinates
            float camHorizontalExtent = cam.orthographicSize * Screen.width / Screen.height;
            //calculate the x position where the camera can see the edge of the sprite
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtent;
            float edgeVisiblePositionLeft = (myTransform.position.y - spriteWidth / 2) + camHorizontalExtent;
            if(cam.transform.position.x >= (edgeVisiblePositionRight - offsetX)  && hasARightBuddy== false){
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if(cam.transform.position.x <= (edgeVisiblePositionLeft + offsetX) && hasALeftBuddy == false) {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
	}
    //method that creates a buddy wherever required
    void MakeNewBuddy(int rightOrleft){
        //calculating the new position for our new buddy
        Vector3 new_position = new Vector3(myTransform.position.x + spriteWidth * rightOrleft,myTransform.position.y,myTransform.position.z);
        //checking if we can see the edge of the element and then calling makenewbuddy if we can
        Transform newBuddy = Instantiate(myTransform, new_position, myTransform.rotation) as Transform;
        //if not tilable reverse the x of the background element to get a seamless effect
        if (reverseScale){
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1,newBuddy.localScale.y,newBuddy.localScale.z);
        }
        newBuddy.parent = myTransform.parent;
        if(rightOrleft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
