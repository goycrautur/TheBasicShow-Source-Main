using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private int fps;
    private float averageFps = 1f;
    [SerializeField] private TMP_Text fpsText;

    private void Update()
    {
        averageFps = 0.9f * averageFps + (1f - 0.9f) * 1f / Time.unscaledDeltaTime;
        fps = Mathf.RoundToInt(averageFps);
        fpsText.text = fps.ToString() + " fps";
    }
}