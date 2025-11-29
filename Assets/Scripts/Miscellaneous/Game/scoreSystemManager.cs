using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class scoreSystemManager : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static scoreSystemManager Instance;
    #endregion
    public void Update()
    {
        Detectrank();
        curScore = Mathf.Lerp(curScore,scorevalue, 3f * Time.deltaTime);
        ScoreTXT.text = "Score: <#"+colorTexreal+">"+ Mathf.Round(curScore);
        RanksTXT.text = "Ranks: <#"+colorTexreal+">"+ CurRank;
        scoreGmbObj.transform.localScale = new Vector3 (zoomscorval, zoomscorval, zoomscorval);
        RankGmbObj.transform.localScale = new Vector3 (zoomRankval, zoomRankval, zoomRankval);
        if (!stopzoombindown)
        {
            zoomscorval -= Time.deltaTime; 
        }
        if (zoomscorval < 1f)
		{
            zoomscorval = 1f;
            stopzoombindown = true;
		}
        if (!stopzoomRank)
        {
            zoomRankval -= Time.deltaTime; 
        }
        if (zoomRankval < 1f)
		{
            zoomRankval = 1f;
            stopzoomRank = true;
		}
    }
    public void Detectrank()
    {
        for (int i = 0; i < Ranks.Length; ++i)
        {
            if (Ranks[i].rankScore <= Mathf.Round(curScore))
            {
                colorTexreal = ColorUtility.ToHtmlStringRGB(Ranks[i].rankColor);
                CurRank = Ranks[i].ranks;
                if (Ranks[i].soundplay && !Ranks[i].diditplay)
                {
                    RankAudioSource.PlayOneShot(Ranks[i].rankSound);
                    Ranks[i].diditplay = true;
                    zoomRankval = 1.2f;
                    stopzoomRank = false;
                }
            }
        }
        if (!stopUpdatingTSDiscord)
        {
            GameControllerScript.Instance.discordupdate(discordUpdateType);
        }
        
    }
    public void AddScore(int score,bool AffectedByMultipler = true,bool UpdateDiscord = false, string UpdateType = "chees")
    {
        if (!AffectedByMultipler)
        {
        realscore = score;
        }
        if (AffectedByMultipler)
        {
        realscore = score*PointsMultiplier;
        }
        scorevalue += realscore;
        if (UpdateDiscord)
        {
            discordUpdateType = UpdateType;
        }
        zoomscorval = 1.2f;
        stopzoombindown = false;
    }
    public RanksShit[] Ranks;
    [Serializable]
	public class RanksShit
    {
        public string ranks;
        public Color rankColor;
        public int rankScore;
        public bool usesTexture;
		public Sprite rankTexture;
        public bool soundplay,rankZoomed,diditplay;
        public AudioClip rankSound;
	}
    public bool stopUpdatingTSDiscord,stopzoombindown,stopzoomRank;
    public string CurRank = "none",colorTexreal = "/color",discordUpdateType;
    public float scorevalue,curScore,realscore,PointsMultiplier = 1f,zoomscorval,zoomRankval;
    public GameObject scoreGmbObj,RankGmbObj;
    public TMP_Text ScoreTXT, RanksTXT;
    public AudioSource RankAudioSource;
}
