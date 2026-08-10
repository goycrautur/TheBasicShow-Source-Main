using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_mmmmcranberry : BaseItem
{
    private float duration = 60f;
    [SerializeField] private float energy,SpeedModDefault;
    [SerializeField] private AudioObjectyeah MinecraftDrink,leDrink;
    [SerializeField] private Sprite IconImage;
    [SerializeField] private MovementModifier SpeedModifier = new MovementModifier(default(Vector3), 0f);
    [SerializeField] private bool used;

    public override bool OnUse()
    {
        if (used) return false;
        used = true;
        StartCoroutine(Wait());
        return true;
    }

    private IEnumerator Wait()
    {
        float timeone = leDrink.audClip.length;
        float timetwo = MinecraftDrink.audClip.length;
        float WhateverBro = 0f;
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(MinecraftDrink);
        while (timetwo > 0f)
        {
            timetwo -= Time.deltaTime;
            yield return null;
        }
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, energy);
        GameControllerScript.Instance.player.pModManag.movementModifiers.Add(SpeedModifier);
        GameControllerScript.Instance.lbams.CranberrySodaSource.PlaySingleClip(leDrink);
        AdditionalGameCustomizer.Instance.FovAmmount += 15;
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(IconImage, duration);
        while (timeone > 0f)
        {
            timeone -= Time.deltaTime;
            SpeedModifier.movementMultiplier = 1f + SpeedModDefault;
            if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(leDrink.audClip.length, timeone);
            }
            yield return null;
        }
        newGauge.Hide();
        GameControllerScript.Instance.player.pModManag.movementModifiers.Remove(SpeedModifier);
        AdditionalGameCustomizer.Instance.FovAmmount -= 15;
        used = false;
        yield break;
    }
}
