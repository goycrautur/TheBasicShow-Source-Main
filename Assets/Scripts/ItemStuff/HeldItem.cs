using System;
using UnityEngine;

[Serializable]
public struct HeldItem
{
    public int ItemID;

    public int SlotID;

    public BaseItem ItemInstance;
}