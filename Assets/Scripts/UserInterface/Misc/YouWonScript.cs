using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class YouWonScript : MonoBehaviour
{
    private void Start()
    {
        AudioSourcereal.ignoreListenerPause = true;
        Subtitles.SetActive(false);
        Singleton<TimeOutManagerFUCKYEA>.Instance.ResetTimeoutStuff();
        scoreSystemManager.Instance.stopUpdatingTSDiscord = true;
        LappingOfAsylumController.LapInstance.vanishScore = false;
        GameControllerScript.Instance.discordupdate("youwo");
        GameControllerScript.Instance.modeState = "";
    }

    private void Update()
    {
        AudioSourcereal.pitch = audPitch;
        if (!updateExecuted)
        {
            ScoreDelay -= Time.deltaTime;
            RankDelay -= Time.deltaTime;
            ResultDelay -= Time.deltaTime;
            if (youCanGoNow)
            {
                resAndQuiDelay -= Time.deltaTime;
            }
            if (secretCount)
            {
                secreDelay -= Time.deltaTime;
            }
            if (ScoreDelay <= 0f)
            {
                scorePreRoundup = Mathf.Lerp(scorePreRoundup,scoreSystemManager.Instance.scorevalue, 5f * Time.deltaTime);
                scoretext.SetActive(true);
                score = Mathf.Round(scorePreRoundup);
                ScoreTXTrea.text = "Score: " + score;
                if (!diditVineBoo)
                {
                    GameControllerScript.Instance.modeState = "Score: " + scoreSystemManager.Instance.scorevalue;
                    AudioSourcereal.clip = vinebo;
                    AudioSourcereal.loop = false;
                    audPitch = 1f;
                    AudioSourcereal.Play();
                    diditVineBoo = true;
                }
            }
            if (RankDelay <= 0f)
            {
                ranktext.SetActive(true);
                RanksTXTrea.text = "Ranks: " + scoreSystemManager.Instance.CurRank;
                if (!diditVineBoo2)
                {
                    GameControllerScript.Instance.modeState = "Score: " + scoreSystemManager.Instance.scorevalue +" | " + "Ranks: " + scoreSystemManager.Instance.CurRank;
                    AudioSourcereal.clip = vinebo;
                    AudioSourcereal.loop = false;
                    audPitch = 2f;
                    AudioSourcereal.Play();
                    diditVineBoo2 = true;
                }
            }
            if (ResultDelay <= 0f)
            {
                uwon.sprite = Yippe;
                if (secreDelay > 0f)
                {
                audPitch = 1f;
                }
                for (int i = 0; i < RanksMusicKys.Length; ++i)
                {
                    if (RanksMusicKys[i].rankScore <= scoreSystemManager.Instance.scorevalue)
                    {
                        RanksMusicKys[i].ranks = scoreSystemManager.Instance.CurRank;
                        if (!RanksMusicKys[i].diditPlay)
                        {
                            AudioSourcereal.clip = RanksMusicKys[i].rankMusic;
                            resAndQuiDelay = RanksMusicKys[i].rankMusic.length;
                            secreDelay = RanksMusicKys[i].rankMusic.length-3f;
                            AudioSourcereal.loop = false;
                            AudioSourcereal.Play();
                            RanksMusicKys[i].diditPlay = true;
                            if (EM.GetResults)
                            {
                                youCanGoNow = true;
                            }
                            if (EM.GetSecret)
                            {
                                secretCount = true;
                            }
                        }
                    }
                }  
            }
            if (resAndQuiDelay <= 0f)
            {
                cursor.UnlockCursor();
                restartObjec.SetActive(true);
                quitObjec.SetActive(true);
            }
            if (secreDelay <= 0f)
            {
                GameControllerScript.Instance.modeState = "You Won?";
                audPitch -= Time.deltaTime;
                Sdelay2 -= Time.deltaTime;
            }
            if (Sdelay2 <= 0f)
            {
                Subtitles.SetActive(true);
                
                EM.LoadSecretEnding();
                GameControllerScript.Instance.TimeoutMusic.mute = true;
            }
        }
    }

    [Header("Scene Transition Settings")]
    public GameObject Subtitles;
    public GameObject scoretext,ranktext,restartObjec,quitObjec;
    public Sprite Yippe;
    public Image uwon;
    public TMP_Text ScoreTXTrea, RanksTXTrea; 
    public float ScoreDelay,RankDelay,ResultDelay,resAndQuiDelay,secreDelay,Sdelay2=3f,scorePreRoundup,score,audPitch=1f;
	public bool updateExecuted = false,diditVineBoo,diditVineBoo2,youCanGoNow,secretCount;
    public AudioSource AudioSourcereal;
    public RanksMusi[] RanksMusicKys;
    [Serializable]
	public class RanksMusi
    {
        public string ranks;
        public int rankScore;
        public bool diditPlay;
        public AudioClip rankMusic;

	}
    public AudioClip vinebo;
    [SerializeField] private EndingManager EM;
    [SerializeField] private CursorControllerScript cursor;
}
