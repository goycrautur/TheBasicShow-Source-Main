using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public struct HeldItem
{
    public int ItemID;
    [HideInInspector]public int SlotID;

    public BaseItem ItemInstance;
    [Header("Item Slots Stuff")]
    public Image ItemImageBGs;
    public RawImage ItemImages;
    public Image ItemImageSlots;
    public TextMeshProUGUI ItemStacksText;
}