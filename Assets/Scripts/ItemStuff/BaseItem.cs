using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public void ChangeReferencesTexture(Texture smolsprites)
    {
        SmallSprite = smolsprites;
    }
    #region Virtual Hooks
    public virtual bool OnUse() => true;
    public virtual void OnSelect() { }
    public virtual void OnDeselect() { }
    public virtual void OnPickup() { }

    public virtual BaseItem CreateInstance() => Instantiate(this);
    #endregion

    #region Helpers
    protected bool SendRay(string tag, out RaycastHit rayHit, float range = 10f)
    {
        rayHit = default;

        if (Sych.ScreenCenterRaycast(out RaycastHit hit))
        {
            bool withinRange = hit.transform.IsWithinDistance(range);
            bool tagMatch = string.IsNullOrEmpty(tag) || hit.collider.CompareTag(tag);

            if (withinRange && tagMatch)
            {
                rayHit = hit;
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Serialized Data
    [Tooltip("The name of the item")] public string Name;
    [Tooltip("The item info")] public string ItmInfoText;

    [Tooltip("The color of the item name")] public Color NameColor = Color.black;

    [Header("Sprite"), Tooltip("Sprite of the item used for pickups")] public Texture BigSprite;

    public Texture SmallSprite;
    [Header("ID stuff")]
    [Tooltip("ID of the item.")] public int ItemID;
    [Tooltip("Name ID of the item.")] public string NameID;
    
    [Header("Settings")]
    [Tooltip("How many uses the item has")] public int Uses = 1;
    [Tooltip("How many Stack the item can have")] public int MaxUsesCap = 1;
    [Tooltip("erm")] public int TexturePPUThing = 100;
    [Tooltip("Should the item get specified to be an starred item on the minimap")] public bool SpecialItemIcon;
    [Tooltip("Disable it from getting dropped fear")] public bool undropable;
    [Tooltip("litearlly infinite uses yea")] public bool InfiniteUses;
    #endregion
}