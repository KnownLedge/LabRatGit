using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressSounds : MonoBehaviour
{
    
    public AudioSource heartBeatSound;

    void Start()
    {
        //EnableHeartBeat();
    }


    public void EnableHeartBeat(){
        if(heartBeatSound.isPlaying == false){
            heartBeatSound.Play();
        }
    }

    public void DisableHeartBeat(){
        heartBeatSound.Stop();
    }
}
