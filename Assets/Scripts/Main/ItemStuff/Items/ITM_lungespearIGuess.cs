using System.Collections;
using UnityEngine;

public class ITM_lungespearIGuess : BaseItem
{
    public override bool OnUse()
    {
        PlayerScript player = GameControllerScript.Instance.player;
        if (used) return false;
        else if (player.stamina < StaminaNeededValue) return false;
        
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Remove, StaminaDrainValue);
        Vector3 pos = !Singleton<InputManager>.Instance.GetActionKey(InputAction.LookBehind) ? player.transform.forward : -player.transform.forward;
        player.PushPlayer(pos, LungeDistance * (1+(player.playerSpeed/100)), 0.5f,false);
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(Used);
        used = true;
        StartCoroutine(lungeStuff(0.5f));
        StartCoroutine(CamCorou(0.6f));
        StartCoroutine(amwaitin(coohdown));
        return true;
    }

    public override void OnSelect()
    {
        base.OnSelect();
        if (IsJarona && GameControllerScript.Instance.mode == "story")
        {
            FadeAudioRea(true,JaronaAudio,2);
            JaronaAudio.ClearQueue(true);
            JaronaAudio.QueueAudio(flowerMan);
            JaronaAudio.SetLoop(true);
        }
    }
    public override void OnDeselect()
    {
        if (IsJarona) FadeAudioRea(false,JaronaAudio,2);
    }
    public void FadeAudioRea(bool FadeOut,AudioManagerLiveReaction FadeSource,float duration,bool UnscaledDeltatime = false) 
    {
        if (JaronaCoroutine != null) 
        {
            StopCoroutine(JaronaCoroutine);
		    FadeSource.SetVolume(1);
        }
        if (!FadeOut) 
        {
            Debug.Log("it fade in");
            JaronaCoroutine = StartCoroutine(FadeAudioIn(FadeSource,duration,UnscaledDeltatime));
        }
        else 
        {
            Debug.Log("it fade out");
            JaronaCoroutine = StartCoroutine(FadeAudioOut(FadeSource,duration,UnscaledDeltatime));
        }
    }
    private IEnumerator FadeAudioIn(AudioManagerLiveReaction FadeSource,float duration,bool UnscaledDeltatime = false) 
	{
		float elapsed = 0f;
        float dura = duration;
		while (elapsed < dura)
		{
			elapsed += !UnscaledDeltatime ? Time.deltaTime :Time.unscaledDeltaTime;
			float t = elapsed / dura;
			FadeSource.SetVolume(Mathf.Lerp(1, 0f, t));
			yield return null;
		}
        FadeSource.ClearQueue(true);
		FadeSource.SetVolume(1);  //volumen
        yield break;

	}
    private IEnumerator FadeAudioOut(AudioManagerLiveReaction FadeSource,float duration,bool UnscaledDeltatime = false) 
	{
		float elapsed = 0f;
        float dura = duration;
		while (elapsed < dura)
		{
			elapsed += !UnscaledDeltatime ? Time.deltaTime :Time.unscaledDeltaTime;
			float t = elapsed / dura;
			FadeSource.SetVolume(Mathf.Lerp(0, 1f, t));
			yield return null;
		}
		FadeSource.SetVolume(1);  //volumen
        yield break;

	}

    private IEnumerator CamCorou(float time)
    {
        while (time > 0f)
        {
            CameraScript.Instance.TempJumpNum = !IsJarona? time*6 : time*10;
            time -= Time.deltaTime;
            yield return null;
        }
        CameraScript.Instance.TempJumpNum = 0;
        yield break;
    }
    private IEnumerator lungeStuff(float time)
    {
        PlayerScript player = GameControllerScript.Instance.player;
        Vector3 pos = !Singleton<InputManager>.Instance.GetActionKey(InputAction.LookBehind) ? player.transform.forward : -player.transform.forward;
        Vector3 pos2 = !Singleton<InputManager>.Instance.GetActionKey(InputAction.LookBehind) ? -player.transform.forward : player.transform.forward;
        AdditionalGameCustomizer.Instance.FovAmmount += 10;
        AdditionalGameCustomizer.Instance.FovAmmount -= 10;
        while (time > 0f)
        {
            
            player.IsLunging = true;
            foreach (NPC ennPeeCee in FindObjectsOfType<NPC>())
            {
                if (ennPeeCee != null)
                {
                    if (Vector3.Distance(player.transform.position, ennPeeCee.transform.position) <= 7.5f)
                    {
                        if (ennPeeCee.iFrames > 0f || ennPeeCee.fuckingdead) yield break;
                        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(NpcStab);
                        ennPeeCee.DealsDamageIFramesStuff(damageToNpc,1);
                        player.IsLunging = false;
                        if (IsJarona) 
                        {
                            player.PushPlayer(pos2, 96 * (1+(player.playerSpeed/100)), 0.5f);
                            ennPeeCee.PushNpc(ennPeeCee.GetNPCPushDirection(pos),128, 2f);
                        }
                        else
                        {
                            ennPeeCee.Stun(3f);
                            ennPeeCee.PushNpc(ennPeeCee.GetNPCPushDirection(pos),64, 0.75f);
                        }
                    }
                }
            }
            if (IsJarona)
            {
                foreach (SwingingDoorScript swindor in FindObjectsOfType<SwingingDoorScript>())
                {
                    if (swindor != null)
                    {
                        if (Vector3.Distance(player.transform.position, swindor.transform.position) <= 5f)
                        {
                            if (swindor.destroyed) yield break;
                            GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(NpcStab);
                            swindor.PleaseDie();
                            player.IsLunging = false;
                            player.PushPlayer(pos2, 64 * (1+(player.playerSpeed/100)), 0.5f);
                        }
                    }
                }
                foreach (DoorScriptExtender dor in FindObjectsOfType<DoorScriptExtender>())
                {
                    DoorScript dordor = dor.DoorScripts;
                    if (dordor != null)
                    {
                        if (Vector3.Distance(player.transform.position, dordor.transform.position) <= 5f)
                        {
                            if (dordor.destroyed) yield break;
                            GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(NpcStab);
                            dordor.DestroyDoor();
                            player.IsLunging = false;
                            player.PushPlayer(pos2, 64 * (1+(player.playerSpeed/100)), 0.5f);
                        }
                    }
                }
            }
            time -= Time.deltaTime;
            yield return null;
        }
        CameraScript.Instance.TempJumpNum = 0;
        player.IsLunging = false;
        yield break;
    }
    private IEnumerator amwaitin(float time)
    {
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        used = false;
        yield break;
    }
    [SerializeField] private int damageToNpc;

    [SerializeField] private float StaminaDrainValue,StaminaNeededValue,coohdown,LungeDistance;
    [SerializeField] private AudioObjectyeah Used,NpcStab;
    [SerializeField] private bool used;
    [Header("flowuh man")]
    [HideInInspector] public Coroutine JaronaCoroutine;
    [SerializeField] private bool IsJarona;
    [SerializeField] private AudioObjectyeah flowerMan;
    [SerializeField] private AudioManagerLiveReaction JaronaAudio;
}
