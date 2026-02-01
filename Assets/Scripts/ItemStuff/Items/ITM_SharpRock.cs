using UnityEngine;
using System.Collections;

public class ITM_SharpRock : BaseItem
{
    public override bool OnUse()
    {
        if (AdditionalGameCustomizer.Instance.DetentionAfterScissorUse)
        {
            if (!GameControllerScript.Instance.player.outdoorsfr)
		    {
			    if (GameControllerScript.Instance.player.door.lockTime <= 0f)
			    {
			    GameControllerScript.Instance.player.ResetGuilt("bully", 1f);
			    }
		    }
        }

        if (GameControllerScript.Instance.player.jumpropes.Count > 0)
        {
            GameControllerScript.Instance.player.jumpropes[0].End(false);
            GameControllerScript.Instance.audioDevice.PlayOneShot(punc);
            return true;
        }

        if (SendRay("", out RaycastHit Ray, GameControllerScript.Instance.player.LocalRange))
        {
            if (Ray.collider.name == "1st Prize" || Ray.collider.name == "washingmachine")
            {
                GameControllerScript.Instance.firstPrizeScript.GoCrazy();
                GameControllerScript.Instance.audioDevice.PlayOneShot(punc);
                return true;
            }
        }
        if (SendRay("", out RaycastHit Rayham, GameControllerScript.Instance.player.LocalRange))
        {
            WindowScript w = Rayham.collider.GetComponent<WindowScript>();
            if (w != null && !w.broken && w.isActiveAndEnabled)
            {
                GameControllerScript.Instance.audioDevice.PlayOneShot(punc);
                w.Window(true, true, 6f);
                GameControllerScript.Instance.player.ResetGuilt("destroyingproperty", 3f);
                CameraScript.Instance.TempShakeAmount += 0.5f;
                return true;
            }
        }
        if (SendRay("", out RaycastHit Rayham2, GameControllerScript.Instance.player.LocalRange))
        {
            basicshowWindowScript w = Rayham2.collider.GetComponent<basicshowWindowScript>();
            if (w != null && !w.broken)
            {
                GameControllerScript.Instance.audioDevice.PlayOneShot(punc);
                w.SetWindowState(true, 6f, 0f, 1);
                GameControllerScript.Instance.player.ResetGuilt("destroyingproperty", 3f);
                CameraScript.Instance.TempShakeAmount += 0.5f;
                return true;
            }
        }
        if (SendRay("", out RaycastHit RayUll, GameControllerScript.Instance.player.LocalRange))
        {
            if (ZerullClassic.Instance.realBossStarted && ZerullClassic.Instance.health != 1)
            {
                if (RayUll.transform.GetComponent<ZerullBossScript>() != null)
                {
                    StartCoroutine(StunBoss());
                    IEnumerator StunBoss()
                    {
                        while (ZerullClassic.Instance.maxHealth == ZerullClassic.Instance.health-1 && !ZerullClassic.Instance.realBossStarted && ZerullClassic.Instance.GetBoss().hitted || ZerullClassic.Instance.isbroyapping)
                        {
                            yield return null;
                        }
                        ZerullClassic.Instance.OnHit(ZerullClassic.Instance.zs.hit.length);
                        GameControllerScript.Instance.audioDevice.PlayOneShot(punc);
                    }
                    return true;
                }
            }
        }
        return false;
    }
    [SerializeField] protected AudioClip punc;
}
