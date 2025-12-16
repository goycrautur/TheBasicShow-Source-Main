using UnityEngine;

public class VendingMachineScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        if (crazyMode && isOutOfGoods)
        {
            Debug.LogWarning("Conflict: CrazyMode and isOutOfGoods both true. Disabling CrazyMode.");
            crazyMode = false;
        }

        if (crazyMode)
        {
            VendingFront.material = CrazyFront;
            itemID = Random.Range(1, 48);
        }
    }
    #endregion

    #region Public Actions
    public void DispenseItem()
    {
        AudioSource audioDevice = GameControllerScript.Instance.audioDevice;
        if (AdditionalGameCustomizer.Instance.ReworkedCurrency)
        {
            if (!ItemManager.Instance.IsInventoryFull())
            {
                
                if (AdditionalGameCustomizer.Instance.Cash >= ItemCostRaldMoneyType)
                {
                    audioDevice.PlayOneShot(AdditionalGameCustomizer.Instance.aud_Drop);
                    AdditionalGameCustomizer.Instance.Cash = AdditionalGameCustomizer.Instance.Cash - ItemCostRaldMoneyType;
                    ItemManager.Instance.CollectItem(itemID);
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
            }

        }

        if (isOutOfGoods)
        {
            HandleOutOfGoodsState();
            return;
        }

        if (crazyMode)
        {
            itemID = Random.Range(1, 42);
        }
    }

    public void RestockVendingMachine()
    {
        if (!gameObject.CompareTag("Untagged")) return;

        gameObject.tag = "VendingMachine";
        VendingFront.material = NormalFront;
    }
    #endregion

    #region State Handlers
    private void HandleOutOfGoodsState()
    {
        if (!crazyMode)
        {
            VendingFront.material = outOfFront;
            gameObject.tag = "Untagged";
        }
    }
    #endregion

    #region Serialized Configuration
    [Header("Settings")]
    [SerializeField] private bool crazyMode = false;
    public bool isOutOfGoods = true;

    [Header("Materials")]
    [SerializeField] private Material CrazyFront;
    [SerializeField] private Material outOfFront, NormalFront;
    [SerializeField] private MeshRenderer VendingFront;

    [Header("Item Settings")]
    [SerializeField] private int itemID = 1;
    public int itemCostNormal = 1,insertedMoney;
    public double ItemCostRaldMoneyType = 0.25;
    #endregion
}