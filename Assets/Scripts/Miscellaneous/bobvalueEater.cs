using UnityEngine;

public class bobValStuf : MonoBehaviour
{
    private static float val;

    private void Update()
    {
        val += Time.deltaTime;
    }

    public static float GetBobValue()
    {
        return Mathf.Sin(val * 5f) / 2f;
    }
}