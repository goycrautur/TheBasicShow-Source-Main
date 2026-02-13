using UnityEngine;
using TMPro;

public class VendingMachineScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        ogOutOfGoodsValue = whenToOutOfGoods;
        if (crazyMode && isOutOfGoods)
        {
            Debug.LogWarning("Conflict: CrazyMode and isOutOfGoods both true. Disabling CrazyMode.");
            crazyMode = false;
        }

        if (crazyMode)
        {
            VendingFront.material = CrazyFront;
            itemID = Random.Range(1, ItemManager.Instance.Items.Count);
        }
        WasItShowItemLeftBefor = showsHowManyItemLeft;
        WasItShowMoneyNeededBefor = showsHowManyMoneyNeeded;
    }
    #endregion
    public void Update()
    {
        moneyNeeded = ItemCostRaldMoneyType-(0.25*(insertedMoney));
        if (MoneyNeededText != null) 
        {
            MoneyNeededTextGmbObj.SetActive(showsHowManyMoneyNeeded); 
            MoneyNeededText.text = !AdditionalGameCustomizer.Instance.ReworkedCurrency && itemCostNormal != 1 ? $"{insertedMoney}/{itemCostNormal}" + '\n' + "Money Inserted" : !AdditionalGameCustomizer.Instance.ReworkedCurrency && itemCostNormal == 1 ? "" :"Money Needed" + '\n' + moneyNeeded+"$";
        }
        if (ItemLeftText != null) 
        {
            ItemLeftTextGmbObj.SetActive(showsHowManyItemLeft);
            ItemLeftText.text = isOutOfGoods ? $"{whenToOutOfGoods}/{ogOutOfGoodsValue}" + '\n' + "Item Left" : $"Unlimited" + '\n' + "Item Left";
        }
    }

    #region Public Actions
    public void rollItem()
    {
        int itemid = Random.Range(1, ItemManager.Instance.Items.Count);
        BaseItem item = ItemManager.Instance.GetItem(itemid);
        if (item.blacklistFromGamblingVending) 
        {
            rollItem();
            Debug.Log("item blacklisted, rerolling");
            return;
        }
        else itemID = itemid;
    }
    public void DispenseItem()
    {
        if (crazyMode)
        {
            rollItem();
        }
        AudioSource audioDevice = GameControllerScript.Instance.audioDevice;
        if (AdditionalGameCustomizer.Instance.ReworkedCurrency)
        {
            if (!ItemManager.Instance.IsInventoryFull())
            {
                
                if (AdditionalGameCustomizer.Instance.Cash >= moneyNeeded)
                {
                    audioDevice.PlayOneShot(AdditionalGameCustomizer.Instance.aud_Drop);
                    AdditionalGameCustomizer.Instance.Cash = AdditionalGameCustomizer.Instance.Cash - moneyNeeded;
                    ItemManager.Instance.CollectItem(itemID);
                    insertedMoney = 0;
                    if (isOutOfGoods)
                    {
                        HandleOutOfGoodsState();
                        return;
                    }
                }
                else
                {
                    audioDevice.PlayOneShot(GameControllerScript.Instance.LoudIncorecBugger);
                }
            }
            else
            {
                Debug.Log("Inventory full. Cannot collect item.");
            }
        }
        else
        {
            if (insertedMoney == itemCostNormal)
            {
                ItemManager.Instance.CollectItem(itemID);
                insertedMoney = 0;
                if (isOutOfGoods)
                {
                    HandleOutOfGoodsState();
                    return;
                }
            }

        }
    }

    public void RestockVendingMachine(bool AddOnto = false)
    {
        //if (!gameObject.CompareTag("Untagged")) return;
        showsHowManyItemLeft = WasItShowItemLeftBefor;
        showsHowManyMoneyNeeded = WasItShowMoneyNeededBefor;
        if (!AddOnto) whenToOutOfGoods = ogOutOfGoodsValue;
        else whenToOutOfGoods += ogOutOfGoodsValue;
        gameObject.tag = "VendingMachine";
        VendingFront.material = NormalFront;
    }
    #endregion

    #region State Handlers
    private void HandleOutOfGoodsState()
    {
        whenToOutOfGoods -= 1;
        if (whenToOutOfGoods < 1)
        {
            if (!crazyMode)
            {
                showsHowManyItemLeft = false;
                showsHowManyMoneyNeeded = false;
                VendingFront.material = outOfFront;
                gameObject.tag = "Untagged";
            }
        }
    }
    #endregion

    #region Serialized Configuration
    [Header("Settings")]
    [SerializeField] private bool crazyMode = false;
    public bool isOutOfGoods = true,showsHowManyItemLeft=false,showsHowManyMoneyNeeded=false;
    private bool WasItShowItemLeftBefor,WasItShowMoneyNeededBefor;
    public TMP_Text MoneyNeededText,ItemLeftText;
    public GameObject MoneyNeededTextGmbObj,ItemLeftTextGmbObj;

    [Header("Materials")]
    [SerializeField] private Material CrazyFront;
    [SerializeField] private Material outOfFront, NormalFront;
    [SerializeField] private MeshRenderer VendingFront;

    [Header("Item Settings")]
    [SerializeField] private int itemID = 1;
    public int itemCostNormal = 1,insertedMoney,whenToOutOfGoods=1,ogOutOfGoodsValue;
    public double ItemCostRaldMoneyType = 0.25,moneyNeeded;
    #endregion
}