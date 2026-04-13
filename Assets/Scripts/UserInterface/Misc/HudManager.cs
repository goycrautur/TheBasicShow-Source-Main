using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudManager : MonoBehaviour
{
	private void Awake() => Instance = this;
    public static HudManager Instance;
	[Header("Gauges and hud color stuff")]
    [SerializeField] public GaugeManager gaugeManager;
    [SerializeField] private Image[] SpritesToDarken;
    [SerializeField] private Color darkColor = new Color(0.25f, 0.25f, 0.25f, 1f);
	public float colorValue = 1f, colorwhenValue = 1f;
	[Header("Global Item Slots anim settings shit")]
	public List<ItemImageSlide> ItemSlotsAnim = new List<ItemImageSlide>();
    [SerializeField] private float BaseSlideSpeed = 567f,BaseSpinSpeed=100f,BaseSlideDistance = 123f,BaseUpDistance = 50f,BaseDownDistance = -50f;
	[SerializeField] private float SlideSpeedValAddend = 567f,BaseSpinSpeedValAddend=100f,BaseSlideDistanceValAddend = 123f,BaseUpDistanceValAddend = 50f,BaseDownDistanceValAddend = -50f;
	public void colorVarSetter(bool hhhh)
    {
		colorwhenValue = hhhh ? 1f : 0f;
    }
	public void Start()
	{
		UpdateItemslotAnimValueStuff();
	}
	public void UpdateItemslotAnimValueStuff()
	{
		for (int i = 0; i < ItemSlotsAnim.Count; i++)
        {
			ItemSlotsAnim[i].speed = BaseSlideSpeed + (SlideSpeedValAddend * ItemSlotsAnim[i].tsId);
			ItemSlotsAnim[i].spinSpeed = BaseSpinSpeed + (BaseSpinSpeedValAddend * ItemSlotsAnim[i].tsId);
			ItemSlotsAnim[i].slideDistance = BaseSlideDistance + (BaseSlideDistanceValAddend * ItemSlotsAnim[i].tsId);
			ItemSlotsAnim[i].upDistance = BaseUpDistance + (BaseUpDistanceValAddend * ItemSlotsAnim[i].tsId);
			ItemSlotsAnim[i].downDistance = BaseDownDistance + (BaseDownDistanceValAddend * ItemSlotsAnim[i].tsId);
        }
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
