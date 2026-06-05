using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
public class MixerSoundOptionIndex : MonoBehaviour
{
    
    private void Start()
    {
        if (GetComponent<Slider>() != null) OptionType = OptionType.Slider;
        else if (GetComponent<Toggle>() != null) OptionType = OptionType.Toggle;
        else if (GetComponent<TMP_Dropdown>() != null) OptionType = OptionType.Dropdown;

        switch (OptionType)
        {
            case OptionType.Toggle:
                /*Toggle toggle = GetComponent<Toggle>();
                toggle.isOn = PlayerPrefsExtension.GetBool(OptionName);
                toggle.onValueChanged.AddListener(ChangeOption);*/
                break;

            case OptionType.Slider:
                Slider slider = GetComponent<Slider>();
                if (PlayerPrefs.GetFloat(OptionName, -999) == -999)
                {
                    PlayerPrefs.SetFloat(OptionName, slider.value);
                }
                slider.value = PlayerPrefs.GetFloat(OptionName);
                slider.onValueChanged.AddListener(ChangeOption);
                break;

            case OptionType.Dropdown:
                /*TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
                int savedValue = PlayerPrefs.GetInt(OptionName, dropdown.value);
                dropdown.value = savedValue;
                dropdown.RefreshShownValue();
                dropdown.onValueChanged.AddListener(ChangeOption);*/
                break;
        }
    }

    public void ChangeOption(float value)
    {
        Singleton<Options>.Instance.SetMixerVolume(value,MixerVolumeNumber,AudioMixerParameter,AudioMixerBase);

        PlayerPrefs.SetFloat(OptionName, value);
        PlayerPrefs.Save();
    }
    [SerializeField] private string OptionName;
    private OptionType OptionType;
    public AudioMixerGroup AudioMixerBase;
    public string AudioMixerParameter;
    public int MixerVolumeNumber;
    
}
