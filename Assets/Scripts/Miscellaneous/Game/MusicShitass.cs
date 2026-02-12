using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

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
            if (type <= 2 && type > 0)
            {
		        GameControllerScript.Instance.escapeMusic.clip = GameControllerScript.Instance.NormalTbsFinale[type];
                GameControllerScript.Instance.escapeMusic.loop = true;
                GameControllerScript.Instance.escapeMusic.Play();
            }
            if (type == 3)
            {
                GameControllerScript.Instance.VideoFade.Rebind();
                GameControllerScript.Instance.VideoFade.Play("VidFadein", -1, 0f);
                GameControllerScript.Instance.MainHudFade.Rebind();
                GameControllerScript.Instance.MainHudFade.Play("hudFadeOutNearly", -1, 0f);
                GameControllerScript.Instance.RainbowHudFade.Rebind();
                GameControllerScript.Instance.RainbowHudFade.Play("hudFadeOutRainb", -1, 0f);
                GameControllerScript.Instance.SubtitlesHudFade.Rebind();
                GameControllerScript.Instance.SubtitlesHudFade.Play("hudFadeOutsubs", -1, 0f);
                GameControllerScript.Instance.youCantPause = true;
                GameControllerScript.Instance.vidplay.enabled = true;
                AudioListener.pause = true;
                GameControllerScript.Instance.thatRawImageThatIHate.enabled = true;
                GameControllerScript.Instance.vidplay.Play();
                GameControllerScript.Instance.vidplay.loopPointReached += isvidfinished;
                Time.timeScale = 0;
		        //GameControllerScript.Instance.escapeMusic.clip = GameControllerScript.Instance.NormalTbsFinale[type];
                //GameControllerScript.Instance.escapeMusic.loop = true;
                //GameControllerScript.Instance.escapeMusic.Play();
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
    private void isvidfinished(VideoPlayer vp)
	{
        Time.timeScale = 1;
        GameControllerScript.Instance.MainHudFade.Rebind();
        GameControllerScript.Instance.MainHudFade.Play("hudFadeIn", -1, 0f);
        GameControllerScript.Instance.RainbowHudFade.Rebind();
        GameControllerScript.Instance.RainbowHudFade.Play("hudFadeInRainb", -1, 0f);
        GameControllerScript.Instance.SubtitlesHudFade.Rebind();
        GameControllerScript.Instance.SubtitlesHudFade.Play("hudFadeInsubs", -1, 0f);
        ZerullClassic.Instance.yourflashbang.Rebind();
        ZerullClassic.Instance.yourflashbang.Play("flashAnim", -1, 0f);
		GameControllerScript.Instance.escapeMusic.clip = GameControllerScript.Instance.NormalTbsFinale[4];
        GameControllerScript.Instance.escapeMusic.loop = true;
        GameControllerScript.Instance.escapeMusic.Play();
        GameControllerScript.Instance.vidplay.enabled = false;
        GameControllerScript.Instance.thatRawImageThatIHate.enabled = false;
        GameControllerScript.Instance.youCantPause = false;
        AudioListener.pause = false;
	}
    public IEnumerator truerfinale(int type)
    {
        if (GameControllerScript.Instance.mode == "story")
        {
            if (GameControllerScript.Instance.ExclusiveRoute == "")
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
            if (GameControllerScript.Instance.ExclusiveRoute == "ClassicPlayerSecretEndChal")
            {
                if (type <= 2)
                {
                    GameControllerScript.Instance.EvapV2FinaleSounSource[type].clip = lowBudgetAudioManagementShit.Instance.ThroughTheFireAndFlamesClipIntro[type];
                    GameControllerScript.Instance.EvapV2FinaleSounSource[type].Play();
                    yield return new WaitForSeconds(lowBudgetAudioManagementShit.Instance.ThroughTheFireAndFlamesClipIntro[type].length);
                    GameControllerScript.Instance.EvapV2FinaleSounSource[type].clip = lowBudgetAudioManagementShit.Instance.ThroughTheFireAndFlamesClipLoop[type];
                    GameControllerScript.Instance.EvapV2FinaleSounSource[type].loop = true;
                    GameControllerScript.Instance.EvapV2FinaleSounSource[type].Play();
                }
                if (type == 3)
                {
                    GameControllerScript.Instance.EvapV2FinaleSounSource[3].clip = lowBudgetAudioManagementShit.Instance.ThroughTheFireAndFlamesClipLoop[2];
                    GameControllerScript.Instance.EvapV2FinaleSounSource[3].loop = true;
                    GameControllerScript.Instance.EvapV2FinaleSounSource[3].Play();
                }
                if (type == 4)
                {
                    GameControllerScript.Instance.EvapV2FinaleSounSource[4].clip = lowBudgetAudioManagementShit.Instance.ThroughTheFireAndFlamesClipLoop[3];
                    GameControllerScript.Instance.EvapV2FinaleSounSource[4].loop = true;
                    GameControllerScript.Instance.EvapV2FinaleSounSource[4].Play();
                }
                if (type == 5)
                {
                    GameControllerScript.Instance.EvapV2FinaleSounSource[5].clip = lowBudgetAudioManagementShit.Instance.ThroughTheFireAndFlamesClipIntro[2];
                    GameControllerScript.Instance.EvapV2FinaleSounSource[5].Play();
                    yield return new WaitForSeconds(lowBudgetAudioManagementShit.Instance.ThroughTheFireAndFlamesClipIntro[2].length);
                    GameControllerScript.Instance.EvapV2FinaleSounSource[5].clip = lowBudgetAudioManagementShit.Instance.ThroughTheFireAndFlamesClipLoop[4];
                    GameControllerScript.Instance.EvapV2FinaleSounSource[5].loop = true;
                    GameControllerScript.Instance.EvapV2FinaleSounSource[5].Play();
                }
            }
        }
    }
    public void KillCorou()
    {
        StopAllCoroutines();
    }
}
