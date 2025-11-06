using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPrefsLockBehind : MonoBehaviour
{
    public GameObject thingtodisable;
    private bool unloc;
    public bool dotheOpposite;
    public string playerprefsname;

    public void Start()
    {
        unloc = PlayerPrefsExtension.GetBool(playerprefsname);
        if (!dotheOpposite)
        {
            if (unloc)
            {
                thingtodisable.SetActive(true);
            }
            else
            {
                thingtodisable.SetActive(false);
            }
        }
        if (dotheOpposite)
        {
            if (!unloc)
            {
                thingtodisable.SetActive(true);
            }
            else
            {
                thingtodisable.SetActive(false);
            }
        }
    }
}
