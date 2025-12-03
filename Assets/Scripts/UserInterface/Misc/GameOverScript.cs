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

		audioDevice.PlayOneShot(fnfgaemov);
		whentoplaygameoverloop = fnfgaemov.length;
		delay = 5f;
		chance = Random.Range(1f, 99f);
		if (chance < 98f)
		{
			int num = Random.Range(0, images.Length);
			image.sprite = images[num];
		}
		else
		{
			image.sprite = rare;
		}
		 DiscordRPC_stuff.current.UpdateStatus("Game Over Screen", "bro died sobpray", "", "");
	}
	private void gameovertuff()
	{
		float ratioy = (float)Screen.width / 360f;
		text.transform.DOMoveY(ratioy * 35, 3f);
		if (!fnfgaemovrdevice.isPlaying)
		{
			fnfgaemovrdevice.Play();
		}
	}

	private void Update()
	{
		if (!youupdated)
		{
			whentoplaygameoverloop -= 1f * Time.deltaTime;
			if (whentoplaygameoverloop <= 0f)
			{
				gameovertuff();
			}
		}
		if (Input.GetButtonDown("Submit") && whentoplaygameoverloop <= 0 && !retryedBooldetect)
		{
			float ratioy = (float)Screen.width / 360f;
			text.transform.DOMoveY(ratioy * -35, 3f);
			thingtext.text = "ok";
			youupdated = true;
			okitaccepte = true;
			fnfgaemovrdevice.Stop();
			audioDevice.PlayOneShot(acceptfate);
			retryedBooldetect = true;
		}
		if (okitaccepte)
		{
			delay -= 1f * Time.deltaTime;
			if (delay <= 0f)
			{
				if (chance < 98f)
				{
					SceneManager.LoadScene(OverScene);
				}
				else
				{
					image.transform.localScale = new Vector3(5f, 5f, 1f);
					image.color = Color.red;
					if (!audioDevice.isPlaying)
					{
						audioDevice.Play();
					}
					if (delay <= -5f)
					{
						Application.Quit();
					}
				}
			}
		}
	}

	[Header("Game Over Settings")]
	[SerializeField] private Sprite[] images = new Sprite[5];
    [SerializeField] private Sprite rare;
	[SerializeField] private string OverScene;
	[SerializeField] private GameObject text, imaga;
	[SerializeField] private AudioSource audioDevice, fnfgaemovrdevice;
	[SerializeField] private AudioClip fnfgaemov, acceptfate;
	[SerializeField] private Image image;
	[SerializeField] public TMP_Text thingtext;
	private bool youupdated,retryedBooldetect;
	private float delay,whentoplaygameoverloop;
	public float chance;
	private bool okitaccepte;
}
