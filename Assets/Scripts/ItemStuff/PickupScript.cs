using UnityEngine;
using System.Collections.Generic;

public class PickupScript : Interactable
{
    #region Initialization Logic
    public void Start()
    {
        mpb = new MaterialPropertyBlock();
        cachedSprites = new Dictionary<int, Sprite>();


        if (PresentMode)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = GameControllerScript.Instance.Present;
            ID = Random.Range(1, 46);
        }

        if (SpawnAtRandom)
        {
            wanderer = FindObjectOfType<AILocationSelectorScript>();

            GameObject Set = GameObject.Find("AI_LocationSelector");
            location = Set.transform;

            location.position = wanderer.SetNewTargetForAgent(null, "present");
            transform.position = location.position + Vector3.up * 4f;
        }
        OriginalSprite = GetComponentInChildren<SpriteRenderer>().sprite;
        originalId = ID;
    }
    public void ItemRespawning()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = OriginalSprite;
        ID = originalId;
    }
    public void itsPresentTime(bool resetIDONLY = false)
    {
        if (resetIDONLY)
        {
            ID = Random.Range(1, 46);
        }
        if (!PresentMode && !resetIDONLY)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = GameControllerScript.Instance.Present;
            ID = Random.Range(1, 46);
            PresentMode = true;
        }
    }
    #endregion
    public void OnEnable()
    {
        if (mapIconSprite != null)
        {
            mapIconSprite.enabled = true;
        }
        hiding = false;
    }
    public void OnDisable()
    {
        if (mapIconSprite != null)
        {
            mapIconSprite.enabled = false;
        }
        hiding = true;
    }


    #region Player Interaction
    public override void Interact()
    {
        GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.aud_ItemCollect);
        if (ID == 5)
        {
            if (ZerullClassic.Instance.realBossStarted)
            {
                ZerullClassic.Instance.objects -= 1;
            }
        }
        if (PresentMode)
        {
            GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.gambling);
        }
        if (AdditionalGameCustomizer.Instance.ReworkedCurrency & ID == 5)
        {
            GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.monesound);
            AdditionalGameCustomizer.Instance.Cash += 0.25;
            if (killafterpickup)
            {
                Destroy(gameObject);
            }
            if (!killafterpickup)
            {
                transform.gameObject.SetActive(false);
                mapIconSprite.enabled = false;
            }
            return;
        }
        else if (SlotStuffs(true))
        {
            if (!DroppedItem)
            {
                if (killafterpickup)
                {
                    Destroy(gameObject);
                }
                if (!killafterpickup)
                {
                    transform.gameObject.SetActive(false);
                    mapIconSprite.enabled = false;
                }
                if (ZerullClassic.Instance.realBossStarted)
                {
                    ZerullClassic.Instance.objects -= 1;
                }
            }
            else
                Destroy(gameObject);

            ItemManager.Instance.CollectItem(ID, GetHeldInstance());
            return;
        }

        int orgID = ID;
        BaseItem orgItem = GetHeldInstance();

        ID = ItemManager.Instance.GetSelectedItem();
        BaseItem newItem = ItemManager.Instance.GetSelectedItemObject();
        newItem.transform.parent = transform;

        if (!cachedSprites.ContainsKey(ID))
        {
            Texture itemTexture = ItemManager.Instance.GetItem(ID).BigSprite;
            Sprite itemSprite = Sprite.Create((Texture2D)itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), newItem.TexturePPUThing);
            cachedSprites.Add(ID, itemSprite);
        }

        GetComponentInChildren<SpriteRenderer>().sprite = cachedSprites[ID];
        gameObject.name = $"Pickup_{ItemManager.Instance.GetItem(ID).Name}";

        if (SlotStuffs(false))
        {
            transform.gameObject.SetActive(true);
            mapIconSprite.enabled = true;
        }

        ItemManager.Instance.CollectItem(orgID, orgItem);
    }
    #endregion

    #region Utility Methods
    private BaseItem GetHeldInstance()
    {
        return GetComponentInChildren<BaseItem>();
    }

    public bool SlotStuffs(bool trueOrNot)
    {
        for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
        {
            if (ItemManager.Instance.Inventory[i].ItemID == 0)
                return trueOrNot;
        }
        return !trueOrNot;
    }
    #endregion
    public void Update()
    {
        if (!hiding)
        {
            GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(mpb);
            mpb.SetFloat("_OutlineSize", 0);
            mpb.SetColor("_OutlineColor", Color.clear);
            GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(mpb);
            if (Sych.ScreenCenterRaycast(out RaycastHit hit))
            {
                Transform hitTransform = hit.transform;
                float maxDistance = 0f;
                if (hitTransform.GetComponent<Collider>().gameObject == this.gameObject)
                {
                    maxDistance = GameControllerScript.Instance.player.LocalRange;
                    if (hitTransform.IsWithinDistanceFrom(GameControllerScript.Instance.player.transform, maxDistance))
                    {
                        GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(mpb);
                        mpb.SetFloat("_OutlineSize", 2);
                        mpb.SetColor("_OutlineColor", Color.white);
                        GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(mpb);
                    }
                }
            }
        }
    }

    #region Configuration & State
    [Header("Pickup Settings")]
    public int ID;
    [SerializeField] private bool PresentMode, killafterpickup;
    public bool SpawnAtRandom;
    public SpriteRenderer mapIconSprite;

    private static Dictionary<int, Sprite> cachedSprites = new Dictionary<int, Sprite>();
    [HideInInspector] public bool DroppedItem;

    private AILocationSelectorScript wanderer;
    private Transform location;
    private int originalId;
    private Sprite OriginalSprite;
    private MaterialPropertyBlock mpb;
    private bool hiding;
    #endregion
}