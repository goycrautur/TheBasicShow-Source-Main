using System;
using Discord;
using UnityEngine;
public class DiscordRPC_stuff : MonoBehaviour
{
    //please forgive me i stole this from ron caseoh basics i DONT want to deal with this headache anymore for god sake :sob:
	private void Awake()
    {
		if (DiscordRPC_stuff.current == null)
		{
			DiscordRPC_stuff.current = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
    }

	private void Start()
	{
		if (discord == null)
		{
			discord = new Discord.Discord(applicationID, 1UL);
		}
	}

	private void Update()
	{
		if (!cantConnect)
		{
			try
			{
				discord.RunCallbacks();
			}
			catch
			{
				cantConnect = true;
			}
		}
		#if UNITY_EDITOR
		ActivityManager activityManager = discord.GetActivityManager();
		Activity activity = default(Activity);
		activity.Details = "Unity Editor";
		Activity activity2 = activity;
		activityManager.UpdateActivity(activity2, delegate (Result res)
		{
			if (res != Result.Ok)
			{
				Debug.LogWarning("Failed to connect to Discord.");
			}
		});
		#endif
	}
    public void UpdateStatus(string details = "", string state = "", string largeImage = "", string largeText = "")
	{
		//Debug.Log("Details:" + details + " State:" + state + " largeImage:" + largeImage + " largeText:" + largeText);
		#if UNITY_EDITOR
		return;
		#endif
		if (cantConnect)
		{
			return;
		}
		if (discord == null)
		{
			discord = new Discord.Discord(applicationID, 1UL);
		}
		ActivityManager activityManager = discord.GetActivityManager();
		Activity activity = default(Activity);
		activity.Details = details;
		activity.State = state;
		activity.Assets.LargeImage = largeImage;
		activity.Assets.LargeText = largeText;
		Activity activity2 = activity;
		activityManager.UpdateActivity(activity2, delegate (Result res)
		{
			if (res != Result.Ok)
			{
				Debug.LogWarning("Failed to connect to Discord.");
			}
		});
	}
	public static DiscordRPC_stuff current;
	public Discord.Discord discord;
	public long applicationID;
	public bool cantConnect;
}
