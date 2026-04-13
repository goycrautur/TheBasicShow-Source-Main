using System;
using Discord;
using UnityEngine;
public class DiscordRPC_stuff : MonoBehaviour
{
    //ron discord rpc x swordablet discord rpc yum
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
		if (discord != null) discord.Dispose();
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
				Debug.LogWarning("discord was found dead cuz you dont have it on haha"); // dont spam console please
				
				Refresh = true;
			}
		}
        changeActivity();
	}
	public void changeActivity()
    {
		if (!Refresh)
		{
			currentActivity.Details = StateDetails;
			currentActivity.State = StateStatus;
			currentActivity.Assets.LargeImage = StateIMGLarge;
			currentActivity.Assets.LargeText = LargeIMGText;
        	var activityManager = discord.GetActivityManager();
        	activityManager.UpdateActivity(currentActivity, result =>
        	{
        	    if (result != Discord.Result.Ok)
        	        Debug.LogWarning("Failed to update Discord!");
        	});
		}
    }
    public void UpdateStatus(string details = "", string state = "", string largeImage = "", string largeText = "")
	{
		StateDetails = details;
        StateStatus = state;
		StateIMGLarge = largeImage;
		LargeIMGText = largeText;
	}
	public static DiscordRPC_stuff current;
	public Discord.Discord discord;
	public long applicationID;
	public bool Refresh,Dont;
	public string StateDetails,StateStatus,StateIMGLarge,LargeIMGText;
	private Activity currentActivity; 
}
