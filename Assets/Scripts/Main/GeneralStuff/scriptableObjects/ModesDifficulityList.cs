using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Object Creator/Difficulity List For Modes IDFK", order = 4)]
    public class ModesDifficulityList : ScriptableObject
    {
        public int StartWithWhatDifficulity;
        public List<DifficulityImagesAndShit> difficulityeeee =new List<DifficulityImagesAndShit>();
    }
    [Serializable]
    public class DifficulityImagesAndShit
    {
        public string DifficulitiesString;
        public Sprite DifficulitySprites;
        public int DifficulityInts;
        public bool NeedRequirements;
        public Requirements whatsTheRequirements;
    }
    
