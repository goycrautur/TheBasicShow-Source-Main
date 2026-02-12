using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_oversizedMegaphone : BaseItem
{
    public override bool OnUse()
    {
        if (used) return false;
        ogFov = AdditionalGameCustomizer.Instance.FovAmmount;
        AdditionalGameCustomizer.Instance.FovAmmount -= FovMinus;
        itemSoundSource.PlayOneShot(inhal);
        GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(InhaleSub.subtitleOption,InhaleSub,new Vector3(0f,-170.5f,0f),itemSoundSource);
        used = true;
        StartCoroutine(amwaitin(inhal.length+InhaleDelay));
        return true;
    }
    public IEnumerator fah(float downDuration)
    {
        float time = downDuration;
        itemSoundSource.PlayOneShot(screa);
        GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(ScreaSub.subtitleOption,ScreaSub,new Vector3(0f,-170.5f,0f),itemSoundSource);
        StartCoroutine(windBreakBlast());
        StartCoroutine(stunBlast());
        while (time > 0f)
        {
            time -= Time.deltaTime;
            AdditionalGameCustomizer.Instance.FovAmmount = ogFov + Random.Range(FovMin,FovMax);
            yield return null;
        }
        AdditionalGameCustomizer.Instance.FovAmmount = ogFov;
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
    public IEnumerator windBreakBlast()
    {
        foreach (WindowScript w in FindObjectsOfType<WindowScript>())
		{
			if (!w.broken)
			{
				if (Vector3.Distance(GameControllerScript.Instance.player.transform.position, w.transform.position) <= windoBreakRadius)
				{
					w.Window(true, true, 6f);
				}
			}
		}
        yield return new WaitForSeconds(0.1f);
        foreach (WindowScript w in FindObjectsOfType<WindowScript>())
		{
			if (!w.broken)
			{
				if (Vector3.Distance(GameControllerScript.Instance.player.transform.position, w.transform.position) <= windoBreakRadius)
				{
					w.Window(true, true, 6f);
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
        StartCoroutine(fah(screa.length));
        yield break;
    }
    [SerializeField] private float InhaleDelay,FovMinus,FovMin,FovMax,windoBreakRadius,stunRadius,stuntime;
    private float ogFov;
    [SerializeField] private AudioClip inhal,screa;
    [SerializeField] private AudioSource itemSoundSource;
    [SerializeField] private subsScriptableObject InhaleSub,ScreaSub;
    [SerializeField] private bool used;
}
