using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemSlotsCharSelectManager : MonoBehaviour
{
    public void ChangeSlots(int slots = 9)
    {
        if (slots == 0) return;
        mainSlotsAnimator.Rebind();
		mainSlotsAnimator.Play($"{slots}itemSlot", -1, 0f);
        ItemImageSlotsYea[slots-1].sprite = ItemSlotsCharSprites[2+(CharacterValue*3)];
        for (int i = 0; i < slots; ++i)
        {
            SlotsAnimatorFNFStyle[i].Rebind();
        }
        for (int i = 1; i < slots-1; ++i)
        {
            ItemImageSlotsYea[i].sprite = ItemSlotsCharSprites[1+(CharacterValue*3)];
        }
        ItemImageSlotsYea[0].sprite = ItemSlotsCharSprites[0+(CharacterValue*3)];
    }
    public int CharacterValue;
    public Animator mainSlotsAnimator;
    public Animator[] SlotsAnimatorFNFStyle;
    public Sprite[] ItemSlotsCharSprites;
    public List<Image> ItemImageSlotsYea = new List<Image>();
}

