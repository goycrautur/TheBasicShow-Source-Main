using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "windows data, really easy to understand hu-", order = 5)]
public class WindowData : ScriptableObject
{
    public int durability = 1;
    public bool uniqueCrackSound;
    [Header("particles prefab\n")]
    [Tooltip("array 0 and 1 is for broken window particl and repair prefab")] public GameObject[] particlPrefab;
    [Header("materials stuff\n")]
    [Tooltip("array num 0 and 1 are broken window mats on in and out side respectively, same for unbroken windows mat but on array 2 or 3")] public Material[] normalWinMats;
    [Header("subtitles stuff\n")]
    [Tooltip("array num 0 is broken window subtitles object, array num 1 is window repair subtitles object")]public subsScriptableObject[] subtitlesObject;
    [Header("sound stuff\n")]
    [Tooltip("array 0 and 1 respectively is for broken and repair sound of the window")] public AudioClip[] sounds;
    [Header("-----Cracked Windows Stuff-----\nparticles prefab\n")]
    [Tooltip("cracked window particle prefab")] public GameObject crackParticlPrefab;
    [Tooltip("again, but its an array")] public GameObject[] crackParticlPrefabArra;

    [Header("materials stuff\n")]
    [Tooltip("used for mostly cracked window sprite")] public Material[] cracWindowMatsSide1;
    [Tooltip("same up there but for the other window side")] public Material[] cracWindowMatsSide2;
    
    [Header("subtitles stuff\n")]
    [Tooltip("cracked window subtitle object i pray")]public subsScriptableObject CrackWindSubtitlesObject;
    [Tooltip("cracked window subtitle object but array")]public subsScriptableObject[] CrackWindSubtitlesObjectArrayified;

    [Header("sound stuff\n")]
    [Tooltip("cracked window sound audio cli")] public AudioClip CrackedWindowSounds;
    [Tooltip("cracked window sound audio cli but its an array")] public AudioClip[] CrackedWindowSoundsButItsAnArray;
}
    
