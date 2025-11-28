using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class subtitlesScriptReal : MonoBehaviour
{
    // yea forgive me im too braindead to do scaling lol dont copystrike me yuri
    private void Start()
    {
        shakespeed = subtitlys.shakeyspeed;
        shake = subtitlys.shakey;
        shakeyradius = subtitlys.shakeyradius;
        tmpTxt.text = "";
        hjea = subtitlys.headText;
        tmpTxt.color = subtitlys.textColor;
        aspectRatio = 1f;
        bg.localScale = new Vector3(1f, 1f, 1f);
        bg.anchoredPosition = fixesPosition;
        
        checkStuff();
    }
    private void checkStuff()
    {
        if (subtitlys.duration != 0f && realduration == 0f)
        {
            duration = subtitlys.duration;
        }
        else if (realduration == 0f)
        {
            duration = audiObject.audioClip.length;
        }
        else
        {
            if (!subtitlys.timedSub)
            {
                duration = realduration;
                
            }
        }
        if (subtitlys.utStyle)
        {
            StartCoroutine(TextAnimator(hjea,subtitlys.TextAppearSpeed));
        }
        if (!subtitlys.utStyle)
        {
            tmpTxt.text = subtitlys.headText;
        }
        if (subtitlys.timedSub)
        {
            for (int i = 0; i < subtitlys.subtitleTimings.Length; ++i)
            {
                StartCoroutine(TimingIthink(subtitlys.subtitleTimings[i].duration,i));
            }
        }

        if (is3d)
        {
            bg.anchoredPosition = new Vector3(0f, -266.66f / aspectRatio, 0f);
            return;
        }
    }
    private void Update()
    {
        if (shake)
        {
            Vector3 localPosition = new Vector3(Mathf.Sin(Time.time * shakespeed) * shakeyradius, Mathf.Cos(Time.time * shakespeed) * shakeyradius, 0f);
            tmpTxt.transform.localPosition = localPosition;
            return;
        }
        tmpTxt.transform.localPosition = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (producerAud == null)
        {
            Destroy(base.gameObject);
            return;
        }
        if (!producerAud.gameObject.activeInHierarchy)
        {
            Destroy(base.gameObject);
            return;
        }
        if (!infinite)
        {
            if (duration <= 0f)
            {
                Destroy(base.gameObject);
                return;
            }
            duration -= Time.deltaTime;
        }
        if (!is3d)
        {
            return;
        }
            Vector3 position = Camera.main.transform.position;
            Vector3 position2 = producerAud.transform.position;
            float num = Vector3.Distance(position2, position);
            float maxDistance = producerAud.maxDistance;
            float minDistance = producerAud.minDistance;
            float num2 = 1f;
            if ((num > maxDistance && num2 > 0.5f) || producerAud.volume == 0f)
            {
                bg.localScale = new Vector3(0f, 0f, 1f);
            }
            else
            {
                float num3 = Mathf.Atan2(position.z - position2.z, position.x - position2.x) * 57.29578f + Camera.main.transform.rotation.eulerAngles.y + 180f;
                float num4 = 100f * producerAud.panStereo;
                float num5 = 248.88f / aspectRatio;
                float num6 = Mathf.Lerp(num5, -num5, producerAud.spread / 360f);
                bg.anchoredPosition = new Vector3(Mathf.Cos(num3 * 0.017453292f) * num6 * num2 + num4, Mathf.Lerp(-266.66f / aspectRatio, Mathf.Sin(num3 * 0.017453292f) * num6, num2), 0f);
                float num7 = 1f;
                switch (producerAud.rolloffMode)
                {
                    case AudioRolloffMode.Logarithmic:
                        {
                            float num8 = 1f;
                            num7 = minDistance * (1f / (1f + num8 * (num - 1f)));
                            break;
                        }
                    case AudioRolloffMode.Linear:
                        num7 = Mathf.Lerp(1f, 0f, num / maxDistance - minDistance / maxDistance);
                        break;
                    case AudioRolloffMode.Custom:
                        num7 = producerAud.GetCustomCurve(AudioSourceCurveType.CustomRolloff).Evaluate(num / maxDistance);
                        break;
                }
                num7 = Mathf.Clamp01(1f * num7);
                float num9 = Mathf.Lerp(1f, num7, num2);
                num9 *= 1f;
                bg.localScale = new Vector3(num9, num9, 1f);
            }
        if (is3d)
        {
            if (producerAud != null)
            {
                if (producerAud.mute)
                {
                    imagbg.enabled = false;
                    tmpTxt.enabled = false;
                }
                if (!producerAud.mute)
                {
                    imagbg.enabled = true;
                    tmpTxt.enabled = true;
                }
            }
        }
    }
    private IEnumerator TimingIthink(float time,int i)
    {

        yield return new WaitForSeconds(time);
        tmpTxt.text = "";
        tmpTxt.color = subtitlys.subtitleTimings[i].textColor;
        shake = subtitlys.subtitleTimings[i].shakey;
        shakespeed = subtitlys.subtitleTimings[i].shakeyspeed;
        shakeyradius = subtitlys.subtitleTimings[i].shakeyradius;
        hjea = subtitlys.subtitleTimings[i].headText;
        if (subtitlys.subtitleTimings[i].utStyle)
        {
            StartCoroutine(TextAnimator(hjea,subtitlys.subtitleTimings[i].TextAppearSpeed));
        }
        if (!subtitlys.subtitleTimings[i].utStyle)
        {
            tmpTxt.text = hjea;
        }
        Debug.Log("timing sub " + i);
        yield break;
    }
    private IEnumerator TextAnimator(string Text,float textAppearSpeed)
    {
        foreach (char c in Text)
        {
            tmpTxt.text += c.ToString();
            yield return new WaitForSeconds(textAppearSpeed);
        }
        yield break;
    }
    public string hjea;
    public subtitlingIt subtitlys;
    public subsScriptableObject audiObject;
    public float duration,realduration,shakespeed,shakeyradius;
    public bool is3d, infinite,shake;
    public AudioSource producerAud;
    public TMP_Text tmpTxt;
    public Image imagbg;
    public RectTransform bg;
    public Vector3 fixesPosition = Vector3.zero;
    private float aspectRatio;
}
