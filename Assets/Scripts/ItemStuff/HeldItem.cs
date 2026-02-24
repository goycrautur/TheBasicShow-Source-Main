using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct HeldItem
{
    public int ItemID;
    [HideInInspector]public int SlotID;

    public BaseItem ItemInstance;
    [Header("Item Images Stuff")]
    public Image ItemImageBGs;
    public RawImage ItemImages;
    public Image ItemImageSlots;
}