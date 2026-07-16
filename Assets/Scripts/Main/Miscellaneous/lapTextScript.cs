using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class lapTextScript : MonoBehaviour
{
    public void Update()
    {
        if (updateText)
        {
            if (GameControllerScript.Instance.LapManag.CurrentLap == 0)
            {
                thingtext.text = "Lap 2";
            }
            else
                thingtext.text = "Lap " + (GameControllerScript.Instance.LapManag.CurrentLap+1);
        }
    }
    public void OnEnable()
    {
        updateText = true;
    }
    public void OnDisable()
    {
        updateText = false;
    }
    public void OnDestroy()
    {
        updateText = false;
    }
    [SerializeField] public TMP_Text thingtext;
    private bool updateText;
}
