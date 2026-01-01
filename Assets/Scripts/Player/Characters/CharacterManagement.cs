using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagement : MonoBehaviour
{
    private void Start()
    {
        CurrentCharID = PlayerPrefs.GetInt("CharInt", 0);
        updateRefrenceThing();
    }
    public void doSpecialThing()
    {
        SpecialCharStuff charthingyes = CharactersReal[CurrentCharID].CustomCharExtension;
        charthingyes.GiveItemOnSpawn();
    }
    public void updateRefrenceThing()
    {
        if (CurrentCharID == null) CurrentCharID = 0;
        GameControllerScript.Instance.SlotsAmmount = CharactersReal[CurrentCharID].SlotsAmmount;
        AdditionalGameCustomizer.Instance.ItemSlotsSprites[0] = CharactersReal[CurrentCharID].SlotsSkin[0];
        AdditionalGameCustomizer.Instance.ItemSlotsSprites[1] = CharactersReal[CurrentCharID].SlotsSkin[1];
        AdditionalGameCustomizer.Instance.ItemSlotsSprites[2] = CharactersReal[CurrentCharID].SlotsSkin[2];
        play.DefaultWalkSpeed = CharactersReal[CurrentCharID].WalkSpeedStats;
        play.DefaultRunSpeed = CharactersReal[CurrentCharID].RunSpeedStats;
        play.DefaultstaminaDrop = CharactersReal[CurrentCharID].StaminaDrainRateStats;
        play.DefaultstaminaRise = CharactersReal[CurrentCharID].StaminaHealsRateStats;
        play.maxHealth = CharactersReal[CurrentCharID].MaxHpStats;
        play.health = CharactersReal[CurrentCharID].MaxHpStats;
        play.maxStamina = CharactersReal[CurrentCharID].MaxStamina;
        play.PlayerDmgResistance = CharactersReal[CurrentCharID].DefendMultiplierStats;
        Singleton<OtherMainStuffManager>.Instance.slot();
        if (CharactersReal[CurrentCharID].HasAnySpecialShit)
        {
            doSpecialThing();
        }
    }
    public int CurrentCharID;
    public PlayerScript play;
    public List<Character> CharactersReal = new List<Character>();
    [Serializable]
	public class Character
    {
        public float WalkSpeedStats,RunSpeedStats,MaxStamina,StaminaDrainRateStats,StaminaHealsRateStats,MaxHpStats,DefendMultiplierStats;
        public int SlotsAmmount;
        public Sprite[] SlotsSkin;
        public bool HasAnySpecialShit;
        public SpecialCharStuff CustomCharExtension;
	}
}
