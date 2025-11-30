using UnityEngine;

public class ITM_Quarter : BaseItem
{
    public override bool OnUse()
    {
        if (AdditionalGameCustomizer.Instance.ReworkedCurrency)
        {
            AdditionalGameCustomizer.Instance.Cash += 0.25;
            return true;
        }
        if (SendRay("", out RaycastHit Ray, GameControllerScript.Instance.player.LocalRange))
        {
            AudioSource audioDevice = GameControllerScript.Instance.audioDevice;

            if (Ray.collider.CompareTag("VendingMachine"))
            {
                VendingMachineScript vendingMachine = Ray.collider.GetComponent<VendingMachineScript>();
                if (vendingMachine != null)
                {
                    audioDevice.PlayOneShot(aud_Drop);
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
                    audioDevice.PlayOneShot(aud_Drop);
                    return true;
                }
            }
        }
        return false;
    }
    
    [SerializeField] private AudioClip aud_Drop;
}
