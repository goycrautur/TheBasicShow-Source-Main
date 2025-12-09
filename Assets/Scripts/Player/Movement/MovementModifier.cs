using UnityEngine;

[System.Serializable]
public class MovementModifier
{
    public MovementModifier(Vector3 addend, float multiplier)
    {
        movementAddend = addend;
        movementMultiplier = multiplier;
    }

    public Vector3 movementAddend;

    public float movementMultiplier = 1f;
}
