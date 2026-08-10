using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class IntroScreenManager : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static IntroScreenManager Instance;
    #endregion
    #if UNITY_EDITOR
    private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeChanged;

    private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeChanged;

    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        Debug.Log(state);
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            doTheThing(true);
        }
    }
    #endif
    public void doTheThing(bool SavefileSelect)
    {
        
        if (SavefileSelect) SavefileSelecting();
        else
        {
            SaveObjec.SetActive(SavefileSelect);
            MenuObjec.SetActive(!SavefileSelect);
            dihtherMainMenu.speed = 0.5f;
            dihtherSaveSelect.SetTrigger("open");
            DiscordRPC_stuff.current.UpdateStatus("Opening menu", "peak", "van", "the crx");
            audMan.ClearQueue(true);
            audMan.SetLoop(true);
            audMan.QueueAudio(musicMainMenu);
        }
    }
    private void SavefileSelecting()
    {
        SaveObjec.SetActive(true);
        MenuObjec.SetActive(false);
        dihtherSaveSelect.speed = 0.25f;
        dihtherSaveSelect.SetTrigger("open");
        DiscordRPC_stuff.current.UpdateStatus("Savefile select (its a fakeout)", "???", "van", "the crx");
        audMan.ClearQueue(true);
        audMan.SetLoop(true);
        audMan.QueueAudio(musikSaveSelect);
    }


    public Animator dihtherSaveSelect,dihtherMainMenu;
    public AudioManagerLiveReaction audMan;
    public AudioObjectyeah musikSaveSelect,musicMainMenu;
    public GameObject MenuObjec,SaveObjec;
}
