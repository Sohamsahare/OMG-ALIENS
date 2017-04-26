using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

    [SerializeField]
    private WaveSpawner spawner;
    [SerializeField]
    private Animator waveAnimator;
    [SerializeField]
    private Text waveCountdownText;
    [SerializeField]
    private Text waveCountText;
    private WaveSpawner.spawnState previousState;      //to check if the last phase was similar to the current phase

    //initialization
    private void Start()
    {
        if(spawner == null)
        {
            Debug.LogError("No spawner referenced!!");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No waveAnimator referenced!!");
            this.enabled = false;
        }
        if (waveCountdownText == null)
        {
            Debug.LogError("No waveCountdownText referenced!!");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("No waveCountText referenced!!");
            this.enabled = false;
        }

    }
    private void Update()
    {
        switch (spawner.State)
        {
            case WaveSpawner.spawnState.Counting:
                WaveCountdownUI();
                break;
            case WaveSpawner.spawnState.Spawning:
                WaveSpawningUI();
                break;
        }
        previousState = spawner.State;
    }

    private void WaveSpawningUI()
    {
        if(previousState != WaveSpawner.spawnState.Spawning)
        {
            waveAnimator.SetBool("WaveIncoming", true);
            waveAnimator.SetBool("WaveCountdown", false);
            //Debug.Log("Spawning!");
        }
        waveCountText.text = ((int)spawner.NextWave).ToString();
        
    }

    private void WaveCountdownUI()
    {
        if (previousState != WaveSpawner.spawnState.Counting)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
            //Debug.Log("Counting!");
        }
        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
    }
}
