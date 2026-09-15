using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPrefsLockBehind : MonoBehaviour
{
    public GameObject thingtodisable;
    private bool unloc;
    public bool dotheOpposite;
    public string playerprefsname;
    [Header("String stuff")]
    public bool StringPlayerprefs;
    public string StringValue;

    public void Start()
    {
        thingtodisable.SetActive(false);
        if (!StringPlayerprefs)
        {
        unloc = PlayerPrefsExtension.GetBool(playerprefsname);
        thingtodisable.SetActive(!dotheOpposite? unloc : !unloc);
        }
        else 
        {
            string TempStringStfu = PlayerPrefs.GetString(playerprefsname);
            
            if (TempStringStfu == StringValue) 
            {
                thingtodisable.SetActive(!dotheOpposite? true : false);
            }
        }
        
    }
}
