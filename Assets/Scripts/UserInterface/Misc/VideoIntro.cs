using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoIntro : MonoBehaviour
{
	private void Start()
	{
		Singleton<Options>.Instance.Start();
		Singleton<Options>.Instance.GetVolume();
        Singleton<Options>.Instance.GetVSync();
		bool skip = PlayerPrefsExtension.GetBool("warnskip");
		if (skip) SceneManager.LoadScene(scenenaem);
		videoPlayer.Play();
		videoPlayer.loopPointReached += OnVideoFinished;
	}

	private void OnVideoFinished(VideoPlayer vp)
	{
		splash.sploosh();
        videoPlayer.enabled = false;
	}

	public VideoPlayer videoPlayer;
    public UISplashScreenManager splash;
	public string scenenaem;
}
