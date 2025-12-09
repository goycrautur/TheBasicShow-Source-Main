using UnityEngine;

public class ITM_BSODA : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript Contoller = GameControllerScript.Instance;
        Instantiate(bsodaSpray, Contoller.player.transform.position, Contoller.cameraTransform.rotation);
        if (!Contoller.player.outdoorsfr)
		{
			if (Contoller.player.door.lockTime <= 0f)
			{
			Contoller.player.ResetGuilt("drink", 1f);
			}
		}
        GameControllerScript.Instance.audioDevice.PlayOneShot(aud_Soda);
        if (SummonSubtitles)
        {
            if (Subtitles != null)
            {
            GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(Subtitles.subtitleOption,Subtitles,0f,new Vector3(0f,-170.5f,0f),GameControllerScript.Instance.audioDevice);
            }
        }
        return true;
    }
    
    [SerializeField] private GameObject bsodaSpray;
    [SerializeField] private AudioClip aud_Soda;
    [SerializeField] private bool SummonSubtitles;
    [SerializeField] private subsScriptableObject Subtitles;
}
