using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pitManager : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static pitManager Instance;
    #endregion
    public void Start()
    {
        InitializeSomeShitsIDK();
    }
    public void InitializeSomeShitsIDK()
    {
        string mode = PlayerPrefs.GetString("CurrentMode");
        if (mode == "thePit")
        {
            for (int i = 0; i < HudElementToHide.Length; ++i) HudElementToHide[i].SetActive(false);
            GameControllerScript.Instance.player.transform.position = TpPoint.transform.position;
        }

    }
    public GameObject[] HudElementToHide;
    public GameObject TpPoint;
    public AudioManagerLiveReaction PitAudSourc;
    public AudioObjectyeah Pitpeaksound;
}
