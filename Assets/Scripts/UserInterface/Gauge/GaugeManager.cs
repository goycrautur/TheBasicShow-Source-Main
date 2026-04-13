using UnityEngine;
using System.Collections.Generic;

public class GaugeManager : MonoBehaviour
{
    private void Awake() => Instance = this;
    private void Update()
	{
		Vector3 vector = default(Vector3);
		Vector3 localPosition = default(Vector3);
		float num = horizontalSlideSpeed * Time.deltaTime;
		float num2 = verticalSlideSpeed * Time.deltaTime;
		sortedGauges.Clear();
		float num3 = -100f;
		int index = 0;
		int num4 = 0;
		while (sortedGauges.Count < gauges.Count)
		{
			for (int i = 0; i < gauges.Count; i++)
			{
				if (!sortedGauges.Contains(gauges[i]) && gauges[i].time > num3)
				{
					num3 = gauges[i].time;
					index = i;
				}
			}
			sortedGauges.Add(gauges[index]);
			gauges[index].screenRank = num4;
			num4++;
			num3 = -100f;
		}
		verticalGap = 48f;
		if (gauges.Count > 6)verticalGap = Mathf.Floor(288f / (float)gauges.Count - 1f);
		foreach (Gauge sortedGauge in sortedGauges)
		{
			vector.x = ((!sortedGauge.Activated) ? 80 : 0);
			vector.y = (float)sortedGauge.screenRank * verticalGap;
			sortedGauge.position.x = Mathf.Max(sortedGauge.position.x - num, Mathf.Min(sortedGauge.position.x + num, vector.x));
			sortedGauge.position.y = Mathf.Max(sortedGauge.position.y - num2, Mathf.Min(sortedGauge.position.y + num2, vector.y));
			localPosition.x = Mathf.Round(sortedGauge.position.x);
			localPosition.y = Mathf.Round(sortedGauge.position.y);
			sortedGauge.transform.localPosition = localPosition;
		}
		for (int num5 = sortedGauges.Count - 1; num5 >= 0; num5--)sortedGauges[num5].transform.SetSiblingIndex(gauges.Count - sortedGauges[num5].screenRank);
		for (int j = 0; j < gauges.Count; j++)
		{
			if (!gauges[j].Activated && gauges[j].position.x == 80f)
			{
				DestroyGauge(gauges[j]);
				j--;
			}
		}
	}
    public void SetGaugesColor(Color color)
    {
        foreach (Gauge hudGauge in gauges) hudGauge.SetColor(color);
    }
    private void DestroyGauge(Gauge gauge)
	{
		gauges.Remove(gauge);
		Object.Destroy(gauge.gameObject);
	}

	public void Clear()
	{
		while (gauges.Count > 0)
		{
			DestroyGauge(gauges[0]);
		}
	}
    public Gauge CreateGaugeInstance(Sprite icon, float totalTime)
    {
        Gauge gauge = Instantiate(gaugePrefab, transform);
        gauge.transform.localPosition = new Vector3(80f, (float)gauges.Count * verticalGap, 0f);
		gauge.position = gauge.transform.localPosition;
        gauges.Add(gauge);
        gauge.Show(icon, totalTime);
        HudManager.Instance.ForceUpdateColor();
        return gauge;
    }

    public static GaugeManager Instance;
    [SerializeField] private Gauge gaugePrefab;
    private List<Gauge> gauges = new List<Gauge>();

	private List<Gauge> sortedGauges = new List<Gauge>();
    [SerializeField] private float horizontalSlideSpeed = 80f;

	[SerializeField] private float verticalSlideSpeed = 128f;

	private float verticalGap = 48f;
}