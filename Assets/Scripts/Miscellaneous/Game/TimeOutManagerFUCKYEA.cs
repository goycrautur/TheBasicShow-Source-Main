using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOutManagerFUCKYEA : Singleton<TimeOutManagerFUCKYEA>
{
    public bool countItDown,ohboy,spamupdatethese;
    public float TimeDuratiOk = 9999f,timeoutTVDurationStuffIncaseReachingFinaleLmfao;
    public void Awake()
    {
        TimeDuratiOk = 9999f;
    }
    public void Update()
    {

        timeoutTVDurationStuffIncaseReachingFinaleLmfao -= Time.deltaTime; 
        if (TimeDuratiOk < 0f && countItDown)
		{
            ohboy = true;
            countItDown = false;
		}
        if (countItDown)TimeDuratiOk -= Time.deltaTime; 
        if (ohboy)
        {
            itsfuckingtimeout();
            ohboy = false;
        }
    }
    public void itsfuckingtimeout()
    {
        if (spamupdatethese)
        {
        Singleton<OtherMainStuffManager>.Instance.AngerShit(GameControllerScript.Instance.mode == "story" ? 0.0025f*LearningGameManager.Instance.angerMult : 0.0001f*LearningGameManager.Instance.angerMult, 0f,false, "all");
        GameControllerScript.Instance.player.ResetGuilt("running", 1f);
        }
        if (!GameControllerScript.Instance.timeout)
        {
            timeoutTVDurationStuffIncaseReachingFinaleLmfao = LearningGameManager.Instance.aud_Timeout.audClip.length + (LearningGameManager.Instance.Television.Markings ? 3.1f : 0.85f);
            LearningGameManager.Instance.Television.baldingit = true;
            StartCoroutine(LearningGameManager.Instance.Television.StartTVSequence(LearningGameManager.Instance.aud_Timeout));
            if (GameControllerScript.Instance.mode == "story")
            {
            StartCoroutine(GameControllerScript.Instance.tiemoutStu());
            }
            if (GameControllerScript.Instance.mode != "story")
            {
            StartCoroutine(GameControllerScript.Instance.easingExit(new Color(0.45f, 0.45f, 0.45f, 1f), 0, 2, 5));
            }
            GameControllerScript.Instance.timeout = true;
            spamupdatethese =true;
            return;
        }
    }
    public void InitializeTimeoutStuff(float durati)
    {
        TimeDuratiOk = durati;
        countItDown = true;
    }
    public void ResetTimeoutStuff()
    {
        GameControllerScript.Instance.timeout = false;
        countItDown = false;
        TimeDuratiOk = 9999;
    }
}
