using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeLockerScript : MonoBehaviour
{
    public void OpenDoor()
    {
        if (!opened)
        {
            opened = true;
            audi.PlaySingleClip(lockSound);
            lockerAnim.enabled = true;
        }
    }

    public bool opened;

    public Animator lockerAnim;

    public AudioObjectyeah lockSound;
    public AudioManagerLiveReaction audi;
    public ShapeLocktypes ShapeLockerType;
    public enum ShapeLocktypes
    {
        EasyDifficulityFace,
        NormalDifficulityFace,
        HardDifficulityFace,
        HarderDifficulityFace,
        InsaneDifficulityFace,
        DemonDifficulityFace
    }
}
