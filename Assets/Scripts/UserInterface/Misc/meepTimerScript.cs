using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class meepTimerScript : MonoBehaviour
{
    private void Awake() => Instance = this;
    public static meepTimerScript Instance;
    private void Start()
	{
		TimerImage = base.GetComponent<Image>();
		startingTime = 30f;
        float ratioy = (float)Screen.width / 360f;
        transform.DOMoveY(ratioy * 180, 3f);
	}
	private void Update()
	{
		if (canTime)startingTime -= Time.deltaTime;
		int num = Mathf.FloorToInt(startingTime / 60f);
        int num2 = Mathf.FloorToInt(startingTime % 60f);
		countdownText.text = string.Format("{0:00}:{1:00}", num, num2);
		if (startingTime <= 1f)
		{
			canTime = false;
			Player.SetHP(PlayerScript.HealthChangeMode.Remove, 1f, 0f, true, false);
		}
		if (startingTime > 0f) canTime = !inEnding ? true : false;
		if ((Mathf.FloorToInt(startingTime) != Mathf.FloorToInt(startingTime + Time.deltaTime) & canTime) && tickSound != null)
		{
			AudMang.ClearQueue(true);
			AudMang.PlaySingleClip(tickSound);
		}
	}
	public void AddTime(float timeToAdd,Color leColor, bool SetTime=false)
	{
		StartCoroutine(AnimateTimeAddCoroutine(timeToAdd, leColor,SetTime));
	}
	private IEnumerator AnimateTimeAddCoroutine(float timeToAdd,Color leColor,bool SetTime)
	{
		float initialTime = startingTime;
		float targetTime = SetTime ? timeToAdd : initialTime + timeToAdd;
		float elapsedTime = 0f;
		float originalTime = startingTime;
		countdownText.color = leColor;
		if (timeAddedSound != null)
		{
			AudMang.ClearQueue(true);
			AudMang.PlaySingleClip(timeAddedSound);
		}
		while (elapsedTime < animationDuration)
		{
			startingTime = Mathf.Lerp(initialTime, targetTime, elapsedTime / animationDuration);
			elapsedTime += Time.deltaTime;
			canTime = false;
			yield return null;
		}
		countdownText.color = Color.white;
		startingTime = SetTime ? timeToAdd : originalTime + timeToAdd;
		canTime = true;
		yield break;
	}

	public bool canTime = true,inEnding;

	public float startingTime;
	public TMP_Text countdownText;
    public PlayerScript Player;
	public AudioManagerLiveReaction AudMang;
	private Image TimerImage;
	public AudioObjectyeah tickSound,timeAddedSound;
	public float animationDuration = 1f;
}
