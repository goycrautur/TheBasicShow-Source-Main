using System.Collections;
using UnityEngine;

public class ITM_dimcraab : BaseItem
{
    public override bool OnUse()
    {
        if (GameControllerScript.Instance.player.oncar) return false;
        GameControllerScript.Instance.player.asgor(SpeedModifier,duration);
        return true;
    }
    [SerializeField] private float duration = 60f;
    [SerializeField] private MovementModifier SpeedModifier = new MovementModifier(default(Vector3), 0f);
}
