using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bosTrig : MonoBehaviour
{
    [SerializeField]
    private BossfightTriggerScript trigger;

    public bool IsEnterBossTrigger()
    {
        return trigger.Enter;
    }
}
