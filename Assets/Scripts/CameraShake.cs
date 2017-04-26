using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    public Camera mainCam;
    private float shakeAmount = 0f;
    private void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }
    public void Shake(float amt,float length)
    {
        shakeAmount = amt;
        InvokeRepeating("BeginShake",0,0.01f);
        Invoke("StopShake", length);
    }
    void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;
            float OffsetX = Random.value * shakeAmount*2 - shakeAmount;       //optimizes the shakeamount .
            float OffsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += OffsetX;
            camPos.y += OffsetY;
            mainCam.transform.position = camPos;
        }
    }
    void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
    
}
