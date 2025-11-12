using TMPro;
using UnityEngine;

public class JumpRopeScript : MonoBehaviour
{
	#region Methods
	private void OnEnable()
	{
		KeyCode jumpKey = Singleton<InputManager>.Instance.KeyboardMapping[InputAction.Jump];
		string jumpkeyFR = $"{jumpKey}";
		string jumpkeyFRthe2nd = jumpkeyFR == "Mouse0" ? "Left Mouse Button" : jumpkeyFR == "Mouse1" ? "Right Mouse Button" : jumpkeyFR;
		Instructions.text = $"Time to jump rope!\nUse {jumpkeyFRthe2nd} to jump!";

		jumpDelay = 1f;
		ropeHit = true;
		jumpStarted = false;
		jumps = 0;

		if (AdditionalGameCustomizer.Instance.RandomizeJumps)
			maxJumps = Random.Range(1, 10);

		jumpCount.text = $"{jumps}/{maxJumps}";
	}

	private void Update()
	{
		if (jumpDelay > 0f)
		{
			jumpDelay -= Time.deltaTime;
			return;
		}

		if (!jumpStarted)
		{
			if (jumps >= maxJumps)
			{
				EndJumpRope();
				return;
			}

			StartJumpRope();
		}

		if (ropePosition > 0f)
		{
			ropePosition -= Time.deltaTime;
		}
		else if (!ropeHit)
		{
			RopeHit();
		}
	}
	#endregion

	#region Core Logic
	private void StartJumpRope()
	{
		jumpStarted = true;
		ropePosition = 1f;
		rope.SetTrigger("ActivateJumpRope");
		ropeHit = false;
	}

	private void RopeHit()
	{
		ropeHit = true;

		if (cs.jumpHeight <= 0.2f)
			Fail();
		else
			Success();

		jumpStarted = false;
	}

	private void Success()
	{
		playtime.audioDevice.Stop();
		playtime.audioDevice.PlayOneShot(playtime.aud_Numbers[jumps].audios);
		//UIPopupTextManagerWithMovement.Show(playtime.aud_Numbers[jumps].captionsTextDatarea, playtime.aud_Numbers[jumps].holor, playtime.eeek.transform, playtime.aud_Numbers[jumps].audios.length, 0f);

		jumps++;
		jumpCount.text = $"{jumps}/{maxJumps}";
		jumpDelay = 0.25f;
	}

	private void Fail()
	{
		jumps = 0;
		jumpCount.text = $"{jumps}/{maxJumps}";
		jumpDelay = 2f;

		playtime.audioDevice.Stop();
		playtime.audioDevice.PlayOneShot(playtime.aud_Oops);
		//UIPopupTextManagerWithMovement.Show("MemeBoiLines_3", Color.red, playtime.eeek.transform, playtime.aud_Oops.length, 0f);
	}

	private void EndJumpRope()
	{
		playtime.canTargetPlayer = true;
		playtime.jumpRopeStarted = false;
		playtime.playCool = 15f;

		playtime.audioDevice.PlayOneShot(playtime.aud_Congrats);
		//UIPopupTextManagerWithMovement.Show("MemeBoiLines_4", Color.red, playtime.eeek.transform, playtime.aud_Congrats.length, 0f);
		ps.DeactivateJumpRope();
		playtime.ResumeWandering();
	}
	#endregion
	
	#region SerializedFields
	[Header("UI Elements")]
    [SerializeField] private TMP_Text jumpCount;
    [SerializeField] private TMP_Text Instructions;
	
    [Header("Rope Animation")]
    [SerializeField] private Animator rope;

    [Header("Player and Playtime Scripts")]
    public CameraScript cs;
    public PlayerScript ps;
    public PlaytimeScript playtime;

    [Header("Jumping Variables")]
    [SerializeField] private int jumps;
    [SerializeField] private float jumpDelay;
    [SerializeField] private float ropePosition;

    [Header("State Variables")]
    [SerializeField] private bool ropeHit;
    [SerializeField] private bool jumpStarted;
   
    [Header("Jump Settings")]
    [SerializeField] private int maxJumps;
    #endregion
}