using TMPro;
using UnityEngine;

namespace YuriArchive.GlobalLocalization
{
    [ExecuteAlways]
    public class LocalizedText : MonoBehaviour
    {
        TMP_Text tmp;
        public string key;

        public void setkey(string keyh)
        {
            key = keyh;
            UpdateText();
        }

        private void Awake()
        {
            tmp = GetComponent<TMP_Text>();
            if (string.IsNullOrEmpty(key)) key = tmp.text;
            UpdateText();
        }
        public void UpdateText()
        {
            if (!tmp) tmp = GetComponent<TMP_Text>();
            try
            {
                tmp.text = LocalizationManager.Instance.GetText(key);
            }
            catch
            { }
        }
    }
}