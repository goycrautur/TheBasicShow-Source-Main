using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace YuriArchive.GlobalLocalization
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance;
        public List<LanguageDefault> defaultLanguages = new List<LanguageDefault>();
        public string currentLang = "EN";
        public int currentJsonVersion = 1;
        private Dictionary<string, string> data = new Dictionary<string, string>();
        private string folderPath;

        [System.Serializable] public class Pair { public string key; public string value; }

        [System.Serializable] public class Wrapper { public int version; public List<Pair> list; }

        [System.Serializable]
        public class LanguageDefault
        {
            public string language;
            public List<Pair> pairs = new List<Pair>();

            public Dictionary<string, string> ToDictionary()
            {
                var dict = new Dictionary<string, string>();
                foreach (var p in pairs)
                    if (!string.IsNullOrEmpty(p.key))
                        dict[p.key] = p.value;
                return dict;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                folderPath = Path.Combine(Application.persistentDataPath, "Yuri Localization Stuff");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                if (defaultLanguages.Count == 0)
                {
                    var en = new LanguageDefault
                    {
                        language = "EN",
                        pairs = new List<Pair>
                        {
                            new Pair { key = "Menu_Warning", value = "WARNING" },
                            new Pair { key = "Menu_WarningDescription", value = "Insert the rest here..." }
                        }
                    };
                    defaultLanguages.Add(en);
                }
                LoadLanguage(currentLang);
            }
            else Destroy(gameObject);
        }

        public void LoadLanguage(string lang)
        {
            folderPath = Path.Combine(Application.persistentDataPath, "Yuri Localization Stuff");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            currentLang = lang;
            string filePath = Path.Combine(folderPath, "Local_" + lang + ".json");
            if (!File.Exists(filePath))
            {
                var defaults = GetDefaultDict(lang);
                WriteJsonFile(filePath, defaults);
                data = new Dictionary<string, string>(defaults);
            }
            else
            {
                string json = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(json))
                {
                    var defaults = GetDefaultDict(lang);
                    data = new Dictionary<string, string>(defaults);
                    WriteJsonFile(filePath, defaults);
                }
                else
                {
                    Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
                    if (wrapper == null || wrapper.list == null || wrapper.version < currentJsonVersion)
                    {
                        var defaults = GetDefaultDict(lang);
                        data = new Dictionary<string, string>(defaults);
                        WriteJsonFile(filePath, defaults);
                    }
                    else
                    {
                        data = ConvertToDict(wrapper.list);
                    }
                }
            }
            UpdateAllLocalizedText();
        }

        private Dictionary<string, string> GetDefaultDict(string lang)
        {
            foreach (var d in defaultLanguages)
                if (d.language.Equals(lang))
                    return d.ToDictionary();
            return defaultLanguages.Count > 0 ? defaultLanguages[0].ToDictionary() : new Dictionary<string, string>();
        }

        public string GetText(string key)
        {
            if (data.TryGetValue(key, out string value)) return value;
            return key;
        }

        public void UpdateAllLocalizedText()
        {
            foreach (var item in FindObjectsOfType<LocalizedText>(true))
                item.UpdateText();
        }

        private List<Pair> ConvertToList(Dictionary<string, string> dict)
        {
            List<Pair> list = new List<Pair>();
            foreach (var kv in dict)
                list.Add(new Pair { key = kv.Key, value = kv.Value });
            return list;
        }

        private Dictionary<string, string> ConvertToDict(List<Pair> list)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var p in list)
                if (!string.IsNullOrEmpty(p.key))
                    dict[p.key] = p.value;
            return dict;
        }

        private void WriteJsonFile(string filePath, Dictionary<string, string> dict)
        {
            var pairs = ConvertToList(dict);
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"version\":").Append(currentJsonVersion).Append(",\"list\":[");
            if (pairs.Count > 0) sb.AppendLine();

            for (int i = 0; i < pairs.Count; i++)
            {
                string pairJson = JsonUtility.ToJson(pairs[i], false);
                sb.Append(pairJson);
                if (i < pairs.Count - 1) sb.Append(",");
                sb.AppendLine();
            }
            sb.Append("]}");
            File.WriteAllText(filePath, sb.ToString());
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(LocalizationManager))]
        public class LocalizationManagerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                if (GUILayout.Button("Update Default Dictionaries"))
                {
                    var mgr = (LocalizationManager)target;
                    mgr.UpdateDefaultDictionaries();
                }
                if (GUILayout.Button("Rebuild All JSONs From Defaults"))
                {
                    var mgr = (LocalizationManager)target;
                    mgr.RebuildAllJsonsFromDefaults();
                }
            }
        }
#endif

        public void UpdateDefaultDictionaries()
        {
            folderPath = Path.Combine(Application.persistentDataPath, "Yuri Localization Stuff");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string[] files = Directory.GetFiles(folderPath, "Local_*.json");
            if (files == null || files.Length == 0)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Localization", "No localization files found.", "OK");
#endif
                return;
            }
            List<LanguageDefault> updated = new List<LanguageDefault>();
            foreach (string file in files)
            {
                if (string.IsNullOrEmpty(file) || !File.Exists(file)) continue;
                string json = File.ReadAllText(file);
                if (string.IsNullOrEmpty(json)) continue;
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
                if (wrapper == null || wrapper.list == null) continue;

                string name = Path.GetFileNameWithoutExtension(file).Replace("Local_", "");
                if (wrapper.version < currentJsonVersion)
                {
                    var defaults = GetDefaultDict(name);
                    WriteJsonFile(file, defaults);
                    updated.Add(new LanguageDefault { language = name, pairs = ConvertToList(defaults) });
                }
                else
                {
                    updated.Add(new LanguageDefault { language = name, pairs = wrapper.list });
                }
            }
            defaultLanguages = updated;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            Debug.Log("Default language dictionaries updated.");
#endif
        }

        public void RebuildAllJsonsFromDefaults()
        {
            folderPath = Path.Combine(Application.persistentDataPath, "Yuri Localization Stuff");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            foreach (var lang in defaultLanguages)
            {
                string filePath = Path.Combine(folderPath, "Local_" + lang.language + ".json");
                var dict = lang.ToDictionary();
                WriteJsonFile(filePath, dict);
            }
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Localization", "Rebuilt all JSON localization files from defaults.", "OK");
            Debug.Log("Rebuilt all localization JSON files from defaultLanguages.");
#endif
        }
    }
}