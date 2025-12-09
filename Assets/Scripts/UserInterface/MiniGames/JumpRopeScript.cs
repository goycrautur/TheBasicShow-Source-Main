using TMPro;
using UnityEngine;
using System.Collections;

public class JumpRopeScript : MonoBehaviour
{
	#region Methods
	public void jumpRopTime(PlaytimeScript playtimething)
	{
		playtime = playtimething;
		if (AdditionalGameCustomizer.Instance.RandomizeJumps)
		{
			maxJumps = Random.Range(1, 10);
		}

		startPos = GameControllerScript.Instance.player.transform.position;
		initialized = true;
		jumpDelay = 1f;
		ropeHit = true;
		jumps = 0;
		GameControllerScript.Instance.player.pModManag.movementModifiers.Add(jumpropeMoveModifier);
		jumpropeMoveModifier.movementMultiplier = 0f;

		KeyCode jumpKey = Singleton<InputManager>.Instance.KeyboardMapping[InputAction.Jump];
		string jumpkeyFR = $"{jumpKey}";
		string jumpkeyFRthe2nd = jumpkeyFR == "Mouse0" ? "Left Mouse Button" : jumpkeyFR == "Mouse1" ? "Right Mouse Button" : jumpkeyFR;
		Instructions.text = $"Time to jump rope!\nUse {jumpkeyFRthe2nd} to jump!";
		jumpCount.text = $"{jumps}/{maxJumps}";
		StartCoroutine(Rope());
	}

	#endregion
	private IEnumerator Rope()
	{
		if (jumps < maxJumps)
		{
            if (jumpDelay <= 0f)
                jumpDelay = 1f;

            float delay = jumpDelay;
            while (delay > 0f)
			{
                delay -= Time.deltaTime * speedModifier;
				yield return null;
			}
            rope.SetTrigger("ActivateJumpRope");
            ropeHit = false;
            float moveTime = 0.6f;
            while (moveTime > 0f)
            {
                moveTime -= Time.deltaTime * speedModifier;
                yield return null;
            }
			if (!ropeHit)
                RopeHit();

            StartCoroutine(Rope());
            yield break;
		}
		while (jumpHeight > 0f)
		{
            yield return null;
		}
		End(true);
    }
	private void Update()
	{
		if (initialized)
		{
        	if (Singleton<InputManager>.Instance.GetActionKeyDown(InputAction.Jump) && jumpHeight <= 0f)
			{
        	    StartCoroutine(Jump());
			}

        	if ((GameControllerScript.Instance.player.transform.position - startPos).magnitude >= 20f)
			{
            	End(false);
			}
		}
    }

	#region Core Logic
	private void RopeHit()
	{
		ropeHit = true;
		if (jumpHeight <= 0.2f)
            Fail();
        else
            Success();
        jumpDelay = 0.7f;
    }
	private IEnumerator Jump()
	{
        float startVelocity = initVelocity;
        while (jumpHeight >= 0f)
        {
            jumpropeMoveModifier.movementMultiplier = 0.25f;
            jumpHeight += startVelocity * Time.deltaTime + 0.5f * -42f * Time.deltaTime * Time.deltaTime * speedModifier;
            startVelocity += -42f * Time.deltaTime * speedModifier;
            CameraScript.Instance.jumpfloatThing = jumpHeight + 4.88f;
            yield return null;
        }
        CameraScript.Instance.jumpfloatThing = 4.88f;
        jumpHeight = 0f;
        jumpropeMoveModifier.movementMultiplier = 0f;
        yield break;
    }

	private void Success()
	{
		playtime.audioDevice.Stop();
		playtime.audioDevice.PlayOneShot(playtime.aud_Numbers[jumps].audios);
		jumps++;
		jumpCount.text = $"{jumps}/{maxJumps}";
	}

	private void Fail()
	{
		jumps = 0;
		jumpCount.text = $"{jumps}/{maxJumps}";
		playtime.audioDevice.Stop();
		playtime.audioDevice.PlayOneShot(playtime.aud_Oops);
	}
	public void End(bool success)
	{
		if (success)
		{
			playtime.canTargetPlayer = true;
            playtime.jumpRopeStarted = false;
            playtime.PlayCool = 15f;
            playtime.audioDevice.PlayOneShot(playtime.aud_Congrats);
		}
		else
		{
			
            playtime.Disappoint();
        }
		playtime.dontUpdateTheSpeedYOUFUCKINGBITCH = false;
		playtime.disablingWandering = false;
        GameControllerScript.Instance.player.jumpropes.Remove(this);
        GameControllerScript.Instance.player.pModManag.movementModifiers.Remove(jumpropeMoveModifier);
        jumpHeight = 0f;
		Destroy(base.gameObject);
    }
	#endregion
	
	#region SerializedFields
	[Header("UI Elements")]
    [SerializeField] private TMP_Text jumpCount;
    [SerializeField] private TMP_Text Instructions;
	
    [Header("Rope Animation")]
    [SerializeField] private Animator rope;

    [Header("Player and Playtime Scripts")]
    public PlaytimeScript playtime;

    [Header("Jumping Variables")]
	[SerializeField] private float initVelocity = 16f;
	[SerializeField] private MovementModifier jumpropeMoveModifier = new MovementModifier(default(Vector3), 0f);
    [SerializeField] private int jumps;
    [SerializeField] private float jumpDelay;
    [SerializeField] private float jumpHeight, velocity;

    [Header("State Variables")]
    [SerializeField] private bool ropeHit;
    public bool initialized;
	[SerializeField] private Vector3 startPos;
   
    [Header("Jump Settings")]
    [SerializeField] private int maxJumps;
	[SerializeField] private float speedModifier = 1f;
    #endregion
}