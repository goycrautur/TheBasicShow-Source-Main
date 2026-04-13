using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class Gauge : CircleSlider
{
    private void Update()
    {
        if (!shows)
        {
            time -= 1f * Time.deltaTime;
        }
        else
        {
            value = Mathf.Max(value - 1f * Time.deltaTime, Mathf.Min(value + 1f * Time.deltaTime, specialValue));
            //base.UpdateSliderValue(value);
        }
    }

    public void Show(Sprite iconSprite, float showTime)
    {
        shows = true;
        Set(showTime,showTime);
        icon.sprite = iconSprite;
    }

    public void Hide()
    {
        shows = false;
    }

    public void Set(float total, float remain)
    {
        specialValue = Mathf.Clamp(remain / total, 0f, 1f);
        time = remain;
        textShit.text = $"{Math.Round(time, 2)}";
    }
    public void SetColor(Color color)
	{
		Image[] array = toDarkenImage;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = color;
		}
        textShit.color = color;
	}

    private float specialValue;
    private float value;

	public int screenRank;
    [HideInInspector] public float time;
    public Vector3 position;
    private bool shows;
    [SerializeField] private Image[] toDarkenImage;

    public bool Activated
    {
        get
        {
            return shows;
        }
    }

    public Image icon;
    public TMP_Text textShit;
}
