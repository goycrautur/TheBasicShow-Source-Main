using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicShitass : Singleton<MusicShitass>
{
    public IEnumerator basicShowMusicShit(int type)
    {
        if (GameControllerScript.Instance.mode == "story")
        {
            if (type == 0)
            {
		        GameControllerScript.Instance.escapeMusic.clip = GameControllerScript.Instance.NormalTbsFinale[0];
                GameControllerScript.Instance.escapeMusic.loop = true;
                GameControllerScript.Instance.escapeMusic.Play();
                yield return new WaitForSeconds(GameControllerScript.Instance.NormalTbsFinale[0].length);
                GameControllerScript.Instance.ElevdorRea.ForEach(ed => ed.Opendor = true);
                GameControllerScript.Instance.Gatesrea.ForEach(g => g.Down(false));
                
                StartCoroutine(basicShowMusicShit(1));
                yield return new WaitForSeconds(0.1f);
                GameControllerScript.Instance.finaleMode = true;
            }
            if (type <= 4 && type > 0)
            {
		        GameControllerScript.Instance.escapeMusic.clip = GameControllerScript.Instance.NormalTbsFinale[type];
                GameControllerScript.Instance.escapeMusic.loop = true;
                GameControllerScript.Instance.escapeMusic.Play();
            }
            if (type == 5)
            {
                GameControllerScript.Instance.escapeMusic.clip = GameControllerScript.Instance.NormalTbsFinale[5];
                GameControllerScript.Instance.escapeMusic.Play();
                yield return new WaitForSeconds(GameControllerScript.Instance.NormalTbsFinale[5].length);
		        GameControllerScript.Instance.escapeMusic.clip = GameControllerScript.Instance.NormalTbsFinale[6];
                GameControllerScript.Instance.escapeMusic.loop = true;
                GameControllerScript.Instance.escapeMusic.Play();
            }
        }
    }
    public IEnumerator truerfinale(int type)
    {
        if (GameControllerScript.Instance.mode == "story")
        {
            if (type <= 2)
            {
                GameControllerScript.Instance.EvapV2FinaleSounSource[type].clip = GameControllerScript.Instance.EvapV2FinaleTypeShit[type];
                GameControllerScript.Instance.EvapV2FinaleSounSource[type].Play();
	            yield return new WaitForSeconds(GameControllerScript.Instance.EvapV2FinaleTypeShit[type].length);
		        GameControllerScript.Instance.EvapV2FinaleSounSource[type].clip = GameControllerScript.Instance.EvapV2FinaleTypeShit[5 + type];
                GameControllerScript.Instance.EvapV2FinaleSounSource[type].loop = true;
                GameControllerScript.Instance.EvapV2FinaleSounSource[type].Play();
            }
            if (type == 3)
            {
                GameControllerScript.Instance.EvapV2FinaleSounSource[3].clip = GameControllerScript.Instance.EvapV2FinaleTypeShit[8];
                GameControllerScript.Instance.EvapV2FinaleSounSource[3].loop = true;
                GameControllerScript.Instance.EvapV2FinaleSounSource[3].Play();
                yield return null;
            }
            if (type == 4)
            {
                GameControllerScript.Instance.EvapV2FinaleSounSource[4].clip = GameControllerScript.Instance.EvapV2FinaleTypeShit[3];
                GameControllerScript.Instance.EvapV2FinaleSounSource[4].Play();
                yield return new WaitForSeconds(GameControllerScript.Instance.EvapV2FinaleTypeShit[3].length);
		        GameControllerScript.Instance.EvapV2FinaleSounSource[4].clip = GameControllerScript.Instance.EvapV2FinaleTypeShit[5 + type];
                GameControllerScript.Instance.EvapV2FinaleSounSource[4].loop = true;
                GameControllerScript.Instance.EvapV2FinaleSounSource[4].Play();
            }
            if (type == 5)
            {
                GameControllerScript.Instance.EvapV2FinaleSounSource[5].clip = GameControllerScript.Instance.EvapV2FinaleTypeShit[4];
                GameControllerScript.Instance.EvapV2FinaleSounSource[5].Play();
                yield return new WaitForSeconds(GameControllerScript.Instance.EvapV2FinaleTypeShit[4].length);
		        GameControllerScript.Instance.EvapV2FinaleSounSource[5].clip = GameControllerScript.Instance.EvapV2FinaleTypeShit[10];
                GameControllerScript.Instance.EvapV2FinaleSounSource[5].loop = true;
                GameControllerScript.Instance.EvapV2FinaleSounSource[5].Play();
            }
        }
    }
    public void KillCorou()
    {
        StopAllCoroutines();
    }
}
