using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mostSillyTrigger : MonoBehaviour
{
    public void Start()
    {
        Cooldown = -1f;
        ItemGivingLimit = BaseItemGivingLimit;
    }
    public void Update()
    {
        if (Cooldown > -1f) Cooldown -= Time.deltaTime;
        RaycastBullshit();
    }
    public void RaycastBullshit()
    {
        if ((Input.GetMouseButtonDown(0) | Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact)) && collid.ScreenRaycastMatchesCollider(out _, GameControllerScript.Instance.player.LocalRange,KeyFunctions.hi.PlayerClickablesLayer.value) && Time.timeScale != 0f)
        {
            raycastBullshitPartTwo();
        }
    }
    public void raycastBullshitPartTwo()
    {
        if (ItemGivingLimit <= 0) return;
        if (Cooldown < 0f)
        {
            Cooldown = BaseCooldown;
            ItemManager.Instance.CollectItem(ItemId);
            GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(GameControllerScript.Instance.lbams.ItemCollect);
            ItemGivingLimit--;
            return;
        }
    }
    public BoxCollider collid;
    public float Cooldown,BaseCooldown;
    public int ItemId,ItemGivingLimit,BaseItemGivingLimit;
}
