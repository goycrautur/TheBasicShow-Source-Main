using System;

//using Discord;
using Lachee.Discord;
using UnityEngine;
//using DiscordRPC;
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
		DiscordManager.current.applicationID = applicationID.ToString();
		DiscordManager.current.OnPresence.AddListener((message) =>
		{
			Debug.Log("Received a new presence! Current App: " + message.applicationID + ", " + message.name);
			this.presence = presence;
		});
		Invoke(nameof(changeActivity), 0.25f);
		presence.startTime = DateTime.UtcNow;

		
		/*client = new DiscordRpcClient(applicationID.ToString());
		client.OnReady += (sender, e) =>
        {
            Debug.Log($"Discord RPC is ready for user: {e.User.Username}");
        };
        client.Initialize();
		if (discord == null)
		{
			discord = new Discord.Discord(applicationID, (ulong)Discord.CreateFlags.NoRequireDiscord);
			//discord = new Discord.Discord(applicationID);
		}
		currentActivity = new Activity
        {
            State = StateStatus,
            Details = StateDetails,
			Timestamps =
			{
				Start = System.DateTimeOffset.Now.ToUnixTimeSeconds() 
            }
       	};*/
	}
	private void OnDisable()
    {
		//if (discord != null) discord.Dispose();
		/*if (client != null) 
		{
			client.ClearPresence();
			client.Dispose();
		}*/
    }
	private void OnApplicationQuit()
    {
        /*if (client != null)
        {
            client.ClearPresence();
            client.Dispose();
        }*/
    }

	private void Update()
	{
		if (!Refresh)
		{
			try
			{
				//discord.RunCallbacks();
				//if (client != null) client.Invoke();
				
			}
			catch
			{
				Debug.LogWarning("discord was found dead cuz you dont have it on haha"); // dont spam console please
				
				Refresh = true;
			}
		}
	}
	public void changeActivity()
    {
		if (!Refresh)
		{
			if (presence == null) return;
			presence.state = StateStatus;
			presence.details = StateDetails;

			presence.largeAsset = new Asset()
			{
				image = StateIMGLarge,
				tooltip = LargeIMGText
			};
			/*presence.smallAsset = new Asset()
			{
				image = inputSmallKey.text,
				tooltip = inputSmallTooltip.text
			};*/
			presence.buttons = new Button[]
			{
				new Button()
				{
					label = "join this craz baldi mod server!",
					url = "https://discord.gg/4GV9PsCyJh"
				},
			};
			DiscordManager.current.SetPresence(presence);
			//Debug.Log("invoked");
			Invoke(nameof(changeActivity), 0.25f);
			/*if (client == null || !client.IsInitialized) return;
			client.SetPresence(new RichPresence()
			{
				Details = StateDetails,
				State = StateStatus,
				Timestamps =
				{
					Start = System.DateTimeOffset.Now.ToUnixTimeSeconds() 
				},
				
				Assets = new Assets()
				{
					LargeImageKey = StateIMGLarge,   // Matches the asset name in developer portal
					LargeImageText = LargeIMGText,
					//SmallImageKey = "icon_small",
					//SmallImageText = "Level 1"
				}
			});
			currentActivity.Details = StateDetails;
			currentActivity.State = StateStatus;
			currentActivity.Assets.LargeImage = StateIMGLarge;
			currentActivity.Assets.LargeText = LargeIMGText;
        	var activityManager = discord.GetActivityManager();
        	activityManager.UpdateActivity(currentActivity, result =>
        	{
        	    if (result != Discord.Result.Ok)
        	        Debug.LogWarning("Failed to update Discord!");
        	});*/
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
	//public Discord.Discord discord;
	public long applicationID;
	public bool Refresh,Dont;
	public string StateDetails,StateStatus,StateIMGLarge,LargeIMGText;
	//private Activity currentActivity; 

	//private DiscordRpcClient client;
	public Presence presence;
}
