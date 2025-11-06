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
                GameControllerScript.Instance.player.ResetGuilt("bully", 1f);
            }
        }

        if (GameControllerScript.Instance.player.jumpRope)
        {
            GameControllerScript.Instance.player.DeactivateJumpRope();
            GameControllerScript.Instance.audioDevice.PlayOneShot(punc);
            GameControllerScript.Instance.playtimeScript.Disappoint();
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
            if (w != null && !w.broken)
            {
                GameControllerScript.Instance.audioDevice.PlayOneShot(punc);
                w.Window(true, true, 6f);
                GameControllerScript.Instance.player.ResetGuilt("destroyingproperty", 1f);
                return true;
            }
        }
        if (SendRay("", out RaycastHit RayUll, GameControllerScript.Instance.player.LocalRange))
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
        return false;
    }
    [SerializeField] protected AudioClip punc;
}
