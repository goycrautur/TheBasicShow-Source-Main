using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_oversizedMegaphone : BaseItem
{
    public override bool OnUse()
    {
        if (used) return false;
        AdditionalGameCustomizer.Instance.FovAmmount -= FovMinus;
        itemSoundSource.PlaySingleClip(inhal);
        used = true;
        StartCoroutine(amwaitin(inhal.audClip.length+InhaleDelay));
        return true;
    }
    public IEnumerator fah(float downDuration)
    {
        float time = downDuration;
        itemSoundSource.PlaySingleClip(screa);
        GameControllerScript.Instance.player.breakwind(true,windoBreakRadius);
        StartCoroutine(stunBlast());
        while (time > 0f)
        {
            time -= Time.deltaTime;
            AdditionalGameCustomizer.Instance.FovAmmount = AdditionalGameCustomizer.Instance.DefaultFovAmmount + Random.Range(FovMin,FovMax);
            yield return null;
        }
        AdditionalGameCustomizer.Instance.FovAmmount = AdditionalGameCustomizer.Instance.DefaultFovAmmount;
        used = false;
        yield break;
    }
    public IEnumerator stunBlast()
    {
        foreach (NPC ennPeeCee in FindObjectsOfType<NPC>())
		{
			if (ennPeeCee != null)
			{
				if (Vector3.Distance(GameControllerScript.Instance.player.transform.position, ennPeeCee.transform.position) <= stunRadius)
				{
					ennPeeCee.Stun(stuntime);
				}
			}
		}
        
        yield return new WaitForSeconds(0.1f);
        foreach (NPC ennPeeCee in FindObjectsOfType<NPC>())
		{
			if (ennPeeCee != null)
			{
				if (Vector3.Distance(GameControllerScript.Instance.player.transform.position, ennPeeCee.transform.position) <= stunRadius)
				{
					ennPeeCee.Stun(stuntime);
				}
			}
		}
        yield break;
    }
    private IEnumerator amwaitin(float time)
    {
        yield return null;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(fah(screa.audClip.length));
        yield break;
    }
    [SerializeField] private float InhaleDelay,FovMinus,FovMin,FovMax,windoBreakRadius,stunRadius,stuntime;
    [SerializeField] private AudioObjectyeah inhal,screa;
    [SerializeField] private AudioManagerLiveReaction itemSoundSource;
    [SerializeField] private bool used;
}
