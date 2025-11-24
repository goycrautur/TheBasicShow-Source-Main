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
    }
    public void Detectrank()
    {
        for (int i = 0; i < Ranks.Length; ++i)
        {
            if (Ranks[i].rankScore <= scorevalue)
            {
                colorTexreal = ColorUtility.ToHtmlStringRGB(Ranks[i].rankColor);
                CurRank = Ranks[i].ranks;
                if (Ranks[i].soundplay && !Ranks[i].diditplay)
                {
                    RankAudioSource.PlayOneShot(Ranks[i].rankSound);
                    Ranks[i].diditplay = true;
                    Ranks[i-1].diditplay = true;
                    Ranks[i+1].diditplay = true;
                }
            }
        }
    }
    public void AddScore(int score, bool AffectedByMultipler = true)
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
    public string CurRank = "none",colorTexreal = "/color";
    public float scorevalue,curScore,realscore,PointsMultiplier = 1f;
    public GameObject scoreGmbObj,RankGmbObj;
    public TMP_Text ScoreTXT, RanksTXT;
    public AudioSource RankAudioSource;
}
