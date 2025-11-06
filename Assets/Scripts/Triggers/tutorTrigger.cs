using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorTrigger : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static tutorTrigger Instance;
    #endregion
    public bool closeElevator, onetimestuff;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !onetimestuff)
        {
            Debug.Log ("it work");
            if (GameControllerScript.Instance.mode == "story")
            {
                GameControllerScript.Instance.tutorobj.SetActive(true);
            }
            if (GameControllerScript.Instance.mode == "famished" || GameControllerScript.Instance.mode == "zerullclassic" || GameControllerScript.Instance.mode == "LappingOfAsylum")
            {
                closeElevator = true;
                if (GameControllerScript.Instance.mode == "zerullclassic")
                {
                    ZerullClassic.Instance.dosomeupdatebitch();
                }
            }
            onetimestuff = true;
        }
    }
}
