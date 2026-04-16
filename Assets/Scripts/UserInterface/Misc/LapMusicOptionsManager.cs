using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class LapMusicOptionsManager : MonoBehaviour
{
    public void Start() => okbro = 0f;
    public void Update()
    {
        for (int i = 0; i < LapTracksText.Length; i++)
        {
            DirectoryInfo gimmeYorName = new DirectoryInfo(PlayerPrefs.GetString($"Lap {i+1} Music Alt", "Default"));
            LapTracksText[i].text = $"Lap {i+1} Music - " + gimmeYorName.Name;
        }
        if (okbro > 0f) okbro -= Time.deltaTime;
        LapTextChangedText.color = new Color(1f,1f,1f,okbro);
    }

    public void OpenStupidAssMenu(int whatlap)
    {
        foreach (Transform child in LapTrackSelectParent)Destroy(child.gameObject);
        LapMusicSelectText.text = $"lap {whatlap} track selector";
        LapMusicSelectHintText.text = $"Put your ogg files under 'StreamingAssets/lapmusic/lap{whatlap}' to play them here";
        //add a default option so u can switch back whenever
        var defaul = Instantiate(trackSelectPrefab, LapTrackSelectParent);
        defaul.GetComponentInChildren<TMP_Text>().text = "Default";
        var defaulButon = defaul.GetComponent<Button>();
        defaulButon.onClick.AddListener(() => { ChangeMusic("Default","Default",whatlap); transis.Transition(); audi.PlaySingleClip(soun); });

        foreach (var file in Directory.GetFiles(Application.streamingAssetsPath + $"/lapMusic/lap{whatlap}/"))
        {
            if(file.EndsWith("ogg"))
            {
                var select = Instantiate(trackSelectPrefab, LapTrackSelectParent);
                DirectoryInfo gimmeYorName = new DirectoryInfo(file);
                select.GetComponentInChildren<TMP_Text>().text = gimmeYorName.Name;
                var button = select.GetComponent<Button>();
                button.onClick.AddListener(() => { ChangeMusic(file,gimmeYorName.Name,whatlap); transis.Transition(); audi.PlaySingleClip(soun); });
            }
        }
    }

    public void ChangeMusic(string path,string naem,int lapnumber)
    {
        PlayerPrefs.SetString($"Lap {lapnumber} Music", path);
        PlayerPrefs.SetString($"Lap {lapnumber} Music Alt", naem);
        tempLapNumbInt = lapnumber;
        PlayerPrefs.Save();
    }
    public void FlashChangedLapText()
    {
        string lapsongtext = PlayerPrefs.GetString($"Lap {tempLapNumbInt} Music Alt", "Default");
        LapTextChangedText.text = $"Changed Lap {tempLapNumbInt} Music to {lapsongtext}!!";
        okbro = 1f;
    }
    public TMP_Text[] LapTracksText;
    public TMP_Text LapMusicSelectText,LapMusicSelectHintText,LapTextChangedText;
    public Transform LapTrackSelectParent;
    public GameObject trackSelectPrefab;
    public TransistionManager transis;
    public AudioManagerLiveReaction audi;
    public AudioObjectyeah soun;
    private int tempLapNumbInt;
    private float okbro;
}
