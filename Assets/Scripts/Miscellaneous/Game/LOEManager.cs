using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LOEManager : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() 
    {
        EscapePortal.SetActive(false);
        activated = false;
        Instance = this;
    }
    public static LOEManager Instance;
    #endregion
    public void Activate(float timeammou)
    {
        TimeCountdown = timeammou;
        countdou = true;
        activated = true;
        LOEMoment.Invoke();
    }
    public void EnableEscapePortal()
    {
        EscapePortal.SetActive(true);
        PortalLightSource.enabled = false;
        PortalAudioMan.enabled = false;
        PortalCollider.enabled = false;
        PortalSprite.color = InactiveColor;
        PortalTextThing.enabled = true;
        PortalTextThing.text = $"Get All {GameControllerScript.Instance.maxExits-1} exits to get out";
    }

    public void EscapePortalActive()
    {
        PortalCollider.enabled = true;
        PortalLightSource.enabled = true;
        PortalAudioMan.enabled = true;
        PortalSprite.color = ActiveColor;
        PortalTextThing.enabled = false;
    }
    public void PortalEntering()
    {
        countdou = false;
        EscapePortal.SetActive(false);
        GameControllerScript.Instance.lbams.MainSource1.ClearQueue(true);
        GameControllerScript.Instance.lbams.MainSource2.ClearQueue(true);
        GameControllerScript.Instance.lbams.MainSource3.ClearQueue(true);
        
        GameControllerScript.Instance.lbams.ChaosAudioSource.ClearQueue(true);
        ZerullClassic.Instance.yourflashbang.Rebind();
        ZerullClassic.Instance.yourflashbang.Play("flashAnim", -1, 0f);
        EndingManager.Instance.black.SetActive(true);
        GameControllerScript.Instance.MainHudFade.Rebind();
        GameControllerScript.Instance.MainHudFade.Play("hudFadeOutNearly", -1, 0f);
        GameControllerScript.Instance.npcCloneList.ForEach(o => o.SetActive(false));
        GameControllerScript.Instance.player.titlecard = true;
        GameControllerScript.Instance.player.movementLocked = true;
        GameControllerScript.Instance.playerCollider.enabled = false;
        StartCoroutine(wait(1f));
        
    }
    public IEnumerator wait(float Duration)
    {
        GameControllerScript.Instance.lbams.EscapeMusic.ClearQueue(true);
        yield return new WaitForSeconds(Duration);
        EndingManager.Instance.black.SetActive(false);
        EndingManager.Instance.LoadNormalResults(true);
    }

    public void Update()
    {
        if (TimeCountdown < 0f) 
        {
            TimeCountdown = 1f;
            countdou = false;
            for (int i = 0; i < AdditionalGameCustomizer.Instance.ExitImages.Length; ++i) GameControllerScript.Instance.ExitReached(i);
            Singleton<TimeOutManagerFUCKYEA>.Instance.InitializeTimeoutStuff(0.3f);
            GameControllerScript.Instance.ElevdorRea.ForEach(ed => ed.Close());
            GameControllerScript.Instance.ElevdorRea.ForEach(ed => ed.finaleActivated = false);
            GameControllerScript.Instance.Gatesrea.ForEach(g => g.Down(false));
            return;
        }
        if (countdou) TimeCountdown -= Time.deltaTime; 
        int num = Mathf.FloorToInt(TimeCountdown / 60f);
        int num2 = Mathf.FloorToInt(TimeCountdown % 60f);
        int num1 = Mathf.FloorToInt(BaseDurationTillExitRaise / 60f);
        int num12 = Mathf.FloorToInt(BaseDurationTillExitRaise % 60f);
        
        if (activated)
        {
            Timar.text = $"Time Left: {string.Format("{0:00}:{1:00}", num, num2)}";
            if (TimeCountdown > BaseDurationTillExitRaise) 
            {
                WhenWillExitOpen.text = $"Exits are gona be open in... {string.Format("{0:00}:{1:00}", num1, num12)}!";
            }
            else 
            {

                //WhenWillExitOpen.text = "<#00FF00>Exit has been opened!!!!";
                WhenWillExitOpen.text = "";
                if (!DidItRaise)
                {
                    GameControllerScript.Instance.ManualRaiseExit(true, 0.1f);
                    for (int i = 0; i < AdditionalGameCustomizer.Instance.ExitImages.Length; ++i) StartCoroutine(GameControllerScript.Instance.tweeniconSolo(Color.black, 0, 1, 1f, i));
                    GameControllerScript.Instance.maxExits++;
                    EnableEscapePortal();
                    DidItRaise = true;
                    return;
                }
            }
        }
    }
    public TextMeshProUGUI Timar,WhenWillExitOpen;
    public bool countdou,activated,DidItRaise;
    public UltEvents.UltEvent LOEMoment;
    public float TimeCountdown,BaseDurationTillExitRaise;
    [Header("portal that escapes thing")]
    public GameObject EscapePortal;
    public Color InactiveColor,ActiveColor;
    public SpriteRenderer PortalSprite;
    public VoxelLightSource PortalLightSource;
    public TMP_Text PortalTextThing;
    public CapsuleCollider PortalCollider;
    public AudioManagerLiveReaction PortalAudioMan;
}
