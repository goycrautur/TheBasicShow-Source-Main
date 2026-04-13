using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lowBudgetAudioManagementShit : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static lowBudgetAudioManagementShit Instance;
    #endregion
    [Header("Audiosource/manager shit")]
    public AudioManagerLiveReaction HurtSource;
    public AudioManagerLiveReaction TpSource,SchoolMusic,EscapeMusic,EndingMusic,WarMusic,TimeoutMusic,GameOverSource,ChaosAudioSource,MainSource1,MainSource2,MainSource3,DIMCSource,ShuckSource;
    [Header("Audio clip shit")]
    public AudioObjectyeah drivinginmy;
    public AudioObjectyeah quarterDrop,evilLeafTP,TeleporterTp,totem,deltaruneExplud,punchSound,agonyScream,gasterSfx,deadbel,hangAudio,zerullGameover; //part 1
    public AudioObjectyeah ItemCollect,MoneyCollect,gambling,loudIncorrectBuzz;
    public AudioObjectyeah[] HurtSounds,LoseSounds;
    [Header("me when escape music n shit")]
    public AudioObjectyeah SchoolhouseEscape;
    public AudioObjectyeah schoolClip,WAR,timeoutMusicAud,meowlLmsCra,shithourIntro,shithourLoop,TaldiEscape;
    public AudioObjectyeah ChaosStart,ChaosStartLoop,ChaosBuildUp,ChaosFinal;
    public AudioObjectyeah[] NormalTbsFinale,NormalTbsFinaleIntros,EvapV2Finale,EvapV2FinaleIntros;
}
