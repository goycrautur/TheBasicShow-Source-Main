using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ModifierType
{
    Toggle,
    Slider,
    Dropdown
}

public class modifierManagerThing : MonoBehaviour
{
    private void Start()
    {
        if (GetComponent<Slider>() != null)
        {
            ModifierType = ModifierType.Slider;
        }
        else if (GetComponent<Toggle>() != null)
        {
            ModifierType = ModifierType.Toggle;
        }
        switch (ModifierType)
        {
            case ModifierType.Toggle:
                Toggle toggle = GetComponent<Toggle>();
                toggle.isOn = PlayerPrefsExtension.GetBool(ModifierName);
                toggle.onValueChanged.AddListener(ModifierUpdateShit);
                break;

            case ModifierType.Slider:
                Slider slider = GetComponent<Slider>();
                if (PlayerPrefs.GetFloat(ModifierName, -999) == -999)
                {
                    PlayerPrefs.SetFloat(ModifierName, slider.value);
                }
                slider.value = PlayerPrefs.GetFloat(ModifierName);
                slider.onValueChanged.AddListener(ModifierUpdateShit);
                break;
        }
    }

    public void ModifierUpdateShit(bool value)
    {
        PlayerPrefsExtension.SetBool(ModifierName, value);
    }

    public void ModifierUpdateShit(float value)
    {
        PlayerPrefs.SetFloat(ModifierName, value);
        PlayerPrefs.Save();
    }

    [SerializeField] private string ModifierName;
    private ModifierType ModifierType;
}