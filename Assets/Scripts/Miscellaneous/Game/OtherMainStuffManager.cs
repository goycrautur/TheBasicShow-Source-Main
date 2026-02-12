using System.Collections;
using System.Collections.Generic;
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
    public void slot()
    {
        if (ItemManager.Instance.ItemSelection >= GameControllerScript.Instance.SlotsAmmount)
        {
            ItemManager.Instance.ItemSelection = GameControllerScript.Instance.SlotsAmmount - 1;
        }
        AdditionalGameCustomizer.Instance.ItemImageSlots[GameControllerScript.Instance.SlotsAmmount-1].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[2];
        for (int i = 1; i < GameControllerScript.Instance.SlotsAmmount-1; ++i)
        {
            AdditionalGameCustomizer.Instance.ItemImageSlots[i].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[1];
        }
        AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[GameControllerScript.Instance.SlotsAmmount != 1 ? 0 : 1];
        if (GameControllerScript.Instance.SlotsAmmount >= 9)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory9slot;
            //ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages9slot, AdditionalGameCustomizer.Instance.ItemImageBGs9slot);
            for (int i = 0; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
        }
        if (GameControllerScript.Instance.SlotsAmmount == 8)
        {
            if (ItemManager.Instance.Inventory[8].ItemInstance != null&& ItemManager.Instance.ItemSelection != 7)
            {
                ItemManager.Instance.DropItem(8);
            }
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory8slot;
            //ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages8slot, AdditionalGameCustomizer.Instance.ItemImageBGs8slot);
            AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[8].SetActive(false);
            AdditionalGameCustomizer.Instance.ItemSlotsGameObj[8].SetActive(false);
            AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[8].SetActive(false);
            for (int i = 0; i < 8; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
        }
        if (GameControllerScript.Instance.SlotsAmmount == 7)
        {
            if (ItemManager.Instance.Inventory[7].ItemInstance != null && ItemManager.Instance.ItemSelection != 6)
            {
                ItemManager.Instance.DropItem(7);
            }
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory7slot;
            //ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages7slot, AdditionalGameCustomizer.Instance.ItemImageBGs7slot);
            for (int i = 6; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 7; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
        }
        if (GameControllerScript.Instance.SlotsAmmount == 6)
        {
            if (ItemManager.Instance.Inventory[6].ItemInstance != null && ItemManager.Instance.ItemSelection != 5)
            {
                ItemManager.Instance.DropItem(6);
            }
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory6slot;
            //ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages6slot, AdditionalGameCustomizer.Instance.ItemImageBGs6slot);
            for (int i = 5; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 6; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
        }
        if (GameControllerScript.Instance.SlotsAmmount == 5)
        {
            if (ItemManager.Instance.Inventory[5].ItemInstance != null && ItemManager.Instance.ItemSelection != 4)
            {
                ItemManager.Instance.DropItem(5);
            }
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory5slot;
            //ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages5slot, AdditionalGameCustomizer.Instance.ItemImageBGs5slot);
            for (int i = 4; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 5; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
        }
        if (GameControllerScript.Instance.SlotsAmmount == 4)
        {
            if (ItemManager.Instance.Inventory[4].ItemInstance != null && ItemManager.Instance.ItemSelection != 3)
            {
                ItemManager.Instance.DropItem(4);
            }
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory4slot;
            //ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages4slot, AdditionalGameCustomizer.Instance.ItemImageBGs4slot);
            for (int i = 3; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 4; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
        }
        if (GameControllerScript.Instance.SlotsAmmount == 3)
        {
            if (ItemManager.Instance.Inventory[3].ItemInstance != null && ItemManager.Instance.ItemSelection != 2)
            {
                ItemManager.Instance.DropItem(3);
            }
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory3slot;
            //ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages3slot, AdditionalGameCustomizer.Instance.ItemImageBGs3slot);
            for (int i = 2; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 3; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
        }
        if (GameControllerScript.Instance.SlotsAmmount == 2)
        {
            if (ItemManager.Instance.Inventory[2].ItemInstance != null && ItemManager.Instance.ItemSelection != 1)
            {
                ItemManager.Instance.DropItem(2);
            }
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory2slot;
            //ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages2slot, AdditionalGameCustomizer.Instance.ItemImageBGs2slot);
            for (int i = 1; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 2; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
        }
        if (GameControllerScript.Instance.SlotsAmmount == 1)
        {
            if (ItemManager.Instance.Inventory[1].ItemInstance != null && ItemManager.Instance.ItemSelection != 0)
            {
                ItemManager.Instance.DropItem(1);
            }
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory1slot;
            //ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages1slot, AdditionalGameCustomizer.Instance.ItemImageBGs1slot);
            for (int i = 0; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[0].SetActive(true);
            AdditionalGameCustomizer.Instance.ItemSlotsGameObj[0].SetActive(true);
            AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[0].SetActive(true);
        }
        ItemManager.Instance.UpdateItemUI();
    }
    #endregion
    public float MuchoStunDura,FamStunDura,ZerStunDura,BalStunDura;
    public bool MuchoStunCount,FamStunCount,ZerStunCount,BalStunCount;
}
