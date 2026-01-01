using UnityEngine;

public class WreckieboiPlayables : SpecialCharStuff
{
    public override void GiveItemOnSpawn()
    {
        ItemManager.Instance.CollectItem(itemIdToGive);
    }
    public int itemIdToGive;
}
