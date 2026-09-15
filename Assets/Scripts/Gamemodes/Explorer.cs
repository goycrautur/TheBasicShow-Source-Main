using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : MonoBehaviour
{
    public void Start()
    {
        if (gc.mode == "Explorer") gc.UnlockAmount = 0;
    }
    private GameControllerScript gc => GameControllerScript.Instance;
}
