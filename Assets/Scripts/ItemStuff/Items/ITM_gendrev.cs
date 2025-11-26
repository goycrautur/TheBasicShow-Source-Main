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
        meowloop.SetActive(true);
        mwloop.clip = peak;
        mwloop.loop = true;
        mwloop.Play();
        StartCoroutine(amwaitin(duration));
        return true;
    }
    public IEnumerator boowo(float downDuration)
    {
        float time = downDuration;
        mwloop.Stop();
        meowloop.SetActive(false);
        meow.clip = boowoo;
        meow.loop = false;
        meow.Play();
        GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(Subtitlesthing.subtitleOption,Subtitlesthing,0f,new Vector3(0f,-170.5f,0f),meow);
        GameControllerScript.Instance.player.walkSpeedMultipler -= speedMultAlt;
        GameControllerScript.Instance.player.runSpeedMultipler -= speedMultAlt;
        AdditionalGameCustomizer.Instance.FovAmmount -= AltFovAmmount;
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
        GameControllerScript.Instance.player.walkSpeedMultipler += speedMultAlt;
        GameControllerScript.Instance.player.runSpeedMultipler += speedMultAlt;
        AdditionalGameCustomizer.Instance.FovAmmount += AltFovAmmount;
        used = false;
        yield break;
    }
    public IEnumerator windBreakBlast()
    {
        GameControllerScript.Instance.player.breakwindow = true;
        GameControllerScript.Instance.player.windowbreakDistance += windobreakradius;
        yield return new WaitForSeconds(0.1f);
        GameControllerScript.Instance.player.breakwindow = false;
        GameControllerScript.Instance.player.windowbreakDistance -= windobreakradius;
        yield break;
    }
    private IEnumerator amwaitin(float time)
    {
        StartCoroutine(windBreakBlast());
        GameControllerScript.Instance.player.walkSpeedMultipler += speedMult;
        GameControllerScript.Instance.player.runSpeedMultipler += speedMult;
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
        GameControllerScript.Instance.player.walkSpeedMultipler -= speedMult;
        GameControllerScript.Instance.player.runSpeedMultipler -= speedMult;
        AdditionalGameCustomizer.Instance.rainbowTime = false;
        AdditionalGameCustomizer.Instance.FovAmmount -= FovAmmount;
        StartCoroutine(boowo(downDuration));
        yield break;
    }
    [SerializeField] private float duration = 60f,downDuration, speedMult,speedMultAlt,FovAmmount,AltFovAmmount,windobreakradius;
    [SerializeField] private GameObject meowloop;
    [SerializeField] private AudioSource meow,mwloop;
    [SerializeField] private AudioClip peak,boowoo;
    [SerializeField] private Sprite Sprite,sadbob;
    [SerializeField] private subsScriptableObject Subtitlesthing;
    [SerializeField] private bool used;
}
