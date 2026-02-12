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
    public void Update()
    {
        if (SpecialUpdate)
        {
            SpecialCharStuff charthingyes = CharactersReal[CurrentCharID].CustomCharExtension;
            charthingyes.OnUpdates();
        }
    }
    public void doSpecialThing()
    {
        SpecialCharStuff charthingyes = CharactersReal[CurrentCharID].CustomCharExtension;
        charthingyes.OnStarts();
        charthingyes.GiveItemOnSpawn();
        SpecialUpdate = true;
        Debug.Log("updated!!");
    }
    public void updateRefrenceThing()
    {
        if (CurrentCharID == null) CurrentCharID = 0;
        GameControllerScript.Instance.SlotsAmmount = CharactersReal[CurrentCharID].CharStatsThing.SlotsAmmount;
        AdditionalGameCustomizer.Instance.ItemSlotsSprites[0] = CharactersReal[CurrentCharID].CharStatsThing.SlotsSkin[0];
        AdditionalGameCustomizer.Instance.ItemSlotsSprites[1] = CharactersReal[CurrentCharID].CharStatsThing.SlotsSkin[1];
        AdditionalGameCustomizer.Instance.ItemSlotsSprites[2] = CharactersReal[CurrentCharID].CharStatsThing.SlotsSkin[2];
        play.DefaultWalkSpeed = CharactersReal[CurrentCharID].CharStatsThing.WalkSpeedStats;
        play.DefaultRunSpeed = CharactersReal[CurrentCharID].CharStatsThing.RunSpeedStats;
        play.DefaultstaminaDrop = CharactersReal[CurrentCharID].CharStatsThing.StaminaDrainRateStats;
        play.DefaultstaminaRise = CharactersReal[CurrentCharID].CharStatsThing.StaminaHealsRateStats;
        play.maxHealth = CharactersReal[CurrentCharID].CharStatsThing.MaxHpStats;
        play.health = CharactersReal[CurrentCharID].CharStatsThing.MaxHpStats;
        play.maxStamina = CharactersReal[CurrentCharID].CharStatsThing.MaxStamina;
        play.PlayerDmgResistance = CharactersReal[CurrentCharID].CharStatsThing.DefendMultiplierStats;
        play.LocalRange = CharactersReal[CurrentCharID].CharStatsThing.ReachDistanceStats;
        play.defaultlocalRange = CharactersReal[CurrentCharID].CharStatsThing.ReachDistanceStats;
        Singleton<OtherMainStuffManager>.Instance.slot();
        if (CharactersReal[CurrentCharID].CharStatsThing.specialTypeShit)
        {
            doSpecialThing();
        }
    }
    public int CurrentCharID;
    public PlayerScript play;
    private bool SpecialUpdate = false;
    public List<charManagStats> CharactersReal = new List<charManagStats>();
    [Serializable]
    public class charManagStats
    {
        public PlayablesStats CharStatsThing;
        public SpecialCharStuff CustomCharExtension;
    }
}
