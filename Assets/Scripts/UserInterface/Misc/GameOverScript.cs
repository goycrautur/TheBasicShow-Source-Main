using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class GameOverScript : MonoBehaviour
{
	private void Start()
	{
		AudioListener.pause = false;
		float ratioy = (float)Screen.width / 360f;
		imaga.transform.DOMoveY(ratioy * 150, 1f);

		audioDevice.ClearQueue(true);
		audioDevice.PlaySingleClip(fnfgaemov);
		whentoplaygameoverloop = fnfgaemov.audClip.length;
		delay = 5f;
		chance = Random.Range(1f, 99f);
		if (chance < 98f)
		{
			int num = Random.Range(0, images.Length);
			image.sprite = images[num];
		}
		else image.sprite = rare;
		 DiscordRPC_stuff.current.UpdateStatus("Game Over Screen", "bro died sobpray", "", "");
	}
	private void gameovertuff()
	{
		float ratioy = (float)Screen.width / 360f;
		text.transform.DOMoveY(ratioy * 35, 3f);
		audioDevice.ClearQueue(true);
		fnfgaemovrdevice.ClearQueue(true);
		fnfgaemovrdevice.QueueAudio(songloop);
		fnfgaemovrdevice.SetLoop(true);
		loopy = true;
		return;
	}

	private void Update()
	{
		if (!youupdated)
		{
			whentoplaygameoverloop -= 1f * Time.deltaTime;
			if (whentoplaygameoverloop <= 0f && !loopy)gameovertuff();
		}
		if (Input.GetButtonDown("Submit") && whentoplaygameoverloop <= 0)
		{
			if (!retryedBooldetect)
			{
				float ratioy = (float)Screen.width / 360f;
				text.transform.DOMoveY(ratioy * -35, 3f);
				thingtext.text = "ok";
				youupdated = true;
				okitaccepte = true;
				fnfgaemovrdevice.ClearQueue(true);
				audioDevice.ClearQueue(true);
				audioDevice.PlaySingleClip(acceptfate);
				retryedBooldetect = true;
			}
			else if (delay >= 0.11f) delay = 0.1f;
		}
		if (okitaccepte)
		{
			delay -= 1f * Time.deltaTime;
			if (delay <= 0f)
			{
				if (chance < 98f) SceneManager.LoadScene(OverScene);
				else
				{
					image.transform.localScale = new Vector3(5f, 5f, 1f);
					image.color = Color.red;
					if (!audioDevice.audioDevice.isPlaying) 
					{
						audioDevice.ClearQueue(true);
						audioDevice.PlaySingleClip(ninetinine);
					}
					if (delay <= -5f) Application.Quit();
				}
			}
		}
	}

	[Header("Game Over Settings")]
	[SerializeField] private Sprite[] images = new Sprite[5];
    [SerializeField] private Sprite rare;
	[SerializeField] private string OverScene;
	[SerializeField] private GameObject text, imaga;
	[SerializeField] private AudioManagerLiveReaction audioDevice, fnfgaemovrdevice;
	[SerializeField] private AudioObjectyeah fnfgaemov, songloop, acceptfate, ninetinine;
	[SerializeField] private Image image;
	[SerializeField] public TMP_Text thingtext;
	private bool youupdated,retryedBooldetect;
	private float delay,whentoplaygameoverloop;
	public float chance;
	private bool okitaccepte,loopy;
}
