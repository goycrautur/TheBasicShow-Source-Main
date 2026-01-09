using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Playables Character Stats", order = 3)]
    public class PlayablesStats : ScriptableObject // idfc if its unoptimized doing it this way lol its look clean thats all i care
    {
        public float WalkSpeedStats;
        public float RunSpeedStats;
        public float MaxStamina;
        public float StaminaDrainRateStats;
        public float StaminaHealsRateStats;
        public float MaxHpStats;
        public float DefendMultiplierStats;
        public float ReachDistanceStats;
    }
