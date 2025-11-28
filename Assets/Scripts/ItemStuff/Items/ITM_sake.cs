using UnityEngine;

public class ITM_sake : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript Contoller = GameControllerScript.Instance;
        Instantiate(sakeSpray, Contoller.player.transform.position, Contoller.cameraTransform.rotation);
        if (blastcork)
        {
            Instantiate(cork, Contoller.player.transform.position, Contoller.cameraTransform.rotation);
        }

        if (!Contoller.player.outdoorsfr)
		{
			if (Contoller.player.door.lockTime <= 0f)
			{
			Contoller.player.ResetGuilt("drink", 1f);
			}
		}
        GameControllerScript.Instance.audioDevice.PlayOneShot(sake);
        return true;
    }

    [SerializeField] private GameObject sakeSpray, cork;
    [SerializeField] private AudioClip sake;
    [SerializeField] private bool blastcork;
}
