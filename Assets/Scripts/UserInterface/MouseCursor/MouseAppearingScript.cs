using UnityEngine;

public class MouseAppearingScript : MonoBehaviour
{
    private void Update()
    {
        DisablesnShit();
        if (Sych.ScreenCenterRaycast(out RaycastHit hit,KeyFunctions.hi.PlayerClickablesLayer.value))
        {
            Transform hitTransform = hit.transform;
            float maxDistance = 0f;
            basicshowWindowScript w = hit.collider.GetComponent<basicshowWindowScript>();
            VendingMachineScript vent = hit.collider.GetComponent<VendingMachineScript>();
            DoorScriptExtender dor = hit.collider.GetComponentInChildren<DoorScriptExtender>();
            if (hitTransform.CompareTag("TapePlayer") | hitTransform.CompareTag("Item") | hitTransform.CompareTag("Notebook") | hitTransform.CompareTag("Phone"))
            {
                maxDistance = gc.player.LocalRange;
                MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
            }
            if (hitTransform.CompareTag("fatmachine") && gc.isHoldingBall)
            {
                maxDistance = gc.player.LocalRange;
                MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                baloo.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
            }
            if (hitTransform.CompareTag("Window") && !w.broken)
            {
                for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
                {
                    if (ItemManager.Instance.Inventory[i].ItemID == 13)
                    {
                        maxDistance = gc.player.LocalRange;
                        killwindow.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                        MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                    }
                }
            }
            if (hitTransform.CompareTag("VendingMachine") || hitTransform.CompareTag("Phone"))
            {
                if (AdditionalGameCustomizer.Instance.ReworkedCurrency)
                {
                    if (AdditionalGameCustomizer.Instance.Cash >= 0.25)
                    {
                        maxDistance = gc.player.LocalRange;
                        coin.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                        MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                    }
                }
                for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
                    {
                        if (ItemManager.Instance.Inventory[i].ItemID == 5)
                        {
                            maxDistance = gc.player.LocalRange;
                            coin.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                            MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                        }
                    }
            }
            if (hitTransform.CompareTag("SwingingDoor"))
            {
                SwingingDoorScript swing = hitTransform.GetComponent<Collider>().gameObject.GetComponent<SwingingDoorScript>();
                if (!swing.bDoorLocked)
                {
                    for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
                    {
                        if (ItemManager.Instance.Inventory[i].ItemID == 2)
                        {
                            maxDistance = gc.player.LocalRange;
                            lockin.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                            MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                        }
                    }
                }
            }
            if (hitTransform.CompareTag("HarderDifficulityFaceLock"))
            {
                for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
                {
                    if (ItemManager.Instance.Inventory[i].ItemID == 35)
                    {
                        maxDistance = gc.player.LocalRange;
                        firInThe.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                        MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
                    }
                }
            }
            
            if (hitTransform.CompareTag("DoorTrigger1") && !dor.DoorScripts.bDoorOpen)
            {
                maxDistance = gc.player.LocalRange;
                MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
            }
            if (hitTransform.CompareTag("DoorTrigger2") && dor.DoorScripts.bDoorOpen)
            {
                maxDistance = gc.player.LocalRange;
                MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
            }
        }
        if (Sych.ScreenCenterRaycast(out RaycastHit miscRay))
        {
            Transform mischitTransform = miscRay.transform;
            float maxDist = 0f;
            if (mischitTransform.GetComponent<ZerullBossScript>() != null)
            {
                
                for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
                {
                    if (ItemManager.Instance.Inventory[i].ItemID == 13)
                    {
                        maxDist = gc.player.LocalRange;
                        killwindow.SetActive(maxDist > 0 && mischitTransform.IsWithinDistanceFrom(playerTransform, maxDist));
                        MouseCursor.SetActive(maxDist > 0 && mischitTransform.IsWithinDistanceFrom(playerTransform, maxDist));
                    }
                }
            }
        }
    }
    private void DisablesnShit()
    {
        MouseCursor.SetActive(false);
        killwindow.SetActive(false);
        baloo.SetActive(false);
        lockin.SetActive(false);
        coin.SetActive(false);
        firInThe.SetActive(false);
    }


    [Header("References")]
    [SerializeField] private GameObject MouseCursor;
    [SerializeField] private GameObject killwindow, lockin,baloo, coin, firInThe;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private Transform playerTransform;

}

