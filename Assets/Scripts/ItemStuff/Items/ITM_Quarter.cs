using UnityEngine;

public class ITM_Quarter : BaseItem
{
    public override bool OnUse()
    {
        if (AdditionalGameCustomizer.Instance.ReworkedCurrency)
        {
            GameControllerScript.Instance.lbams.PlayClip(GameControllerScript.Instance.lbams.MainSource2,GameControllerScript.Instance.lbams.MoneyCollect);
            AdditionalGameCustomizer.Instance.Cash += 0.25;
            return true;
        }
        if (SendRay("", out RaycastHit Ray, GameControllerScript.Instance.player.LocalRange))
        {
            if (Ray.collider.CompareTag("VendingMachine"))
            {
                VendingMachineScript vendingMachine = Ray.collider.GetComponent<VendingMachineScript>();
                if (vendingMachine != null)
                {
                    GameControllerScript.Instance.lbams.PlayClip(GameControllerScript.Instance.lbams.MainSource3,aud_Drop);
                    vendingMachine.insertedMoney++;
                    vendingMachine.DispenseItem();
                }
                return true;
            }
            if (Ray.collider.CompareTag("Phone"))
            {
                TapePlayerScript tapePlayer = Ray.collider.GetComponent<TapePlayerScript>();
                if (tapePlayer != null && !tapePlayer.TapeCDEnable)
                {
                    tapePlayer.Play();
                    GameControllerScript.Instance.lbams.PlayClip(GameControllerScript.Instance.lbams.MainSource3,aud_Drop);
                    return true;
                }
            }
        }
        return false;
    }
    
    [SerializeField] private AudioObjectyeah aud_Drop;
}
