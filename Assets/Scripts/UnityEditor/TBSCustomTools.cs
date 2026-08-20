#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;
public class TBSCustomTools : MonoBehaviour
{
    //bleh
    [MenuItem("Basic show utilities/File path shits/Open savefile path")]
    static void saveopenHah()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Save files yes");
        Sych.OpenFolderShenanigans(folderPath);
    }
    [MenuItem("Basic show utilities/File path shits/Open localization file path")]
    static void localopenHah()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Yuri Localization Stuff");
        Sych.OpenFolderShenanigans(folderPath);
    }
    [MenuItem("Basic show utilities/File path shits/Open main directory file path", false , 1)]
    static void mainopenHah()
    {
        string folderPath = Application.persistentDataPath;
        Sych.OpenFolderShenanigans(folderPath);
    }
    //go my gamemodes
    [MenuItem("Basic show utilities/Set Gamemodes (do this before you play in the editor you bum,will probally be removed when savefiles are properly coded)/Story Mode",false, 1)]
    static void setModeStory()
    {
        PlayerPrefs.SetString("CurrentMode", "story");
        Debug.Log($"updated the gamemode to: {PlayerPrefs.GetString("CurrentMode", "null")}");
        PlayerPrefs.Save();
    }
    [MenuItem("Basic show utilities/Set Gamemodes (do this before you play in the editor you bum,will probally be removed when savefiles are properly coded)/Famished Mode",false, 2)]
    static void setModefam()
    {
        PlayerPrefs.SetString("CurrentMode", "famished");
        Debug.Log($"updated the gamemode to: {PlayerPrefs.GetString("CurrentMode", "null")}");
        PlayerPrefs.Save();
    }
    [MenuItem("Basic show utilities/Set Gamemodes (do this before you play in the editor you bum,will probally be removed when savefiles are properly coded)/Zerull Mode",false, 3)]
    static void setModezer()
    {
        PlayerPrefs.SetString("CurrentMode", "zerullclassic");
        Debug.Log($"updated the gamemode to: {PlayerPrefs.GetString("CurrentMode", "null")}");
        PlayerPrefs.Save();
    }
    [MenuItem("Basic show utilities/Set Gamemodes (do this before you play in the editor you bum,will probally be removed when savefiles are properly coded)/Lapping Of Asylum",false, 4)]
    static void setModelap()
    {
        PlayerPrefs.SetString("CurrentMode", "LappingOfAsylum");
        Debug.Log($"updated the gamemode to: {PlayerPrefs.GetString("CurrentMode", "null")}");
        PlayerPrefs.Save();
    }
    [MenuItem("Basic show utilities/Set Gamemodes (do this before you play in the editor you bum,will probally be removed when savefiles are properly coded)/wega challenge",false, 5)]
    static void setModewega()
    {
        PlayerPrefs.SetString("CurrentMode", "wegaChallenge");
        Debug.Log($"updated the gamemode to: {PlayerPrefs.GetString("CurrentMode", "null")}");
        PlayerPrefs.Save();
    }
    [MenuItem("Basic show utilities/Set Gamemodes (do this before you play in the editor you bum,will probally be removed when savefiles are properly coded)/minus b",false, 6)]
    static void setModenumberslop()
    {
        PlayerPrefs.SetString("CurrentMode", "minusb");
        Debug.Log($"updated the gamemode to: {PlayerPrefs.GetString("CurrentMode", "null")}");
        PlayerPrefs.Save();
    }
}
