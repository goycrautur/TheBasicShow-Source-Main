using UnityEngine;

public class MouseAppearingScript : MonoBehaviour
{
    private void Update()
    {
        MouseCursor.SetActive(false);
        killwindow.SetActive(false);
        baloo.SetActive(false);
        lockin.SetActive(false);
        coin.SetActive(false);
        if (Sych.ScreenCenterRaycast(out RaycastHit hit))
        {
            Transform hitTransform = hit.transform;
            float maxDistance = 0f;
            WindowScript w = hit.collider.GetComponent<WindowScript>();
            VendingMachineScript vent = hit.collider.GetComponent<VendingMachineScript>();

            if (hitTransform.CompareTag("Door") | hitTransform.CompareTag("TapePlayer") | hitTransform.CompareTag("Item") | hitTransform.CompareTag("Notebook") | hitTransform.CompareTag("Phone"))
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
            if (hitTransform.CompareTag("Window") && !w.broken || hitTransform.GetComponent<ZerullBossScript>() != null)
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
            if (hitTransform.CompareTag("VendingMachine") && !vent.isOutOfGoods || hitTransform.CompareTag("Phone"))
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
    }

    [Header("References")]
    [SerializeField] private GameObject MouseCursor;
    [SerializeField] private GameObject killwindow, lockin,baloo, coin;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private Transform playerTransform;

}