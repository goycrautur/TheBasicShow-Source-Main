using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PickupScript : Interactable
{
    #region Initialization Logic
    public void Start()
    {
        if (killafterpickup) DroppedItem = true;
        cachedSprites = new Dictionary<int, Sprite>();
        spritContro = GetComponentInChildren<SpriteController>();
        if (PresentMode)
        {
            spritContro.targetRenderer.sprite = GameControllerScript.Instance.Present;
            rollItem();
        }
        if (SpawnAtRandom)
        {
            wanderer = FindObjectOfType<AILocationSelectorScript>();

            GameObject Set = GameObject.Find("AI_LocationSelector");
            location = Set.transform;

            location.position = wanderer.SetNewTargetForAgent(null, "present");
            transform.position = location.position + Vector3.up * 4f;
        }
        
        OriginalSprite = spritContro.targetRenderer.sprite;
        originalId = ID;
    }
    public void ItemRespawning()
    {
        spritContro.targetRenderer.sprite = OriginalSprite;
        HideShitsLogic(true,true);
        ID = originalId;
    }
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
        else ID = itemid;
    }
    public void itsPresentTime(bool resetIDONLY = false)
    {
        if (!PresentMode && !resetIDONLY) 
        {
            spritContro.targetRenderer.sprite = GameControllerScript.Instance.Present;
            PresentMode = true;
        }
        rollItem();
    }
    #endregion
    public void OnEnable()
    {
        HideShitsLogic(true);
    }
    public void OnDisable()
    {
        if (mapIconSprite != null) mapIconSprite.enabled = false;
        hiding = true;
    }
    public void moneCollectStuff()
    {
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(GameControllerScript.Instance.lbams.MoneyCollect);
        
        BaseItem orgigItem = GetHeldInstance();
        AdditionalGameCustomizer.Instance.Cash += 1 * (orgigItem != null ? orgigItem.Uses : 1);
        if (DroppedItem) HideShitsLogic(false,false,true);
        else HideShitsLogic(false);
    }

    #region Player Interaction
    public override void Interact()
    {
        if (hiding) return;
        BaseItem holdingitem = ItemManager.Instance.GetSelectedItemObject();
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(GameControllerScript.Instance.lbams.ItemCollect);
        if (PresentMode) GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(GameControllerScript.Instance.lbams.gambling);
        if (ID == 5)
        {
            if (ZerullClassic.Instance.realBossStarted) ZerullClassic.Instance.objects -= 1;
            if (AdditionalGameCustomizer.Instance.ReworkedCurrency & ID == 5)
            {
                moneCollectStuff();
                return;
            }
        }
        if (SlotStuffs(true))
        {
            if (!DroppedItem)
            {
                HideShitsLogic(false);
                if (ZerullClassic.Instance.realBossStarted) ZerullClassic.Instance.objects -= 1;
            }
            else HideShitsLogic(false,false,true);

            ItemManager.Instance.CollectItem(ID, GetHeldInstance());
            return;
        }
        else if (SlotStuffs(false))
        {
            if (holdingitem.Unswapable) return;
            HideShitsLogic(true);
        }
        //same shits copied from item manager dawg :sob:
        for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
        {
            if (ItemManager.Instance.Inventory[i].ItemInstance != null && GetHeldInstance() != null && ItemManager.Instance.Inventory[i].ItemInstance.ItemID == ID && ItemManager.Instance.Inventory[i].ItemInstance.Uses < ItemManager.Instance.Inventory[i].ItemInstance.MultiUseMaxUsesCap)
            {
                if (ItemManager.Instance.StackablesLimitDetection(i,true))
                {
                    Debug.Log($"SLOT {i} REACHED ITEM STACK LIMIT WHILE PICKING UP. FUCK");
                    continue;
                }
                else
                {
                    ItemManager.Instance.Inventory[i].ItemInstance.Uses += GetHeldInstance().Uses;
                    Debug.Log($"item uses : {ItemManager.Instance.Inventory[i].ItemInstance.Uses} at slot {i} after picking up when inven full");
                    HideShitsLogic(false);
                    ItemManager.Instance.UpdateItemUI();
                    return;
                }
            }
            if (ItemManager.Instance.Inventory[i].ItemInstance != null && GetHeldInstance() == null && ItemManager.Instance.Inventory[i].ItemInstance.ItemID == ID && ItemManager.Instance.Inventory[i].ItemInstance.Uses < ItemManager.Instance.Inventory[i].ItemInstance.MultiUseMaxUsesCap)
            {
                BaseItem itemobj = ItemManager.Instance.GetItem(ID);
                if (ItemManager.Instance.StackablesLimitDetection(i,true))
                {
                    Debug.Log($"SLOT {i} REACHED ITEM STACK LIMIT WHILE PICKING UP. FUCK");
                    continue;
                }
                else
                {
                    ItemManager.Instance.Inventory[i].ItemInstance.Uses += itemobj.Uses;
                    Debug.Log($"item uses : {ItemManager.Instance.Inventory[i].ItemInstance.Uses} at slot {i} after picking up when inven full");
                    HideShitsLogic(false);
                    ItemManager.Instance.UpdateItemUI();
                    return;
                }
            }

        }
        ItemSwapLogic();
    }
    public void ItemSwapLogic()
    {
        int orgID = ID;
        BaseItem orgItem = GetHeldInstance();

        ID = ItemManager.Instance.GetSelectedItem();

        ItemSwapBaseitem = ItemManager.Instance.GetSelectedItemObject();
        ItemSwapBaseitem.transform.parent = transform;

        if (!cachedSprites.ContainsKey(ID))
        {
            Texture itemTexture = ItemManager.Instance.GetItem(ID).BigSprite;
            Sprite itemSprite = Sprite.Create((Texture2D)itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), ItemSwapBaseitem.TexturePPUThing);
            cachedSprites.Add(ID, itemSprite);
        }

        spritContro.targetRenderer.sprite = cachedSprites[ID];
        gameObject.name = $"Pickup_{ItemManager.Instance.GetItem(ID).Name}";

        ItemManager.Instance.CollectItem(orgID, orgItem);
    }
    public void HideShitsLogic(bool wuh,bool fadein = false,bool DestroyMainObject = false)
    {
        if (mapIconSprite != null) mapIconSprite.enabled = wuh;
        hiding = !wuh;
        gameObject.layer = !wuh ? LayerMask.NameToLayer("Ignore Raycast") : LayerMask.NameToLayer("Default");
        gameObject.tag = !wuh ? "Untagged" : "Item";
        SpriteDitherOkayBye(!wuh,fadein,DestroyMainObject);
    }
    public void SpriteDitherOkayBye(bool Hide,bool fadein,bool DestroyMainObject)
    {
        if (HideCoroutin != null) 
        {
            StopCoroutine(HideCoroutin);
            HideCoroutin = null;
        }
        HideCoroutin = StartCoroutine(byeByeDihther(Hide,fadein,DestroyMainObject));
    }
    private IEnumerator byeByeDihther(bool Hide,bool fadein,bool DestroyMainObject)
    {
        floatery = 0;
        
        if (spritContro != null)
        {
            spritContro.useOverlay = false;
            spritContro.blendFactor = 0;
            spritContro.OverlayColor = Color.clear;
            spritContro.useTransparency = true;
            if (fadein) spritContro.cutoffOffset = 1;
            if (Hide)
            {
                while (spritContro.cutoffOffset < 1f)
                {
                    spritContro.cutoffOffset += 3f * Time.deltaTime;
                    yield return null;
                }
                spritContro.cutoffOffset = 1;
                if (DestroyMainObject) Destroy(gameObject);
            }
            else
            {
                while (spritContro.cutoffOffset > 0f)
                {
                    spritContro.cutoffOffset -= 3f  * Time.deltaTime;
                    yield return null;
                }
                spritContro.cutoffOffset = 0;
                if (DestroyMainObject) Destroy(gameObject);
            }
        }
    }
    #endregion

    #region Utility Methods
    private BaseItem GetHeldInstance()
    {
        if (ItemSwapBaseitem != null) return ItemSwapBaseitem;
        else return GetComponentInChildren<BaseItem>();
    }

    public bool SlotStuffs(bool trueOrNot)
    {
        for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
        {
            if (ItemManager.Instance.Inventory[i].ItemID == 0) return trueOrNot;
        }
        Debug.Log($"is le slot full?: {!trueOrNot}");
        return !trueOrNot;
    }
    #endregion
    public void Update()
    {
        if (!hiding)
        {
            InRange = false;
            if (Sych.ScreenCenterRaycast(out RaycastHit hit,KeyFunctions.hi.PlayerClickablesLayer.value))
            {
                Transform hitTransform = hit.transform;
                float maxDistance = 0f;
                
                if (hitTransform.GetComponent<Collider>().gameObject == this.gameObject)
                {
                    maxDistance = GameControllerScript.Instance.player.LocalRange;
                    if (hitTransform.IsWithinDistanceFrom(GameControllerScript.Instance.player.transform, maxDistance))
                    {
                        if (floatery <= 0.5) floatery += 1f* Time.deltaTime;
                        InRange = true;
                        if (spritContro != null)
                        {
                            spritContro.useOverlay = true;
                            spritContro.blendFactor = floatery;
                            spritContro.OverlayColor = Color.white;
                        }
                    }
                }
            }
            if (InRange) return;
            if (floatery > 0) 
            {
                floatery -= 3f * Time.deltaTime;
                if (spritContro != null)
                {
                    spritContro.useOverlay = true;
                    spritContro.blendFactor = floatery;
                    spritContro.OverlayColor = Color.white;
                }
            }
            else if (floatery < 0)
            {
                floatery = 0;
                if (spritContro != null)
                {
                    spritContro.useOverlay = false;
                    spritContro.blendFactor = 0;
                    spritContro.OverlayColor = Color.clear;
                }
                
            }
            
        }
    }

    #region Configuration & State
    [Header("Pickup Settings")]
    public int ID;
    [SerializeField] private bool PresentMode, killafterpickup;
    public bool SpawnAtRandom,instahide;
    public SpriteRenderer mapIconSprite;

    private static Dictionary<int, Sprite> cachedSprites = new Dictionary<int, Sprite>();
    [HideInInspector] public bool DroppedItem;

    private AILocationSelectorScript wanderer;
    private Transform location;
    private SpriteController spritContro;
    private int originalId;
    private Sprite OriginalSprite;
    private MaterialPropertyBlock mpb;
    private bool hiding;
    private BaseItem ItemSwapBaseitem;
    private float floatery;
    public bool InRange;
    [HideInInspector] public Coroutine HideCoroutin;
    
    #endregion
}