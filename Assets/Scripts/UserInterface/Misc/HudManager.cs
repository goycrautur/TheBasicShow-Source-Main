using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudManager : MonoBehaviour
{
	private void Awake() => Instance = this;
    public static HudManager Instance;
    [SerializeField] public GaugeManager gaugeManager;
    [SerializeField] private Image[] SpritesToDarken;
    [SerializeField] private Color darkColor = new Color(0.25f, 0.25f, 0.25f, 1f);
	public float colorValue = 1f, colorwhenValue = 1f;
	public void colorVarSetter(bool hhhh)
    {
		colorwhenValue = hhhh ? 1f : 0f;
    }

    private void Update()
	{
		if (colorValue != colorwhenValue)
		{
			ForceUpdateColor();
		}
	}
	public void ForceUpdateColor()
	{
		float num = Time.deltaTime * 2f;
		colorValue = Mathf.Max(colorValue - num, Mathf.Min(colorValue + num, colorwhenValue));
		Image[] array = SpritesToDarken;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = Color.Lerp(darkColor, Color.white, colorValue - Mathf.Repeat(colorValue, 0.2f));
		}
		gaugeManager.SetGaugesColor(Color.Lerp(darkColor, Color.white, colorValue - Mathf.Repeat(colorValue, 0.2f)));
	}
}
