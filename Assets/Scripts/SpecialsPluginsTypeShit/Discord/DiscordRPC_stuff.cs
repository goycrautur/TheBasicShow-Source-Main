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
		// residue from reworking the script... dont worry much
		DiscordManager.current.OnPresence.AddListener((message) =>
		{
			Debug.Log("Received a new presence! Current App: " + message.applicationID + ", " + message.name);
			this.presence = presence;
		});
		changeActivity();
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
		// residue from reworking the script... dont worry much (1)
		//if (discord != null) discord.Dispose();
		/*if (client != null) 
		{
			client.ClearPresence();
			client.Dispose();
		}*/
    }
	private void OnApplicationQuit()
    {
		// residue from reworking the script... dont worry much (2)
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
				//// residue from reworking the script... dont worry much (3), and this is for detecting if you dont have discord on it dosent shit itsef
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
			presence.state = StateStatus; //state status
			presence.details = StateDetails; //state details

			presence.largeAsset = new Asset()
			{
				image = StateIMGLarge, //image name, you must set the name exact same like how you put it in the rich presence/art assets
				tooltip = LargeIMGText //text when u hover over the big image
			};
			presence.smallAsset = new Asset()
			{
				image = StateIMGSmall, //image name, you must set the name exact same like how you put it in the rich presence/art assets
				tooltip = SmallIMGText //text when u hover over the small image
			};
			presence.buttons = new Button[] //rich presence button, you can have max 2 of them, they are forced to have url or else it broke btw
			{
				new Button()
				{
					label = "join this craz baldi mod server!",
					url = "https://discord.gg/4GV9PsCyJh"
				},
			};
			DiscordManager.current.SetPresence(presence);
			Invoke(nameof(changeActivity), 0.25f);



			// residue from reworking the script... dont worry much (4)
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
    public void UpdateStatus(string details = "", string state = "", string largeImage = "", string largeImageText = "", string smallImage = "", string smallImageText = "")
	{
		StateDetails = details;
        StateStatus = state;
		StateIMGLarge = largeImage;
		LargeIMGText = largeImageText;
		StateIMGSmall = smallImage;
		SmallIMGText = smallImageText;
	}
	public static DiscordRPC_stuff current;
	
	public bool Refresh,Dont;
	public string StateDetails,StateStatus,StateIMGLarge,LargeIMGText,StateIMGSmall,SmallIMGText;
	// residue from reworking the script... dont worry much (5)
	//private Activity currentActivity; 
	//public Discord.Discord discord;
	//private DiscordRpcClient client;
	public Presence presence;
}
