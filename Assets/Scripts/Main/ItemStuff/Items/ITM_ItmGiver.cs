using UnityEngine;

public class ITM_ItmGiver : BaseItem
{
    public override bool OnUse()
    {
        givebitch(howManyTimesToGive);
        return true;
    }
    private void givebitch(int howManyTimesToGivee)
    {
        if (howManyTimesToGivee < 10)
        {
            for (int i = 0; i < howManyTimesToGive; ++i)
            {
                givebitch2(i);
            }
        }
    }
    private void givebitch2(int i)
    {
        if (i == i)
        {
            ItemManager.Instance.CollectItem(!giveRandomItem ? ItemIDreal : Random.Range(RandItemIDMin,RandItemIDMax));
        }
    }
    
    [SerializeField] private int ItemIDreal,RandItemIDMin,RandItemIDMax,howManyTimesToGive;
    [SerializeField] private bool giveRandomItem;
}
