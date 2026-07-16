using UnityEngine;
using System;

public class ITM_randomblock : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript Contoller = GameControllerScript.Instance;
        GameObject gameObject = Instantiate<GameObject>(cubeobj, Contoller.player.transform.position, Contoller.cameraTransform.rotation);
        gameObject.GetComponent<litearllyAnBlockScript>().gc = GameControllerScript.Instance;
        return true;
    }
    [SerializeField] private GameObject cubeobj;
}
