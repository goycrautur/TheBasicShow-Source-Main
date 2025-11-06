using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateToPlayerMinimapIcon : MonoBehaviour
{
    public int rotati;
    public void LateUpdate()
    {
		base.transform.rotation = Quaternion.Euler(rotati, GameControllerScript.Instance.player.transform.eulerAngles.y, 0f);
    }
}
