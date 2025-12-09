using System.Collections.Generic;
using UnityEngine;

public class PlayerModifiers : MonoBehaviour
{
    public float Multiplier
    {
        get
        {
            float num = 1f;
            foreach (MovementModifier movementModifier in movementModifiers)
            {
                num *= movementModifier.movementMultiplier;
            }
            return num;
        }
    }

    public Vector3 Addend
    {
        get
        {
            Vector3 vector = Vector3.zero;
            foreach (MovementModifier movementModifier in movementModifiers)
            {
                vector += movementModifier.movementAddend * Time.deltaTime;
            }
            return vector;
        }
    }

    public List<MovementModifier> movementModifiers = new List<MovementModifier>();
}