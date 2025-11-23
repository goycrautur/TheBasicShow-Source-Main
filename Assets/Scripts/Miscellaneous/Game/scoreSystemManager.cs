using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class scoreSystemManager : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static scoreSystemManager Instance;
    #endregion
    public void Update()
    {
        Detectrank();
        curScore = Mathf.Lerp(curScore,scorevalue, 5f * Time.deltaTime);
        ScoreTXT.text = "Score: "+ Mathf.Round(curScore);
        RanksTXT.text = "Ranks: "+ CurRank;
    }
    public void Detectrank()
    {
        for (int i = 0; i < Ranks.Length; ++i)
        {
            if (Ranks[i].rankScore <= scorevalue)
            {
                CurRank = Ranks[i].ranks;
            }
        }
    }
    public void AddScore(int score)
    {
        scorevalue += score;
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
	}
    public string CurRank = "none";
    public float scorevalue,curScore;
    public TMP_Text ScoreTXT, RanksTXT; 
}
