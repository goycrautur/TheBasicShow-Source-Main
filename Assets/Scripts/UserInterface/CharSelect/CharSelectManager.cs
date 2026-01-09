using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CharSelectManager : MonoBehaviour
{
    public void OnEnable()
    {
        changeCharStuff2(PlayerPrefs.GetInt("CharInt"));
    }
    public void changeCharStuff2(int CharacVal)
    {
        if (characterThing[CharacVal].NeedRequirements)
        {
            switch (characterThing[CharacVal].whatsTheRequirements.itNeedSmth)
            {
                case Requirements.NeedTypes.String:
                    string tempstring = PlayerPrefs.GetString(characterThing[CharacVal].whatsTheRequirements.ReqStringPPrefs, "");
                    if (tempstring == characterThing[CharacVal].whatsTheRequirements.PPrefsStringVal) characterThing[CharacVal].unlocked = true;
                    else characterThing[CharacVal].unlocked = false;
                    break;
                case Requirements.NeedTypes.Bool:
                    string tempboolstring = characterThing[CharacVal].whatsTheRequirements.ReqBoolPPrefsName;
                    bool tempbool = PlayerPrefsExtension.GetBool(tempboolstring);
                    if (tempbool = true) characterThing[CharacVal].unlocked = true; // tue is 1 sob
                    else characterThing[CharacVal].unlocked = false;
                    break;
                case Requirements.NeedTypes.Int:
                    int tempint = PlayerPrefs.GetInt(characterThing[CharacVal].whatsTheRequirements.ReqIntPPrefsName, 0);
                    if (tempint == characterThing[CharacVal].whatsTheRequirements.PPrefsIntVal) characterThing[CharacVal].unlocked = true;
                    else characterThing[CharacVal].unlocked = false;
                    break;
            }
        
        }
        ChangeSlotsIMGig(characterThing[CharacVal].SlotsSkin[0],characterThing[CharacVal].SlotsSkin[1],characterThing[CharacVal].SlotsSkin[2]);
        ChangeCharacter(characterThing[CharacVal].TextFontType,characterThing[CharacVal].SlotsAmmount,characterThing[CharacVal].CharSprite,characterThing[CharacVal].nameSprite,characterThing[CharacVal].TextUsesImagesSprite,characterThing[CharacVal].CharacterNameNormText,CharacVal);
    }
    public void ChangeCharacter(TMP_FontAsset whatFontToUse,int slots = 9,Sprite CharSpritese = null,Sprite nameSpriteaa = null,bool useTextImageSprites = false,string text = "",int num = 0 )
    {
        CharacterNameText.font = whatFontToUse;
        CharacterSprites.sprite = CharSpritese;
        TextNameSprites.enabled = useTextImageSprites;
        TextNameSprites.sprite = nameSpriteaa;
        if (characterThing[num].unlocked)
        {
            CharacterSprites.color = Color.white;
            CharacterNameText.text = !useTextImageSprites ?text : "";
            PlayerPrefs.SetString("CurrentCharacter", characterThing[num].CharSaveFileTag);
            PlayerPrefs.SetInt("CharInt", characterThing[num].characterValue);
            PlayerPrefs.Save();
        }
        if (!characterThing[num].unlocked)
        {
            CharacterNameText.text = "LOCKED";
            CharacterSprites.color = Color.black;
        }
        ChangeSlots(slots);
    }
    public void ChangeSlotsIMGig(Sprite leftslot = null,Sprite middleslot = null,Sprite rightslot = null)
    {
        ItemSlotsCharSprites[0] = leftslot;
        ItemSlotsCharSprites[1] = middleslot;
        ItemSlotsCharSprites[2] = rightslot;
    }
    public void ChangeSlots(int slots = 9)
    {
        if (slots == 0) return;
        mainSlotsAnimator.Rebind();
		mainSlotsAnimator.Play($"{slots}itemSlot", -1, 0f);
        ItemImageSlotsYea[slots-1].sprite = ItemSlotsCharSprites[2];
        for (int i = 0; i < slots; ++i)
        {
            SlotsAnimatorFNFStyle[i].Rebind();
        }
        for (int i = 1; i < slots-1; ++i)
        {
            ItemImageSlotsYea[i].sprite = ItemSlotsCharSprites[1];
        }
        ItemImageSlotsYea[0].sprite = ItemSlotsCharSprites[slots != 1 ?0 :1];
    }
    public TMP_Text CharacterNameText;
    public Animator mainSlotsAnimator;
    public Animator[] SlotsAnimatorFNFStyle;
    public Image CharacterSprites,TextNameSprites;
    public Sprite[] ItemSlotsCharSprites;
    public List<Image> ItemImageSlotsYea = new List<Image>();
    public List<CharacterValShit> characterThing = new List<CharacterValShit>();
    [Serializable]
	public class CharacterValShit
    {
		public Sprite nameSprite, CharSprite;
        public bool TextUsesImagesSprite;
        public TMP_FontAsset TextFontType;
        public string CharacterNameNormText;
        public int SlotsAmmount;
        public Sprite[] SlotsSkin;
        public string CharacterDescription;
        public string CharSaveFileTag;
        public PlayablesStats charStats;
        public int characterValue;
        public bool NeedRequirements;
        public Requirements whatsTheRequirements;
        public bool unlocked = true;
	}
}

