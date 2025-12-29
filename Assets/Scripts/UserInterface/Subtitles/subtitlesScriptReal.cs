using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YuriArchive.GlobalLocalization;

public class subtitlesScriptReal : MonoBehaviour
{
    private void OnDestroy()
    {
        if (is3d) GameControllerScript.Instance.SubsManager.subtitle3dList.Remove(this);
        if (!is3d) GameControllerScript.Instance.SubsManager.subtitle2dList.Remove(this);
    }
    public void Start()
    {   //some misc stuff dw
        shakespeed = subtitlys.shakeyspeed;
        shake = subtitlys.shakey;
        shakeyradius = subtitlys.shakeyradius;
        textReversing = subtitlys.textReverse;
        FuckTheText = subtitlys.unreadable;
        upsideDownReal = subtitlys.upsideDown;
        tmpTxt.text = "";
        tmpTxt.color = subtitlys.textColor;
        
        checkStuff();
    }
    private void checkStuff()
    {
        if (audiObject.isTheSubtitlesTimed)
        {
            float prevDur = 0;
            foreach (var subtitlys in audiObject.SubtitleTimingKey) // Calculate total duration
            {
                duration += subtitlys.duration;
                duration -= prevDur;
                prevDur = subtitlys.duration;
            }
            StartCoroutine(TimedSubtitle());
        }
        else
        {
            if (subtitlys.duration != 0f)
            {
                duration = subtitlys.duration; // if the subtitle scriptable object duration aint null, set this subtitle duration as the subtitle scriptable object duration
            }
            if (subtitlys.duration == 0f)
            {
                duration = audiObject.audioClip.length; // if the subtitle scriptable object duration are null tho and you just so happend to attach an audio source to the object itself, the duration will be that audioclip duration
            }
            sub = StartCoroutine(TextAnimator());
        }
    }
    public void updateSubPostion() // just make the position 3d lol, make sure to set the spatial blend to 3d/1 for it to take effect LMFAO
    {
        if (is3d)
        {
        Vector3 position = cameraTransf.position;
        Vector3 position2 = producerAud.transform.position;
        float num = Vector3.Distance(position2, position);
        float maxDistance = producerAud.maxDistance;
        float minDistance = producerAud.minDistance;
        float num2 = 1f;
        float num3 = Mathf.Atan2(position.z - position2.z, position.x - position2.x) * 57.29578f + cameraTransf.rotation.eulerAngles.y + 180f;
        float num4 = 100f * producerAud.panStereo;
        float num5 = 248.88f / aspectRatio;
        float num6 = Mathf.Lerp(num5, -num5, producerAud.spread / 360f);
        //anchoredPos.x = Mathf.Cos(num * ((float)Math.PI / 180f)) * radius;
        //anchoredPos.y = Mathf.Sin(num * ((float)Math.PI / 180f)) * radius;
        //anchoredPos.z = 0f;
        // math pi stuff from yuri that i dont think i know how to port
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
    private void Update()
    {   // subtitle will commit kys if the audio source is not found or not active in hierarchy
        if (hidesub) // are we deadass
        {
            imagbg.enabled = false;
            tmpTxt.enabled = false;
        }
        if (!hidesub)
        {
            imagbg.enabled = true;
            tmpTxt.enabled = true;
        }
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
        updateSubPostion();
        if (shake) // shake the tmp text yummy
        {
            Vector3 whyisthisnamedlocalpositionagain = new Vector3(Mathf.Sin(Time.time * shakespeed) * shakeyradius, Mathf.Cos(Time.time * shakespeed) * shakeyradius, 0f);
            tmpTxt.transform.localPosition = whyisthisnamedlocalpositionagain;
            return;
        }
        else // Reset position rea
        {
            if (tmpTxt.transform.localPosition != Vector3.zero) tmpTxt.transform.localPosition = Vector3.zero;
        }
    }

    private void LateUpdate()
    {
        if (!infinite) // dont despawn if its infinite etc
        {
            if (duration <= 0f)
            {
                Destroy(base.gameObject);
                return;
            }
            duration -= Time.deltaTime;
        }
        
        if (is3d && !hidesub) // stays invis if the audio source is muted
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
    /// Reverses a given string.
    private string ReverseString(string str)
    {
        char[] charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
    /// Replaces each character in the string with a random alphanumeric character, preserving whitespace.
    private string ReplaceWithRandomChars(string str)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder sb = new StringBuilder();
        System.Random random = new System.Random();
        foreach (char c in str)
        {
            if (char.IsWhiteSpace(c))
            {
                sb.Append(c);
            }
            else
            {
                char randomChar = chars[random.Next(chars.Length)];
                sb.Append(randomChar);
            }
        }
        return sb.ToString();
    }
    private IEnumerator TimedSubtitle()
    {
        float prevSec = 0f; // Track previous subtitle duration
        foreach (var thoseWhoSubtitles in audiObject.SubtitleTimingKey) // Iterate through each subtitle in the timed key
        {
            if (sub != null) StopCoroutine(sub);
            subtitlys = thoseWhoSubtitles;
            shakespeed = thoseWhoSubtitles.shakeyspeed;
            shake = thoseWhoSubtitles.shakey;
            shakeyradius = thoseWhoSubtitles.shakeyradius;
            textReversing = thoseWhoSubtitles.textReverse;
            FuckTheText = thoseWhoSubtitles.unreadable;
            upsideDownReal = thoseWhoSubtitles.upsideDown;
            tmpTxt.text = "";
            tmpTxt.color = thoseWhoSubtitles.textColor;
            sub = StartCoroutine(TextAnimator());
            yield return new WaitForSeconds(thoseWhoSubtitles.duration - prevSec);
            prevSec = thoseWhoSubtitles.duration; // Update previous duration for next iteration
        }
    }
    private IEnumerator TextAnimator()
    {
        StringBuilder sb = new StringBuilder(); // Use StringBuilder for efficient string concatenation
        string headTexter = !subtitlys.useLocalization ? subtitlys.headText : LocalizationManager.Instance.GetText(subtitlys.headText);
        //LocalizationManager.Instance.GetText(subtitle.headText); later trust or youre using this from the tuto
        if (textReversing)
            headTexter = ReverseString(headTexter); // Reverse text if enabled
        if (FuckTheText)
            headTexter = ReplaceWithRandomChars(headTexter); // Replace text with random characters if unreadable
        if (upsideDownReal)
            tmpTxt.transform.rotation = Quaternion.Euler(0f, 0f, 180f); // Rotate text upside down if enabled
        else
            tmpTxt.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Reset rotation if not upside down
        if (subtitlys.undertaleStyleTextAppearing) // Animate text if enabled and not in low motion mode
        {
            foreach (char c in headTexter)
            {
                sb.Append(c); // Append current character
                tmpTxt.text = sb.ToString(); // Update text with current character
                yield return new WaitForSeconds(subtitlys.TextAppearSpeed);
            }
        }
        else // Directly set text if animation is disabled
        {
            tmpTxt.text = headTexter;
        }
    }
    public Transform cameraTransf;
    public string hjea;
    public subtitlingIt subtitlys;
    public subsScriptableObject audiObject;
    public float duration,shakespeed,shakeyradius;
    public bool is3d, infinite,shake,textReversing,FuckTheText,upsideDownReal,hidesub;
    public AudioSource producerAud;
    public TMP_Text tmpTxt;
    public Image imagbg;
    public RectTransform bg;
    public Vector3 fixesPosition = Vector3.zero;
    public float aspectRatio;
    private Vector3 anchoredPos;

    private Coroutine sub;
}
