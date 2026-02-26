using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OtherMainStuffManager : Singleton<OtherMainStuffManager>
{
    //peak ass singleton class for just chaos mode and other tuff stuff ky
    public void Start()
    {
        MuchoStunDura = 1f;
        FamStunDura = 1f;
        ZerStunDura = 1f;
        BalStunDura = 1f;
    }
    public void Update()
    {
        if (MuchoStunCount)
		{
			MuchoStunDura -= Time.deltaTime;
            foreach (MuchoScript muc in GameControllerScript.Instance.muchscr)
            {
                if (muc.isActiveAndEnabled)
                {
                    muc.stopMoving = true;
                    muc.agent.speed = 0;
                    muc.stopOverridingStun = true;
                    muc.IsHitboxValid = false;
                }
            }
		}
        if (MuchoStunDura < 0f)
		{
            foreach (MuchoScript muc in GameControllerScript.Instance.muchscr)
            {
                if (muc.isActiveAndEnabled)
                {
                    muc.stopMoving = false;
                    muc.stopOverridingStun = false;
                    muc.IsHitboxValid = true;
                    muc.resetWaitTime();
                    muc.Move();
                }
            }
            MuchoStunCount = false;
            MuchoStunDura = 1f;
        }
        if (FamStunCount)
		{
			FamStunDura -= Time.deltaTime;
		}
        if (FamStunDura < 0f)
		{
            FamStunCount = false;
            FamStunDura = 1f;
        }
        if (ZerStunCount)
		{
			ZerStunDura -= Time.deltaTime;
            foreach (zerullscript zes in GameControllerScript.Instance.zerscr)
            {
                if (zes.isActiveAndEnabled)
                {
                    zes.stopMoving = true;
                    zes.agent.speed = 0;
                    zes.stopOverridingStun = true;
                    zes.IsHitboxValid = false;
                }
            }
		}
        if (ZerStunDura < 0f)
		{
            foreach (zerullscript zes in GameControllerScript.Instance.zerscr)
            {
                if (zes.isActiveAndEnabled)
                {
                    zes.stopMoving = false;
                    zes.stopOverridingStun = false;
                    zes.IsHitboxValid = true;
                    zes.resetWaitTime();
                    zes.Move();
                }
            }
            ZerStunCount = false;
            ZerStunDura = 1f;
            
        }
        if (BalStunCount)
		{
			BalStunDura -= Time.deltaTime;
            foreach (BaldiScript bal in GameControllerScript.Instance.balscr)
            {
                if (bal.isActiveAndEnabled)
                {
                    bal.stopMoving = true;
                    bal.agent.speed = 0;
                    bal.stopOverridingStun = true;
                    bal.IsHitboxValid = false;
                }
            }
		}
        if (BalStunDura < 0f)
		{
            foreach (BaldiScript bal in GameControllerScript.Instance.balscr)
            {
                if (bal.isActiveAndEnabled)
                {
                    bal.stopMoving = false;
                    bal.stopOverridingStun = false;
                    bal.IsHitboxValid = true;
                    bal.resetWaitTime();
                    bal.Move();
                }
            }
            BalStunCount = false;
            BalStunDura = 1f;
        }
    }
    #region TeachersChaosModeStuff
    public void HearingShit(float soundval, Transform wherItCameFrom, Vector3 wherItCameFromAlt, string teacher = null, bool UseVector3 = false)
    {
        if (teacher == "all" || teacher == "All" || teacher == "zerull")
        {
            foreach (zerullscript zes in GameControllerScript.Instance.zerscr)
            {
                if (zes.isActiveAndEnabled)
                {
                    zes.Hear(!UseVector3 ? wherItCameFrom.position : wherItCameFromAlt, soundval);
                }
            }
        }
        if (teacher == "all" || teacher == "All" || teacher == "famished")
        {
            foreach (FamishedScript fam in  GameControllerScript.Instance.famishscr)
            {
                if (fam.isActiveAndEnabled)
                {
                    fam.Hear(!UseVector3 ? wherItCameFrom.position : wherItCameFromAlt, soundval);
                }
            }
        }
        if (teacher == "all" || teacher == "All" || teacher == "mucho")
        {
            foreach (MuchoScript muc in GameControllerScript.Instance.muchscr)
            {
                if (muc.isActiveAndEnabled)
                {
                    muc.Hear(!UseVector3 ? wherItCameFrom.position : wherItCameFromAlt, soundval);
                }
            }
        }
        if (teacher == "all" || teacher == "All" || teacher == "baldi")
        {
            foreach (BaldiScript bal in GameControllerScript.Instance.balscr)
            {
                if (bal.isActiveAndEnabled)
                {
                    bal.Hear(!UseVector3 ? wherItCameFrom.position : wherItCameFromAlt, soundval);
                }
            }
        }
    }
    public void deafshit(float AntiHearingDuration,string teacher = null)
    {
        if (teacher == "all" || teacher == "All" || teacher == "zerull")
        {
            foreach (zerullscript zes in GameControllerScript.Instance.zerscr)
            {
                if (zes.isActiveAndEnabled)
                {
                    zes.ActivateAntiHearing(AntiHearingDuration);
                }
            }
        }
        if (teacher == "all" || teacher == "All" || teacher == "famished")
        {
            foreach (FamishedScript fam in GameControllerScript.Instance.famishscr)
            {
                if (fam.isActiveAndEnabled)
                {
                    fam.ActivateAntiHearing(AntiHearingDuration);
                }
            }
        }
        if (teacher == "all" || teacher == "All" || teacher == "mucho")
        {
            foreach (MuchoScript muc in GameControllerScript.Instance.muchscr)
            {
                if (muc.isActiveAndEnabled)
                {
                    muc.ActivateAntiHearing(AntiHearingDuration);
                }
            }
        }
        if (teacher == "all" || teacher == "All" || teacher == "baldi")
        {
            foreach (BaldiScript bal in GameControllerScript.Instance.balscr)
            {
                if (bal.isActiveAndEnabled)
                {
                    bal.ActivateAntiHearing(AntiHearingDuration);
                }
            }
        }
    }
    public void PeakStun(float Duration,string teacher = null)
    {
        if (teacher == "all" || teacher == "All" || teacher == "zerull")
        {
            ZerStunDura = Duration;
            ZerStunCount = true;
        }
        if (teacher == "all" || teacher == "All" || teacher == "famished")
        {
            FamStunDura = Duration;
            FamStunCount = true;
        }
        if (teacher == "all" || teacher == "All" || teacher == "mucho")
        {
            MuchoStunDura = Duration;
            MuchoStunCount = true;
        }
        if (teacher == "all" || teacher == "All" || teacher == "baldi")
        {
            BalStunDura = Duration;
            BalStunCount = true;
        }
    }
    public void AngerShit(float angerAmmount, float tempAngerAmmount = 0f, bool tempAnger = false, string teacher = null)
    {
        if (teacher == "all" || teacher == "All" || teacher == "zerull")
        {
            foreach (zerullscript zes in GameControllerScript.Instance.zerscr)
            {
                if (zes.isActiveAndEnabled)
                {
                    if (!tempAnger)
                    {
                        zes.GetAngry(angerAmmount);
                    }
                    else
                    {
                        zes.GetTempAngry(tempAngerAmmount);
                    }
                }
            }
        }
        if (teacher == "all" || teacher == "All" || teacher == "famished")
        {
            foreach (FamishedScript fam in GameControllerScript.Instance.famishscr)
            {
                if (fam.isActiveAndEnabled)
                {
                    if (!tempAnger)
                    {
                        fam.GetAngry(angerAmmount);
                    }
                    else
                    {
                        fam.GetTempAngry(tempAngerAmmount);
                    }
                }
            }
        }
        if (teacher == "all" || teacher == "All" || teacher == "mucho")
        {
            foreach (MuchoScript muc in GameControllerScript.Instance.muchscr)
            {
                if (muc.isActiveAndEnabled)
                {
                    if (!tempAnger)
                    {
                        muc.GetAngry(angerAmmount);
                    }
                    else
                    {
                        muc.GetTempAngry(tempAngerAmmount);
                    }
                }
            }
        }
        if (teacher == "all" || teacher == "All" || teacher == "baldi")
        {
            foreach (BaldiScript bal in GameControllerScript.Instance.balscr)
            {
                if (bal.isActiveAndEnabled)
                {
                    if (!tempAnger)
                    {
                        bal.GetAngry(angerAmmount);
                    }
                    else
                    {
                        bal.GetTempAngry(tempAngerAmmount);
                    }
                }
            }
        }
    }
    public void ChangeItemSlot(int SlotAmmount = 1,bool DropAllAvailableItemInInventory = false)
    {
        if (DropAllAvailableItemInInventory) HighSchoolDropOut();
        UpdateInventoryLength(true,SlotAmmount);
        slot();
    }
    public void HighSchoolDropOut()
    {
        for (int i = 0; i < GameControllerScript.Instance.SlotsAmmount; ++i)
        {
            if (ItemManager.Instance.Inventory[i].ItemInstance != null)
            {
                ItemManager.Instance.DropItem(i);
            }
        }
    }
    public void UpdateInventoryLength(bool setSlotNum = false, int SlotNumber = 1)
    {
        if (setSlotNum) GameControllerScript.Instance.SlotsAmmount = SlotNumber;
        if (GameControllerScript.Instance.SlotsAmmount == ItemManager.Instance.Inventory.Length) 
        {
            SlotsAmmou = ItemManager.Instance.Inventory.Length;
            MaxSlotsAmmou = ItemManager.Instance.Inventory.Length;
        }
        else SlotsAmmou = GameControllerScript.Instance.SlotsAmmount;
        GameControllerScript.Instance.SlotsAmmount = SlotsAmmou;
        
    }
    public void ResizeAltInventory() // bru
    {
        Array.Resize(ref AltInventory, MaxSlotsAmmou);
    }
    public void UpdateAltInventory()
    {
        for (int i = 0; i < AltInventory.Length; ++i)
        {
            AltInventory[i].ItemID = 0;
            AltInventory[i].ItemInstance = null;
        }
        for (int i = 0; i < ItemManager.Instance.Inventory.Length; ++i)
        {
            AltInventory[i].ItemID = ItemManager.Instance.Inventory[i].ItemID;
            AltInventory[i].ItemInstance = ItemManager.Instance.Inventory[i].ItemInstance;
            AltInventory[i].ItemImages = ItemManager.Instance.Inventory[i].ItemImages;
            AltInventory[i].ItemImageBGs = ItemManager.Instance.Inventory[i].ItemImageBGs;
            AltInventory[i].ItemImageSlots = ItemManager.Instance.Inventory[i].ItemImageSlots;
            AltInventory[i].SlotID = ItemManager.Instance.Inventory[i].SlotID;
        }
    }
    public void slot()
    {
        Debug.Log("slotted");
        for (int i = 0; i < AltInventory.Length; ++i) if (i != ItemManager.Instance.ItemSelection) AltInventory[i].ItemImages.texture = null;
        for (int i = 0; i < ItemManager.Instance.Inventory.Length; ++i)
        {
            if (i != ItemManager.Instance.ItemSelection)
            {
                ItemManager.Instance.Inventory[i].ItemID = 0;
                ItemManager.Instance.Inventory[i].ItemInstance = null;
            }
        }
        if (ItemManager.Instance.ItemSelection >= SlotsAmmou) ItemManager.Instance.ItemSelection = SlotsAmmou - 1;
        if (SlotsAmmou == 0)
        {
            AltInventory[0].ItemImages.enabled = false;
            AltInventory[0].ItemImageBGs.enabled = false;
            AltInventory[0].ItemImageSlots.enabled = false;
            Debug.Log("mf have 0 slots left holy skull");
            return;
        }
        for (int i = SlotsAmmou-1; i < MaxSlotsAmmou; ++i)
        {
            AltInventory[i].ItemImages.enabled = false;
            AltInventory[i].ItemImageBGs.enabled = false;
            AltInventory[i].ItemImageSlots.enabled = false;
        }
        for (int i = 0; i < SlotsAmmou; ++i)
        {
            AltInventory[i].ItemImages.enabled = true;
            AltInventory[i].ItemImageBGs.enabled = true;
            AltInventory[i].ItemImageSlots.enabled = true;
        }
        
        
        Array.Resize(ref ItemManager.Instance.Inventory, SlotsAmmou);
        Debug.Log($"resized array to {SlotsAmmou} slots");
        for (int i = 0; i < ItemManager.Instance.Inventory.Length; ++i)
        {
            ItemManager.Instance.Inventory[i].ItemID = AltInventory[i].ItemID;
            ItemManager.Instance.Inventory[i].ItemInstance = AltInventory[i].ItemInstance;
            ItemManager.Instance.Inventory[i].ItemImages = AltInventory[i].ItemImages;
            ItemManager.Instance.Inventory[i].ItemImageBGs = AltInventory[i].ItemImageBGs;
            ItemManager.Instance.Inventory[i].ItemImageSlots = AltInventory[i].ItemImageSlots;
            ItemManager.Instance.Inventory[i].SlotID = AltInventory[i].SlotID;
        }
        ItemManager.Instance.Inventory[SlotsAmmou-1].ItemImageSlots.sprite = ItemManager.Instance.ItemSlotsSprites[2];
        for (int i = 1; i < SlotsAmmou-1; ++i) ItemManager.Instance.Inventory[i].ItemImageSlots.sprite = ItemManager.Instance.ItemSlotsSprites[1];
        ItemManager.Instance.Inventory[0].ItemImageSlots.sprite = ItemManager.Instance.ItemSlotsSprites[GameControllerScript.Instance.SlotsAmmount != 1 ? 0 : 1];
        ItemManager.Instance.UpdateItemUI();
    }
    #endregion
    public float MuchoStunDura,FamStunDura,ZerStunDura,BalStunDura;
    public HeldItem[] AltInventory;
    public bool MuchoStunCount,FamStunCount,ZerStunCount,BalStunCount;
    public int SlotsAmmou,MaxSlotsAmmou;
}
