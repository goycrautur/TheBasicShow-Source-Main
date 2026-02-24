using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagement : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static CharacterManagement Instance;
    #endregion

    public void noiseiscallingpickupphone()
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
        GameControllerScript gc = GameControllerScript.Instance;
        if (CurrentCharID == null) CurrentCharID = 0;
        Singleton<OtherMainStuffManager>.Instance.UpdateItemSizeAssignValue(true, CharactersReal[CurrentCharID].CharStatsThing.SlotsAmmount);
        ItemManager.Instance.ItemSlotsSprites[0] = CharactersReal[CurrentCharID].CharStatsThing.SlotsSkin[0];
        ItemManager.Instance.ItemSlotsSprites[1] = CharactersReal[CurrentCharID].CharStatsThing.SlotsSkin[1];
        ItemManager.Instance.ItemSlotsSprites[2] = CharactersReal[CurrentCharID].CharStatsThing.SlotsSkin[2];
        gc.player.DefaultWalkSpeed = CharactersReal[CurrentCharID].CharStatsThing.WalkSpeedStats;
        gc.player.DefaultRunSpeed = CharactersReal[CurrentCharID].CharStatsThing.RunSpeedStats;
        gc.player.DefaultstaminaDrop = CharactersReal[CurrentCharID].CharStatsThing.StaminaDrainRateStats;
        gc.player.DefaultstaminaRise = CharactersReal[CurrentCharID].CharStatsThing.StaminaHealsRateStats;
        gc.player.maxHealth = CharactersReal[CurrentCharID].CharStatsThing.MaxHpStats;
        gc.player.health = CharactersReal[CurrentCharID].CharStatsThing.MaxHpStats;
        gc.player.maxStamina = CharactersReal[CurrentCharID].CharStatsThing.MaxStamina;
        gc.player.PlayerDmgResistance = CharactersReal[CurrentCharID].CharStatsThing.DefendMultiplierStats;
        gc.player.LocalRange = CharactersReal[CurrentCharID].CharStatsThing.ReachDistanceStats;
        gc.player.defaultlocalRange = CharactersReal[CurrentCharID].CharStatsThing.ReachDistanceStats;
        Singleton<OtherMainStuffManager>.Instance.slot();
        if (CharactersReal[CurrentCharID].CharStatsThing.specialTypeShit)
        {
            doSpecialThing();
        }
    }
    public int CurrentCharID;
    private bool SpecialUpdate = false;
    public List<charManagStats> CharactersReal = new List<charManagStats>();
    [Serializable]
    public class charManagStats
    {
        public PlayablesStats CharStatsThing;
        public SpecialCharStuff CustomCharExtension;
    }
}
