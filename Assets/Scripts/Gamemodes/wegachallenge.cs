using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wegachallenge : MonoBehaviour
{
    public void Update()
    {
        curtimesongLenght = wegasource.time;
        if (wegasource.time >= loop1time && getloop1)
        {
            wegasource.time = 0;
        }
        if (wegasource.time >= weg1.length-0.5f && getloop2)
        {
            wegasource.time = loop2time;
        }
    }
    public void manualUpdate()
    {
        globalWegaSpeed += 2.5f;
        if (gc.notebooks == 1)
        {
            getloop1 = true;
            WEGA.SetActive(true);
            wegasource.clip = weg1;
            wegasource.loop = true;
            wegasource.Play();
        }
        if (gc.notebooks == 7)
        {
            wegasource.pitch = 1.25f;
        }
        if (gc.notebooks == gc.maxNotebooks)
        {
            getloop1 = false;
            getloop2 = true;
            wegasource.pitch = 1;
            wegasource.time = loop2time;
        }
    }
    public GameObject WEGA,rory,maltigi;
    public AudioSource wegasource;
    public AudioClip weg1;
    public bool getloop1,getloop2;
    public float loop1time,loop2time,curtimesongLenght,globalWegaSpeed;
    public GameControllerScript gc;
}
