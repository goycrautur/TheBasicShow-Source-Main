using TMPro;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;
public class ItemManager : MonoBehaviour
{
    #region Singleton & Initialization
    public void Awake()
    {
        Instance = this;
        IndexItems();
    }
    #endregion
    public IEnumerator ButterfingersEffect(float duration)
	{
        for (int i = 0; i < ItemCanvasGroup.Length; i++)
        {
            while (ItemCanvasGroup[i].alpha > 0.5f)
            {
                ItemCanvasGroup[i].alpha -= Time.deltaTime;
            }
        }
		float time = duration;
		Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(buttfingStatus, duration);
		while (time > 0f)
		{
            ButterFingered = true;
			time -= Time.deltaTime;
			if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
			{
				newGauge.Set(duration, time);
				yield return null;
			}
		}
        ButterFingered = false;
		newGauge.Hide();
        for (int i = 0; i < ItemCanvasGroup.Length; i++)
        {
            while (ItemCanvasGroup[i].alpha < 1f)
            {
                ItemCanvasGroup[i].alpha += Time.deltaTime;
                yield return null;
            }
            ItemCanvasGroup[i].alpha = 1f;
        }
		yield break;
	}
    public void Butterfingers(float duration)
    {
        StartCoroutine(ButterfingersEffect(duration));
    }

    #region Input Handling
    private void Update()
    {
        FlashingTextCanvasGroup.alpha = Mathf.Lerp(FlashingTextCanvasGroup.alpha,0, 2.5f*Time.deltaTime);
        if (Time.timeScale == 0) return;
        for (int i = 0; i < Inventory.Length; i++) 
        {
            if (Inventory[i].ItemInstance != null && Inventory[i].ItemInstance.MultiUseMaxUsesCap > 1) 
            {
                Inventory[i].ItemStacksText.text = $"{Inventory[i].ItemInstance.Uses}/{Inventory[i].ItemInstance.MultiUseMaxUsesCap}";
            }
            else Inventory[i].ItemStacksText.text = "";
        }
        bool hisquidward = false;
        if (GameControllerScript.Instance.SlotsAmmount == 0) hisquidward = true;
        CanInteractWithSmth = !hisquidward || GameControllerScript.Instance.player.DeathCountdown;
        ItemNameText.enabled = CanInteractWithSmth;
        ItemInfoText.enabled = CanInteractWithSmth;
        theRestOfTheItemInfo.SetActive(CanInteractWithSmth);
        if (!CanInteractWithSmth) return;

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
            if (ButterFingered)
            {
                if (ButterFingersUnityEvent !=null) ButterFingersUnityEvent.Invoke();
                return;
            }
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
                if (SelectedItemObject.Uses >= 1) SelectedItemObject.AfterUse();
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
    public void AnimateSlotIfChanged(int slot)
    {
        bool hasItem = Inventory[slot].ItemID != 0;
        RawImage slotImage = Inventory[slot].ItemImages;
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

    public void AnimateSwap(int slot,Texture newtex = null)
    {
        
        RawImage slotImage = Inventory[slot].ItemImages;
        ItemImageSlide slider = slotImage.GetComponent<ItemImageSlide>();
        Texture newTex = newtex == null ? GetItemTexture(Inventory[slot].ItemID) : newtex;

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
            ItemsListShit fuckMan;
            fuckMan.name = $"{FoundItemObjects[i].Name} - With id: {FoundItemObjects[i].ItemID}";
            fuckMan.items = FoundItemObjects[i];
            RegisteredItemsList.Add(fuckMan);
        }
        Debug.Log($"{Items.Count} items total bitch real");
        Array.Resize(ref KeyIndex, Inventory.Length);

        SlotOccupied = new bool[Inventory.Length];
        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory[i].SlotID = i;
            var slider = Inventory[i].ItemImages.GetComponent<ItemImageSlide>();
            if (slider != null) 
            {
                slider.ForceClear();
            }
            else 
            {
                Inventory[i].ItemImages.texture = null; 
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
        bool shrinky = PlayerPrefsExtension.GetBool("shrink");
        if (shrinky && reduceinventory && Inventory[index].ItemID != 0) Singleton<OtherMainStuffManager>.Instance.ChangeItemSlot(GameControllerScript.Instance.SlotsAmmount-1);
        if (index >= 0 && index < Inventory.Length)
        {
            if (Inventory != null)
            {
                Inventory[index].ItemID = 0;
                Inventory[index].ItemInstance = null;
                AnimateSlotIfChanged(index);
            }
            else return;
        }
        UpdateItemUI();
    }

    private void SetItem(int index, int itemID, BaseItem item = null)
    {
        bool wasFull = Inventory[index].ItemID != 0;
        item?.transform.SetParent(GetItem(itemID).transform);

        ExecuteItem(Inventory[ItemSelection].ItemID, ExecutionType.Deselect);

        Inventory[index].ItemID = itemID;
        Inventory[index].ItemInstance = item;

        CreateItemInstance(index);

        ExecuteItem(Inventory[index].ItemID, ExecutionType.Pickup);
        if (ItemSelection == index)ExecuteItem(Inventory[index].ItemID, ExecutionType.Select);
        if (wasFull) AnimateSwap(index);
        else AnimateSlotIfChanged(index);
    }
    #endregion


    #region UI Management
    public void ClearAllItems()
    {
        for (int i = 0; i < Inventory.Length; i++) ClearItem(i);
        UpdateItemUI();
    }
    public void UpdateItemUI()
    {
        BaseItem SelectedItem = GetSelectedItemObject();
        for (int i = 0; i < Inventory.Length; i++) Inventory[i].ItemImageBGs.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        ItemNameText.text = $"{SelectedItem.Name}";
        ItemInfoText.text = $"{SelectedItem.ItmInfoText}";
        
        ItemNameIdCheck();
        
        Inventory[ItemSelection].ItemImageBGs.color = SelectionColor;
        Singleton<OtherMainStuffManager>.Instance.UpdateAltInventory();
        //ItemHoldImage.texture = SelectedItem.SmallSprite;
    }
    public void ItemNameIdCheck()
    {
        BaseItem SelectedItem = GetSelectedItemObject();
        if (SelectedItem.ItemID == 15) GameControllerScript.Instance.TETOOOOO.SetActive(true);
        else GameControllerScript.Instance.TETOOOOO.SetActive(false);
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
    public void RemoveItemUses(int slotId,int usesRemoveVal)
    {
        if (Inventory[slotId].ItemInstance == null)
        {
            UpdateItemUI();
            return;
        }
        BaseItem ItemObjectSon = Inventory[slotId].ItemInstance;
        if (!ItemObjectSon.InfiniteUses) ItemObjectSon.Uses -= usesRemoveVal;
        if (ItemObjectSon.Uses <= 0)
        {
            ExecuteItem(ItemObjectSon.ItemID, ExecutionType.Deselect);
            if (ItemObjectSon != null) Destroy(ItemObjectSon.gameObject);
            ClearItem(slotId);
        }
        UpdateItemUI();
    }

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
        if (Inventory[index].ItemID == 0)  return;
        if (Inventory[index].ItemInstance == null) Inventory[index].ItemInstance = TrulyMakeNewInstance(Inventory[index].ItemID);
    }
    public void SimplifiedItemCollect(int ItemID, BaseItem instance = null)
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
    public bool StackablesLimitDetection(int whatslot,bool trueOrNot)
    {
        if (Inventory[whatslot].ItemInstance.MaxUsesCap >= 2)
        {
            if (Inventory[whatslot].ItemInstance.OgUsesAmmount >= 2 && (Inventory[whatslot].ItemInstance.Uses > (Inventory[whatslot].ItemInstance.MultiUseMaxUsesCap- (Inventory[whatslot].ItemInstance.MaxUsesCap-1))))
            {
                Debug.Log($"slot {whatslot} has item it cant stack");
                return trueOrNot;
            }
        }
        if (Inventory[whatslot].ItemInstance.MaxUsesCap == 1)
        {
            if (Inventory[whatslot].ItemInstance.OgUsesAmmount >= 2)
            {
                
                Debug.Log($"slot {whatslot} has item it cant stack");
                return trueOrNot;
            }
        }
        Debug.Log($"slot {whatslot} has item it can stack");
        return !trueOrNot;
    }
    

    public void CollectItem(int ItemID, BaseItem instance = null)
    {
        
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i].ItemInstance != null && instance != null && Inventory[i].ItemInstance.ItemID == ItemID && Inventory[i].ItemInstance.Uses < Inventory[i].ItemInstance.MultiUseMaxUsesCap)
            {
                if (StackablesLimitDetection(i,true))
                {
                    Debug.Log($"SLOT {i} REACHED ITEM STACK LIMIT. FUCK");
                    continue;
                }
                else
                {
                    Inventory[i].ItemInstance.Uses += instance.Uses;
                    Debug.Log($"item uses : {Inventory[i].ItemInstance.Uses} at slot {i}");
                    UpdateItemUI();
                    return;
                }
            }
            if (Inventory[i].ItemInstance != null && instance == null && Inventory[i].ItemInstance.ItemID == ItemID && Inventory[i].ItemInstance.Uses < Inventory[i].ItemInstance.MultiUseMaxUsesCap)
            {
                BaseItem itemobj = GetItem(ItemID);
                if (StackablesLimitDetection(i,true))
                {
                    Debug.Log($"SLOT {i} REACHED ITEM STACK LIMIT. FUCK");
                    continue;
                }
                else
                {
                    Inventory[i].ItemInstance.Uses += itemobj.Uses;
                    Debug.Log($"item uses : {Inventory[i].ItemInstance.Uses} at slot {i}");
                    UpdateItemUI();
                    return;
                }
            }
        }
        SimplifiedItemCollect(ItemID,instance);
    }

    public void ReplaceCurrentItem(int ItemID, BaseItem ReplaceWithOtherCurrentItem = null)
    {
        if (Inventory[ItemSelection].ItemInstance != null)
        {
            Destroy(Inventory[ItemSelection].ItemInstance.gameObject);
        }

        SetItem(ItemSelection, ItemID,ReplaceWithOtherCurrentItem);
        UpdateItemUI();
    }
    private BaseItem TrulyMakeNewInstance(int id)
    {
        BaseItem newItemIguessBro = GetItem(id);
        GameObject NewInstance = Instantiate(newItemIguessBro.gameObject, transform);
        NewInstance.name = newItemIguessBro.name;
        return NewInstance.GetComponent<BaseItem>();
    }
    public void CreateDroppedItem(int IdIndex, Vector3 position,bool RandomSpawn = false,bool random = false)
    {
        BaseItem itemIGuess = GetItem(IdIndex);
        ActuallyCreatingTheDroppedItem(itemIGuess,position,true,RandomSpawn,random);
    }

    private void ActuallyCreatingTheDroppedItem(BaseItem baseitemz, Vector3 position, bool RawCreated = false,bool RandomlySpawn = false,bool RandomizedItem = false)
    {
        BaseItem item = baseitemz;
        //Vector3 spawnPosition = GameControllerScript.Instance.player.dropItemPos.position;
        //spawnPosition.y = 4;
        

        Vector3 spawnPosition = position;

        GameObject droppedItem = new GameObject($"Pickup_{item.Name}")
        {
            transform = { position = spawnPosition },
            tag = "Item"
        };

        if (RandomlySpawn) droppedItem.AddComponent<randomSpawnScript>();

        var pickup = droppedItem.AddComponent<PickupScript>();

        GameObject MapSpriteObject = new GameObject("mapSprite")
        {
            transform = { parent = droppedItem.transform, localPosition = new Vector3(0, spawnPosition.y + 45, 0), localScale = new Vector3(31.25f, 31.25f, 20f) },
            layer = 10
        };

        SpriteRenderer spriteRanderer = MapSpriteObject.AddComponent<SpriteRenderer>();
        if (AdditionalGameCustomizer.Instance.itemMapSprite != null && !item.SpecialItemIcon)
        {
            spriteRanderer.sprite = Sprite.Create(AdditionalGameCustomizer.Instance.itemMapSprite, new Rect(0, 0, AdditionalGameCustomizer.Instance.itemMapSprite.width, AdditionalGameCustomizer.Instance.itemMapSprite.height), new Vector2(0.5f, 0.5f), 100);
        }
        if (AdditionalGameCustomizer.Instance.SpecialItemMapSprite != null && item.SpecialItemIcon)
        {
            spriteRanderer.sprite = Sprite.Create(AdditionalGameCustomizer.Instance.SpecialItemMapSprite, new Rect(0, 0, AdditionalGameCustomizer.Instance.SpecialItemMapSprite.width, AdditionalGameCustomizer.Instance.SpecialItemMapSprite.height), new Vector2(0.5f, 0.5f), 100);
        }

        pickup.mapIconSprite = spriteRanderer;

        pickup.enabled = true;
        pickup.DroppedItem = true;
        pickup.ID = item.ItemID;
        pickup.GetType().GetField("PresentMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pickup, RandomizedItem);

        var collider = droppedItem.AddComponent<CapsuleCollider>();
        collider.isTrigger = true;
        collider.center = new Vector3(0, 1, 0);
        collider.radius = 1.5f;
        collider.height = 2f;

        GameObject spriteControllerObject = new GameObject("Item")
        {
            transform = { 
            parent = droppedItem.transform, localPosition = new Vector3(0f,1f,0f), localScale = new Vector3(2f, 2f, 2f) 
            }
        };

        GameObject spriteObject = new GameObject("Sprite")
        {
            transform = { 
            parent = spriteControllerObject.transform, localPosition = Vector3.zero, localScale = new Vector3(1f, 1f, 1f) 
            }
        };
        
        SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();

        var spriteController = spriteControllerObject.AddComponent<SpriteController>();
        spriteController.mainTex = item.BigSprite;
        spriteController.useBobbing = true;
        if (item.BigSprite is Texture2D texture)
        {
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), item.TexturePPUThing);
            
        }
        else  Debug.LogWarning("BigSprite is not a Texture2D, cannot create Sprite.");

        spriteRenderer.material = GameControllerScript.Instance.SpriteRenderer;
        var iconmap = MapSpriteObject.AddComponent<rotateToPlayerMinimapIcon>();
        iconmap.rotati = 90;
        if (!RawCreated)
        {
            item.transform.SetParent(droppedItem.transform);
            item.gameObject.SetActive(true);
        }
    }

    public void DropItem(int index)
    {
        if (Inventory[index].ItemInstance.ItemID == 0 || Inventory[index].ItemInstance == null) return;
        var item = Inventory[index];
        BaseItem itemToDrop = item.ItemInstance;
        Vector3 DropPosition = GameControllerScript.Instance.player.dropItemPos.position;
        DropPosition.y = 4;
        ActuallyCreatingTheDroppedItem(itemToDrop,DropPosition);
        ClearItem(index, false);
        UpdateItemUI();
    }
    #endregion

    public bool CanInteractWithSmth;

    #region Nested Types
    [Serializable]
    private enum ExecutionType { Use, Pickup, Select, Deselect }
    #endregion

    #region Fields & Serialized
    public Dictionary<string, BaseItem> Items = new Dictionary<string, BaseItem>();
    [Serializable]
    public struct ItemsListShit 
    {
        public string name;
        public BaseItem items;
    }
    public List<ItemsListShit> RegisteredItemsList = new List<ItemsListShit>();
    public HeldItem[] Inventory;
    public int ItemSelection = 0;
    private KeyCode[] KeyIndex = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };

    [Header("UI References")]
    public Sprite[] ItemSlotsSprites = new Sprite[3];
    public TextMeshProUGUI ItemNameText, ItemInfoText;
    public GameObject theRestOfTheItemInfo;
    [SerializeField] private Color SelectionColor = Color.red;
    [SerializeField] private RawImage ItemHoldImage;
    public static ItemManager Instance;
    public bool[] SlotOccupied;
    
    public CanvasGroup[] ItemCanvasGroup;
    public bool ButterFingered;
    public Sprite buttfingStatus;
    public UnityEvent ButterFingersUnityEvent;
    public CanvasGroup FlashingTextCanvasGroup;

    
    #endregion 
}