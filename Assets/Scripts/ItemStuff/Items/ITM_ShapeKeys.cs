using UnityEngine;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class ITM_ShapeKeys : BaseItem
{
    [SerializeField] private bool canteat;
    [SerializeField] private float stamina, health;
    [SerializeField] private AudioObjectyeah aud_Crunch,firintheho;
    public override bool OnUse()
    {
        canteat = false;
        if (SendRay("", out RaycastHit Ray, GameControllerScript.Instance.player.LocalRange))
        {
            if (Ray.collider.tag == "HarderDifficulityFaceLock")
            {
                canteat = true;
                Ray.collider.GetComponent<ShapeLockerScript>().OpenDoor();
                return true;
            }
        }
        if (!canteat)
        {
            GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(aud_Crunch);
            GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(firintheho);
            GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, stamina);
            GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, health, 0f, true, false);
            return true;
        }
        return false;
    }
    public Keytypes KeyType;
    public enum Keytypes
    {
        EasyDifficulityFace,
        NormalDifficulityFace,
        HardDifficulityFace,
        HarderDifficulityFace,
        InsaneDifficulityFace,
        DemonDifficulityFace
    }
}
