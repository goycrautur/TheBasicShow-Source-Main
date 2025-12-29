using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CharSelectManager : MonoBehaviour
{
    public void Start()
    {
        Debug.Log(characterThing.Count);
    }
    public void changeCharStuff2(int CharacVal)
    {
        ChangeSlotsIMGig(characterThing[CharacVal].SlotsSkin[0],characterThing[CharacVal].SlotsSkin[1],characterThing[CharacVal].SlotsSkin[2]);
        ChangeCharacter(characterThing[CharacVal].TextFontType,characterThing[CharacVal].SlotsAmmount,characterThing[CharacVal].CharSprite,characterThing[CharacVal].nameSprite,characterThing[CharacVal].TextUsesImagesSprite,characterThing[CharacVal].CharacterNameNormText);
    }
    public void ChangeCharacter(TMP_FontAsset whatFontToUse,int slots = 9,Sprite CharSpritese = null,Sprite nameSpriteaa = null,bool useTextImageSprites = false,string text = "")
    {
        CharacterNameText.text = !useTextImageSprites ?text : "";
        CharacterNameText.font = whatFontToUse;
        CharacterSprites.sprite = CharSpritese;
        TextNameSprites.enabled = useTextImageSprites;
        TextNameSprites.sprite = nameSpriteaa;
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
        ItemImageSlotsYea[0].sprite = ItemSlotsCharSprites[0];
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
        public string CharSaveFileTag;
        public int characterValue;
	}
}

