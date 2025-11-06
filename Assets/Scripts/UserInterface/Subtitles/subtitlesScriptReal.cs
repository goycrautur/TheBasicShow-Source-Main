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
            duration = realduration;
        }
        tmpTxt.text = "";
        StartCoroutine(TextAnimator());
        tmpTxt.color = subtitlys.textColor;
        aspectRatio = 1f;
        bg.localScale = new Vector3(1f, 1f, 1f);
        if (is3d)
        {
            bg.anchoredPosition = new Vector3(0f, -266.66f / aspectRatio, 0f);
            return;
        }
        bg.anchoredPosition = fixesPosition;
    }

    private void Update()
    {
        if (subtitlys.shakey)
        {
            Vector3 localPosition = new Vector3(Mathf.Sin(Time.time * subtitlys.shakeyspeed) * subtitlys.shakeyradius, Mathf.Cos(Time.time * subtitlys.shakeyspeed) * subtitlys.shakeyradius, 0f);
            tmpTxt.transform.localPosition = localPosition;
            return;
        }
        else if (!subtitlys.utStyle)
        {
            if (tmpTxt.text.Length >= 50)
            {
                tmpTxt.fontSize = 16;
            }
            if (tmpTxt.text.Length >= 98)
            {
                tmpTxt.fontSize = 8;
            }
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
        try
        {
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
        }
        catch
        {
            Destroy(base.gameObject);
        }
    }
    private IEnumerator TextAnimator()
    {
        foreach (char c in subtitlys.headText)
        {
            TMP_Text tmp_Text = tmpTxt;
            tmp_Text.text += c.ToString();
            if (subtitlys.utStyle)
            {
                if (tmp_Text.text.Length >= 50)
                {
                    tmpTxt.fontSize = 16;
                }
                if (tmp_Text.text.Length >= 98)
                {
                    tmpTxt.fontSize = 8;
                }
                yield return new WaitForSeconds(subtitlys.TextAppearSpeed);
            }
        }
        yield break;
    }
    public void OnEnable()
    {
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
    public subtitlingIt subtitlys;
    public subsScriptableObject audiObject;
    public float duration,realduration;
    public bool is3d, infinite;
    public AudioSource producerAud;
    public TMP_Text tmpTxt;
    public Image imagbg;
    public RectTransform bg;
    public Vector3 fixesPosition = Vector3.zero;
    private float aspectRatio;
}
