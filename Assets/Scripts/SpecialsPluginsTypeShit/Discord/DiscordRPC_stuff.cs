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
			discord = new Discord.Discord(applicationID, (ulong)Discord.CreateFlags.NoRequireDiscord);
		}
		currentActivity = new Activity
        {
            State = StateStatus,
            Details = StateDetails,
            Timestamps =
            {
                Start = System.DateTimeOffset.Now.ToUnixTimeSeconds() 
            }
        };
	}
	private void OnDisable()
    {
        discord.Dispose();
    }

	private void Update()
	{
		if (!Refresh)
		{
			try
			{
				discord.RunCallbacks();
				
			}
			catch
			{
				Refresh = true;
			}
		}
        changeActivity();
	}
	public void changeActivity()
    {
		currentActivity.Details = StateDetails;
		currentActivity.State = StateStatus;
		currentActivity.Assets.LargeImage = StateIMGLarge;
		currentActivity.Assets.LargeText = StateIMGSmall;
        var activityManager = discord.GetActivityManager();
        activityManager.UpdateActivity(currentActivity, result =>
        {
            if (result != Discord.Result.Ok)
                Debug.LogWarning("Failed to update Discord!");
        });
    }
    public void UpdateStatus(string details = "", string state = "", string largeImage = "", string largeText = "")
	{
		StateDetails = details;
        StateStatus = state;
		StateIMGLarge = largeImage;
		StateIMGSmall = largeText;
	}
	public static DiscordRPC_stuff current;
	public Discord.Discord discord;
	public long applicationID;
	public bool Refresh;
	public string StateDetails,StateStatus,StateIMGLarge,StateIMGSmall;
	private Activity currentActivity; 
}
