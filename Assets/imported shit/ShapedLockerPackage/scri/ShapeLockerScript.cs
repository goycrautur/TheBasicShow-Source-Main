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
            GameControllerScript.Instance.SubsManager.summonLeSubtitle(Subtitlesthing.subtitleOption,Subtitlesthing,lockSound);
            lockSound.Play();
            lockerAnim.enabled = true;
        }
    }

    public bool opened;

    public Animator lockerAnim;

    public AudioSource lockSound;
    [SerializeField] private subsScriptableObject Subtitlesthing;
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
