using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_gendrev : BaseItem
{
    public override bool OnUse()
    {
        if (used) return false;
        AdditionalGameCustomizer.Instance.FovAmmount += FovAmmount;
        AdditionalGameCustomizer.Instance.rainbowTime = true;
        used = true;
        GenderRevealSource1.ClearQueue(true);
        GenderRevealSource1.SetLoop(true);
        GenderRevealSource1.QueueAudio(peak);
        StartCoroutine(amwaitin(duration));
        return true;
    }
    public IEnumerator boowo(float downDuration)
    {
        float time = downDuration;
        GenderRevealSource1.ClearQueue(true);
        GenderRevealSource2.PlaySingleClip(boowoo);
        lowBudgetAudioManagementShit.Instance.MainSource1.PlaySingleClip(lowBudgetAudioManagementShit.Instance.deltaruneExplud);
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, howManyHpToRemove, 0f, true,false);
        GameControllerScript.Instance.player.pModManag.movementModifiers.Add(SlowSpeedMod);
        AdditionalGameCustomizer.Instance.FovAmmount -= AltFovAmmount;
        foreach (NPC ennPeeCee in FindObjectsOfType<NPC>())
		{
			if (ennPeeCee != null)
			{
				if (Vector3.Distance(GameControllerScript.Instance.player.transform.position, ennPeeCee.transform.position) <= windobreakradius/1.5f)
				{
                    ennPeeCee.DrainHp(50);
				}
			}
		}
        Gauge newGauge3 = GaugeManager.Instance.CreateGaugeInstance(sadbob, downDuration);
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if (newGauge3 != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge3.Set(downDuration, time);
            }
            yield return null;
        }
        newGauge3.Hide();
        GameControllerScript.Instance.player.pModManag.movementModifiers.Remove(SlowSpeedMod);
        AdditionalGameCustomizer.Instance.FovAmmount += AltFovAmmount;
        used = false;
        yield break;
    }
    private IEnumerator amwaitin(float time)
    {
        GameControllerScript.Instance.player.breakwind(true,windobreakradius+20f);
        GameControllerScript.Instance.player.pModManag.movementModifiers.Add(SpeedModifier);
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(Sprite, duration);
        time = duration;
        yield return null;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(duration, time);
            }
            yield return null;
        }
        newGauge.Hide();
        GameControllerScript.Instance.player.pModManag.movementModifiers.Remove(SpeedModifier);
        AdditionalGameCustomizer.Instance.rainbowTime = false;
        AdditionalGameCustomizer.Instance.FovAmmount -= FovAmmount;
        StartCoroutine(boowo(downDuration));
        yield break;
    }
    [SerializeField] private float duration = 60f,downDuration,howManyHpToRemove,FovAmmount,AltFovAmmount,windobreakradius;
    [SerializeField] private AudioManagerLiveReaction GenderRevealSource1,GenderRevealSource2;
    [SerializeField] private MovementModifier SpeedModifier = new MovementModifier(default(Vector3), 0f),SlowSpeedMod = new MovementModifier(default(Vector3), 0f);
    [SerializeField] private AudioObjectyeah peak,boowoo;
    [SerializeField] private Sprite Sprite,sadbob;
    [SerializeField] private bool used;
}
