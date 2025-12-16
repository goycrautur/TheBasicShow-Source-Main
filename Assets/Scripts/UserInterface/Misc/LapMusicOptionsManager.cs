using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class LapMusicOptionsManager : MonoBehaviour
{
    public void Update()
    {
        for (int i = 0; i < LapTracksText.Length; i++)
        {
            DirectoryInfo gimmeYorName = new DirectoryInfo(PlayerPrefs.GetString($"Lap {i+1} Music Alt", "Default"));
            LapTracksText[i].text = $"Lap {i+1} Music - " + gimmeYorName.Name;
        }
    }

    public void OpenStupidAssMenu(int whatlap)
    {
        foreach (Transform child in LapTrackSelectParent)
        {
            Destroy(child.gameObject);
        }
        LapMusicSelectText.text = $"lap {whatlap} track selector";
        LapMusicSelectHintText.text = $"Put your ogg files under 'StreamingAssets/lapmusic/lap{whatlap}' to play them here";
        //add a default option so u can switch back whenever
        var defaul = Instantiate(trackSelectPrefab, LapTrackSelectParent);
        defaul.GetComponentInChildren<TMP_Text>().text = "Default";
        var defaulButon = defaul.GetComponent<Button>();
        defaulButon.onClick.AddListener(() => { ChangeMusic("Default","Default",whatlap); LapTrackSelectorMenu.SetActive(false); OptionsMain.SetActive(true); });

        foreach (var file in Directory.GetFiles(Application.streamingAssetsPath + $"/lapMusic/lap{whatlap}/"))
        {
            if(file.EndsWith("ogg"))
            {
                var select = Instantiate(trackSelectPrefab, LapTrackSelectParent);
                DirectoryInfo gimmeYorName = new DirectoryInfo(file);
                select.GetComponentInChildren<TMP_Text>().text = gimmeYorName.Name;
                var button = select.GetComponent<Button>();
                button.onClick.AddListener(() => { ChangeMusic(file,gimmeYorName.Name,whatlap); LapTrackSelectorMenu.SetActive(false); OptionsMain.SetActive(true); });
            }
        }
    }

    public void ChangeMusic(string path,string naem,int lapnumber)
    {
        PlayerPrefs.SetString($"Lap {lapnumber} Music", path);
        PlayerPrefs.SetString($"Lap {lapnumber} Music Alt", naem);
        PlayerPrefs.Save();
    }
    public TMP_Text[] LapTracksText;
    public TMP_Text LapMusicSelectText,LapMusicSelectHintText;
    public Transform LapTrackSelectParent;
    public GameObject trackSelectPrefab,LapTrackSelectorMenu,OptionsMain;
}
