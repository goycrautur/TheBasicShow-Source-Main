using TMPro;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class ItemManager : MonoBehaviour
{
    #region Singleton & Initialization
    public void Awake()
    {
        Instance = this;
        IndexItems();
    }
    #endregion

    #region Input Handling
    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        for (int i = 0; i < KeyIndex.Length; i++)
        {
            bool keyCode = Singleton<InputManager>.Instance.GetActionKey(InputAction.Slot0 + 0 + i);
            if (keyCode)
            {
                ExecuteItem(Inventory[ItemSelection].ItemID, ExecutionType.Deselect);
                ExecuteItem(Inventory[i].ItemID, ExecutionType.Select);
                ItemSelection = i;
                UpdateItemUI();
                break;
            }
        }

        if (Input.GetMouseButtonDown(1) || Singleton<InputManager>.Instance.GetActionKey(InputAction.UseItem))
        {
            int CurrItem = GetSelectedItem();
            bool ShouldDestroy = ExecuteItem(CurrItem);
            BaseItem SelectedItemObject = GetSelectedItemObject();

            if (CurrItem == GetSelectedItem())
            {
                if (!ShouldDestroy)
                {
                    UpdateItemUI();
                    return;
                }

                if (!SelectedItemObject.InfiniteUses) SelectedItemObject.Uses--;
                if (Inventory[ItemSelection].ItemInstance != null)
                {
                    SlotsItemHandlingStuffIdk(ItemSelection,Inventory[ItemSelection].ItemID,SelectedItemObject);
                }
                if (SelectedItemObject.Uses <= 0)
                {
                    ExecuteItem(GetSelectedItem(), ExecutionType.Deselect);
                    if (Inventory[ItemSelection].ItemInstance != null)
                    {
                        Destroy(Inventory[ItemSelection].ItemInstance.gameObject);
                    }
                    
                    ClearItem(ItemSelection);
                }
            }

            UpdateItemUI();
        }

        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0)
        {
            UpdateItemSelection(scrollDelta > 0 ? -1 : 1);
        }
    }
    #endregion
    #region Animation Logic
    private void AnimateSlotIfChanged(int slot)
    {
        bool hasItem = Inventory[slot].ItemID != 0;
        RawImage slotImage = ItemImages[slot];
        ItemImageSlide slider = slotImage.GetComponent<ItemImageSlide>();

        if (slider != null)
        {
            if (hasItem && !SlotOccupied[slot])
            {
                slider.SlideIn(GetItemTexture(Inventory[slot].ItemID));
                SlotOccupied[slot] = true;
            }
            else if (!hasItem && SlotOccupied[slot])
            {
                slider.SlideOut();
                SlotOccupied[slot] = false;
            }
            else if (!hasItem)
            {
               slider.ForceClear();
            }
        }
        else
        {
            slotImage.texture = hasItem ? GetItemTexture(Inventory[slot].ItemID) : null;
            SlotOccupied[slot] = hasItem;
        }
    } 

    private void AnimateSwap(int slot)
    {
        RawImage slotImage = ItemImages[slot];
        ItemImageSlide slider = slotImage.GetComponent<ItemImageSlide>();
        Texture newTex = GetItemTexture(Inventory[slot].ItemID);

        if (slider != null)
        {
            if (Inventory[slot].ItemID != 0)
            {
                slider.PlaySwapAnimation(newTex);
            }
            else
            {
                slider.ForceClear();
            }
        }
        else
        {
            slotImage.texture = newTex;
        }
    }
    
    private Texture GetItemTexture(int itemID)
    {
        if (itemID == 0)
        {
            return null;
        }

        var itemBase = Items.ElementAt(itemID).Value;
        return itemBase != null ? itemBase.SmallSprite : null;
    }
    #endregion

    #region Item Execution & Inventory Management
    private void IndexItems()
    {
        BaseItem[] FoundItemObjects = GetComponentsInChildren<BaseItem>();
        Items.Clear();
        for (int i = 0; i < FoundItemObjects.Length; i++)
        {
            Items.Add(FoundItemObjects[i].Name, FoundItemObjects[i]);
        }

        Array.Resize(ref Inventory, ItemImages.Count);
        Array.Resize(ref KeyIndex, Inventory.Length);

        SlotOccupied = new bool[Inventory.Length];

        for (int i = 0; i < ItemImages.Count; i++)
        {
            var slider = ItemImages[i].GetComponent<ItemImageSlide>();
            if (slider != null) 
            {
                slider.ForceClear();
            }
            else 
            {
                ItemImages[i].texture = null; 
            }
        }

        UpdateItemUI();
    }

    private bool ExecuteItem(int ID, ExecutionType type = ExecutionType.Use)
    {
        BaseItem item = GetItem(ID);
        if (item == null)
        {
            Debug.LogError($"Attempted to execute item with ID {ID} and type {type}, but GetItem returned null");
            return false;
        }

        switch (type)
        {
            case ExecutionType.Use:
                return item.OnUse();
            case ExecutionType.Pickup:
                item.OnPickup();
                break;
            case ExecutionType.Select:
                item.OnSelect();
                break;
            case ExecutionType.Deselect:
                item.OnDeselect();
                break;
        }
        return false;
    }

    private void UpdateItemSelection(int changeAmount)
    {
        ExecuteItem(Inventory[ItemSelection].ItemID, ExecutionType.Deselect);
        ItemSelection = (ItemSelection + changeAmount + Inventory.Length) % Inventory.Length;
        ExecuteItem(Inventory[ItemSelection].ItemID, ExecutionType.Select);
        UpdateItemUI();
    }

    public void ClearItem(int index,bool reduceinventory = true)
    {
        //if (reduceinventory && Inventory[index].ItemID != 0)
        //{
        //    GameControllerScript.Instance.SlotsAmmount = GameControllerScript.Instance.SlotsAmmount-1;
        //    Singleton<OtherMainStuffManager>.Instance.slot();
        //}
        Inventory[index].ItemID = 0;
        Inventory[index].ItemInstance = null;
        AnimateSlotIfChanged(index);
        UpdateItemUI();
        SlotsItemHandlingStuffIdk(index,0,null);
    }

    private void SetItem(int index, int itemID, BaseItem item = null)
    {
        bool wasFull = Inventory[index].ItemID != 0;
        item?.transform.SetParent(GetItem(itemID).transform);

        ExecuteItem(Inventory[ItemSelection].ItemID, ExecutionType.Deselect);

        Inventory[index].ItemID = itemID;
        Inventory[index].ItemInstance = item;
        SlotsItemHandlingStuffIdk(index,itemID,item);

        CreateItemInstance(index);

        ExecuteItem(Inventory[index].ItemID, ExecutionType.Pickup);
        if (ItemSelection == index)
        {
            ExecuteItem(Inventory[index].ItemID, ExecutionType.Select);
        }
        if (wasFull)
        {
            AnimateSwap(index);
        }
        else
        {
            AnimateSlotIfChanged(index);
        }
    }
    #endregion
    public void SlotsItemHandlingStuffIdk(int index, int itemID, BaseItem Whatitem = null)
    {
        for (int i = 0; i < 9; ++i)
        {
            AdditionalGameCustomizer.Instance.Inventory9slot[index].ItemID = itemID;
            AdditionalGameCustomizer.Instance.Inventory9slot[index].ItemInstance = Whatitem;
            if (index < AdditionalGameCustomizer.Instance.Inventory8slot.Length)
            {
            AdditionalGameCustomizer.Instance.Inventory8slot[index].ItemID = itemID;
            AdditionalGameCustomizer.Instance.Inventory8slot[index].ItemInstance = Whatitem;
            }
            if (index < AdditionalGameCustomizer.Instance.Inventory7slot.Length)
            {
            AdditionalGameCustomizer.Instance.Inventory7slot[index].ItemID = itemID;
            AdditionalGameCustomizer.Instance.Inventory7slot[index].ItemInstance = Whatitem;
            }
            if (index < AdditionalGameCustomizer.Instance.Inventory6slot.Length)
            {
            AdditionalGameCustomizer.Instance.Inventory6slot[index].ItemID = itemID;
            AdditionalGameCustomizer.Instance.Inventory6slot[index].ItemInstance = Whatitem;
            }
            if (index < AdditionalGameCustomizer.Instance.Inventory5slot.Length)
            {
            AdditionalGameCustomizer.Instance.Inventory5slot[index].ItemID = itemID;
            AdditionalGameCustomizer.Instance.Inventory5slot[index].ItemInstance = Whatitem;
            }
            if (index < AdditionalGameCustomizer.Instance.Inventory4slot.Length)
            {
            AdditionalGameCustomizer.Instance.Inventory4slot[index].ItemID = itemID;
            AdditionalGameCustomizer.Instance.Inventory4slot[index].ItemInstance = Whatitem;
            }
            if (index < AdditionalGameCustomizer.Instance.Inventory3slot.Length)
            {
            AdditionalGameCustomizer.Instance.Inventory3slot[index].ItemID = itemID;
            AdditionalGameCustomizer.Instance.Inventory3slot[index].ItemInstance = Whatitem;
            }
            if (index < AdditionalGameCustomizer.Instance.Inventory2slot.Length)
            {
            AdditionalGameCustomizer.Instance.Inventory2slot[index].ItemID = itemID;
            AdditionalGameCustomizer.Instance.Inventory2slot[index].ItemInstance = Whatitem;
            }
            if (index < AdditionalGameCustomizer.Instance.Inventory1slot.Length)
            {
            AdditionalGameCustomizer.Instance.Inventory1slot[index].ItemID = itemID;
            AdditionalGameCustomizer.Instance.Inventory1slot[index].ItemInstance = Whatitem;
            }
        }
    }

    #region UI Management
    public void UpdateItemUI()
    {
        BaseItem SelectedItem = GetSelectedItemObject();
        for (int i = 0; i < ItemImages.Count; i++)
        {
            ItemImageBGs[i].color = Color.white;
        }
        ItemNameText.text = $"{SelectedItem.Name}";
        ItemInfoText.text = $"{SelectedItem.ItmInfoText}";

        if (SelectedItem.Uses > 1)
        {
            ItemNameText.text += $" ({SelectedItem.Uses})";
        }
        if (SelectedItem.ItemID == 15)
        {
            GameControllerScript.Instance.TETOOOOO.SetActive(true);
        }
        else
        {
            GameControllerScript.Instance.TETOOOOO.SetActive(false);
        }

        ItemImageBGs[ItemSelection].color = SelectionColor;
        //ItemHoldImage.texture = SelectedItem.SmallSprite;
    }
    #endregion

    #region Function Handling
    public BaseItem GetItem(string name)
    {
        if (Items.ContainsKey(name))
        {
            return Items[name];
        }

        return null;
    }

    public BaseItem GetItem(int id)
    {
        return GetItem(Items.ElementAt(id).Value.Name);
    }

    public void AddItem(BaseItem item)
    {
        if (item != null && !Items.ContainsKey(item.name))
        {
            Items.Add(item.name, item);
            return;
        }

        Debug.LogWarning("Attempted to add an item that was either null or was already apart of the items dictionary");
    }

    public void RemoveItem(string name)
    {
        if (Items.ContainsKey(name))
        {
            Items.Remove(name);
            return;
        }

        Debug.LogWarning("Attempted to remove an item that wasn't apart of the items dictionary");
    }

    public void RemoveItem(BaseItem item) => RemoveItem(item.name);

     public int GetSelectedItem() => Inventory[ItemSelection].ItemID;

    public bool IsInventoryFull() => Inventory.All(i => i.ItemID != 0);
    public void RemoveItemFromInventory(BaseItem item)
    {
        ExecuteItem(item.ItemID, ExecutionType.Deselect);

        for (int slot = 0; slot < Inventory.Length; slot++)
        {
            if (HasItemInInventorySlot(slot, item))
            {
                int index = GetItemSelectionOfItem(slot, item);
                if (index >= 0 && index < Inventory.Length && Inventory[index].ItemInstance != null)
                {
                    Destroy(Inventory[index].ItemInstance.gameObject);
                    ClearItem(index,false);
                    break;
                }
            }
        }

        UpdateItemUI();
    }
    public BaseItem GetSelectedItemObject()
    {
        if (Inventory[ItemSelection].ItemID != 0 && Inventory[ItemSelection].ItemInstance == null)
        {
            CreateItemInstance();
            return Inventory[ItemSelection].ItemInstance.GetComponent<BaseItem>();
        }

        return Inventory[ItemSelection].ItemInstance != null ? Inventory[ItemSelection].ItemInstance : GetItem(GetSelectedItem());
    }

    public bool HasNoItems()
    {
        return Inventory.All(i => i.ItemID == 0);
    }

    public bool IsEmptyInventory() => Inventory.All(i => i.ItemID == 0);

    public bool HasItemInInventorySlot(int slotID, BaseItem item)
    {
        if (slotID < 0 || slotID >= Inventory.Length) return false;

        int index = GetItemSelectionOfItem(slotID, item);
        return index != -1 && index < Inventory.Length && Inventory[index].ItemID == item.ItemID;
    }

    public int GetItemSelectionOfItem(int slotID, BaseItem item)
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i].SlotID == slotID && Inventory[i].ItemID == item.ItemID)
            {
                return i;
            }
        }
        return -1;
    }
    #endregion

    #region Item Instances & Collection
    private void CreateItemInstance(int? at = null)
    {
        int index = at ?? ItemSelection;
        if (Inventory[index].ItemID == 0)
        {
            return;
        }
        if (Inventory[index].ItemInstance == null)
        {
            BaseItem itemobj = GetItem(Inventory[index].ItemID);
            GameObject NewInstance = Instantiate(itemobj.gameObject, transform);
            NewInstance.name = itemobj.gameObject.name;
            Inventory[index].ItemInstance = NewInstance.GetComponent<BaseItem>();
        }
    }

    public void CollectItem(int ItemID, BaseItem instance = null)
    {
        if (GetSelectedItem() == 0)
        {
            SetItem(ItemSelection, ItemID, instance);
            UpdateItemUI();
            return;
        }

        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i].ItemID == 0)
            {
                SetItem(i, ItemID, instance);
                UpdateItemUI();
                return;
            }
        }

        SetItem(ItemSelection, ItemID, instance);
        UpdateItemUI();
    }

    public void ReplaceCurrentItem(int ItemID)
    {
        if (Inventory[ItemSelection].ItemInstance != null)
        {
            Destroy(Inventory[ItemSelection].ItemInstance.gameObject);
        }

        SetItem(ItemSelection, ItemID);
        UpdateItemUI();
    }

    public void DropItem(int index)
    {
        var item = Inventory[index];
        if (item.ItemID == 0 || item.ItemInstance == null)
        {
            return;
        }

        BaseItem itemToDrop = item.ItemInstance;
        Vector3 spawnPosition = GameControllerScript.Instance.player.dropItemPos.position;
        spawnPosition.y = 4;

        GameObject droppedItem = new GameObject($"Pickup_{itemToDrop.Name}")
        {
            transform = { position = spawnPosition },
            tag = "Item"
        };

        var pickup = droppedItem.AddComponent<PickupScript>();
        pickup.DroppedItem = true;
        pickup.ID = Inventory[index].ItemID;
        pickup.GetType().GetField("PresentMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pickup, false);

        var collider = droppedItem.AddComponent<CapsuleCollider>();
        collider.isTrigger = true;
        collider.center = new Vector3(0, 1, 0);
        collider.radius = 1.5f;
        collider.height = 2f;

        GameObject spriteObject = new GameObject("Sprite")
        {
            transform = { parent = droppedItem.transform, localPosition = Vector3.zero, localScale = new Vector3(2f, 2f, 2f) }
        };
        GameObject MapSpriteObject = new GameObject("mapSprite")
        {
            transform = { parent = droppedItem.transform, localPosition = new Vector3(0, spawnPosition.y + 45, 0), localScale = new Vector3(31.25f, 31.25f, 20f) },
            layer = 10
        };
        
        SpriteRenderer spriteRanderer = MapSpriteObject.AddComponent<SpriteRenderer>();
        if (AdditionalGameCustomizer.Instance.itemMapSprite != null && !item.ItemInstance.SpecialItemIcon)
        {
            spriteRanderer.sprite = Sprite.Create(AdditionalGameCustomizer.Instance.itemMapSprite, new Rect(0, 0, AdditionalGameCustomizer.Instance.itemMapSprite.width, AdditionalGameCustomizer.Instance.itemMapSprite.height), new Vector2(0.5f, 0.5f), 100);
        }
        if (AdditionalGameCustomizer.Instance.SpecialItemMapSprite != null && item.ItemInstance.SpecialItemIcon)
        {
            spriteRanderer.sprite = Sprite.Create(AdditionalGameCustomizer.Instance.SpecialItemMapSprite, new Rect(0, 0, AdditionalGameCustomizer.Instance.SpecialItemMapSprite.width, AdditionalGameCustomizer.Instance.SpecialItemMapSprite.height), new Vector2(0.5f, 0.5f), 100);
        }
        SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
        pickup.mapIconSprite = spriteRanderer;
        if (itemToDrop.BigSprite is Texture2D texture)
        {
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), itemToDrop.TexturePPUThing);
        }
        else
        {
            Debug.LogWarning("BigSprite is not a Texture2D, cannot create Sprite.");
        }

        spriteRenderer.material = GameControllerScript.Instance.SpriteRenderer;
        var iconmap = MapSpriteObject.AddComponent<rotateToPlayerMinimapIcon>();
        iconmap.rotati = 90;
        spriteObject.AddComponent<Billboard>().doNotOptimize = true;
        spriteObject.AddComponent<PickupAnimationScript>();
        spriteObject.AddComponent<AddToLightingWhitelist>();

        itemToDrop.transform.SetParent(droppedItem.transform);
        itemToDrop.gameObject.SetActive(true);

        ClearItem(index, false);
        UpdateItemUI();
    }
    #endregion

    #region Change References void (stolen from null decompile LOL)
    public void ChangeReferences(List<RawImage> itemImages, List<Image> itemImgBgs)
    {
        ItemImages = itemImages;
        ItemImageBGs = itemImgBgs;
    }
    #endregion

    #region Nested Types
    [Serializable]
    private enum ExecutionType { Use, Pickup, Select, Deselect }
    #endregion

    #region Fields & Serialized
    private Dictionary<string, BaseItem> Items = new Dictionary<string, BaseItem>();
    public HeldItem[] Inventory;
    public int ItemSelection = 0;
    private KeyCode[] KeyIndex = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };

    [Header("UI References")]
    [SerializeField] private List<RawImage> ItemImages = new List<RawImage>();
    [SerializeField] private List<Image> ItemImageBGs = new List<Image>();
    [SerializeField] public TextMeshProUGUI ItemNameText, ItemInfoText;
    [SerializeField] private Color SelectionColor = Color.red;
    [SerializeField] private RawImage ItemHoldImage;
    public static ItemManager Instance;
    public bool[] SlotOccupied;
    
    #endregion 
}