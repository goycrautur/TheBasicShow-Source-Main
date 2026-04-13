using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wegachallenge : MonoBehaviour
{
    public void Update()
    {
        curtimesongLenght = wegasource.audioDevice.time;
        if (wegasource.audioDevice.time >= loop1time && getloop1) wegasource.SetAudioTime(0);
        if (wegasource.audioDevice.time >= weg.audClip.length-0.5f && getloop2) wegasource.SetAudioTime(loop2time);
    }
    public void manualUpdate()
    {
        globalWegaSpeed += 2.5f;
        if (gc.notebooks == 1)
        {
            getloop1 = true;
            WEGA.SetActive(true);
            wegasource.ClearQueue(true);
            wegasource.SetLoop(true);
            wegasource.QueueAudio(weg);
        }
        if (gc.notebooks == 7)
        {
            wegasource.SetPitch(1.25f);
        }
        if (gc.notebooks == gc.maxNotebooks)
        {
            getloop1 = false;
            getloop2 = true;
            wegasource.SetPitch(1f);
            wegasource.SetAudioTime(loop2time);
        }
    }
    public GameObject WEGA,rory,maltigi;
    public AudioManagerLiveReaction wegasource;
    public AudioObjectyeah weg;
    public bool getloop1,getloop2;
    public float loop1time,loop2time,curtimesongLenght,globalWegaSpeed;
    public GameControllerScript gc;
}
