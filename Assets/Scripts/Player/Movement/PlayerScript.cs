using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class PlayerScript : MonoBehaviour
{
	#region Lifecycle
	private void Start() => InitializeSettings();

	private void FixedUpdate()
	{
		if (!movementLocked) StaminaCheck();
		HealthCheck();
	}

	private void Update()
	{
		AdditionalGameCustomizer.Instance.ReworkedCurrency = false;
		for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
		{
			if (ItemManager.Instance.Inventory[i].ItemID == 31)
			{
				titlecardtotem = true;
			}
			if (ItemManager.Instance.Inventory[i].ItemID == 34)
			{
				AdditionalGameCustomizer.Instance.ReworkedCurrency = true;
			}
		}
		if (Iframes > 0)
		{
			Iframes -= Time.deltaTime;
		}
		ApplyGravity();
		HandleMouseMovement();
		if (!movementLocked) PlayerMove();
		GuiltCheck();
		InitializeMiscellaneous();
		runSpeed = DefaultRunSpeed * runSpeedMultipler;
		walkSpeed = DefaultWalkSpeed * walkSpeedMultipler;
		staminaDrop = DefaultstaminaDrop * staminaDropMultiple;
		staminaRise = DefaultstaminaRise * staminaRiseMultiple;
		if (!IgnoreHpLimit)
		{
			if (health > maxHealth)
			{
				health = maxHealth;
			}
		}
		if (invisi || invisichalk)
		{
			HudManager.Instance.colorVarSetter(false);
			if (ZerullClassic.Instance.RealBossStarted)
			{
				SetHP(HealthChangeMode.Add, 0.05f, 0f, true, false);
			}
			if (jumpRope)
        	{
            	jumpRope = false;
            	DeactivateJumpRope();
            	playtime.Disappoint();
        	}
		}
		else
		{
			HudManager.Instance.colorVarSetter(true);
		}
		if (breakwindow)
			{
				foreach (WindowScript w in FindObjectsOfType<WindowScript>())
				{
					if (!w.broken)
					{
						if (Vector3.Distance(PlayerTransform.position, w.transform.position) <= 4)
						{
							w.Window(true, true, 6f);
						}
					}
				}
			}
	}
	#endregion
	private IEnumerator summonGaug()
	{
		float time = 10f;
		Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(AdditionalGameCustomizer.Instance.invincibl, 10f);
		titlecardtotem = false;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
			{
				newGauge.Set(10f, time);
				yield return null;
			}
		}
		newGauge.Hide();
		totemParticl.SetActive(false);
		yield break;
	}
	public void totemshit(bool consumetotem = true)
	{
		StartCoroutine(summonGaug());
		TotemAnimator.Rebind();
		TotemAnimator.Play("toteOfUndyused", -1, 0f);
		totemParticl.SetActive(true);
		GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.totem);
		SetHP(HealthChangeMode.Set, maxHealth, 10f, true, false, false);
		if (consumetotem)
		{
			for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
			{
				if (ItemManager.Instance.Inventory[i].ItemID == 31)
				{
					ItemManager.Instance.RemoveItemFromInventory(ItemManager.Instance.GetItem(31));
					return;
				}
			}
		}
	}

	#region Initialization
	private void InitializeSettings()
	{
		gradiHealthbarCheck();
		sensitivityActive = PlayerPrefs.GetInt("AnalogMove") == 1;
		height = transform.position.y;
		stamina = maxStamina;
		health = maxHealth;
		playerRotation = transform.rotation;
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 5);
		principalBugFixer = 1;
		gravity = 2763f;
	}

	private void InitializeMiscellaneous()
	{
		if (!gc.KF.gamePaused && cc.velocity.magnitude > 0f)
		{
			gc.KF.LockMouse();
		}
		if (jumpRope & ((transform.position - frozenPosition).magnitude >= 45f) && CamCam.jumpHeight < 0.1f)
		{
			DeactivateJumpRope();
			playtime.Disappoint();
		}
		if (sweepingFailsave > 0f)
		{
			sweepingFailsave -= Time.deltaTime;
		}
		else
		{
			sweeping = false;
			hugging = false;
		}
	}
	#endregion

	#region Movement & Rotation
	private void ApplyGravity()
	{
		Vector3 grav = Vector3.zero;
		if (!cc.isGrounded)
		{
			grav.y -= gravity * Time.deltaTime;
		}
		cc.Move(grav * Time.deltaTime);
	}

	private void HandleMouseMovement()
	{
		if (!isForcedToLook)
		{
			MouseMove();
		}
		else
		{
			HandleForcedLook();
		}
	}

	private void MouseMove()
	{
		playerRotation.eulerAngles = new Vector3(playerRotation.eulerAngles.x, playerRotation.eulerAngles.y);
		playerRotation.eulerAngles += Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity * Time.timeScale;
		transform.rotation = playerRotation;
	}

	private void HandleForcedLook()
	{
		float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.y, Mathf.Atan2(targetToForcelyLookAt.position.x - transform.position.x, targetToForcelyLookAt.position.z - transform.position.z) * Mathf.Rad2Deg);
		if (Mathf.Abs(angleDiff) < 5f)
		{
			LockOnTarget();
		}
		else
		{
			RotateTowardsTarget(angleDiff);
		}
	}

	private void LockOnTarget()
	{
		transform.LookAt(new Vector3(targetToForcelyLookAt.position.x, transform.position.y, targetToForcelyLookAt.position.z));
		playerRotation = transform.rotation;
		movementLocked = false;
		isForcedToLook = false;
	}

	private void RotateTowardsTarget(float angleDiff)
	{
		transform.Rotate(new Vector3(0f, forceLookSpeed * Mathf.Sign(angleDiff) * Time.deltaTime, 0f));
		playerRotation = transform.rotation;
		movementLocked = true;
	}

	private void PlayerMove()
	{
		if (jumpRope && CamCam.jumpHeight <= 0.1f)
		{
			moveDirection = Vector3.zero;
			cc.Move(Vector3.zero);
			return;
		}

		Vector3 movement = Vector3.zero;
		Vector3 lateralMovement = Vector3.zero;

		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveForward))
		{
			movement = transform.forward;
		}
		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveBackward))
		{
			movement = -transform.forward;
		}
		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveLeft))
		{
			lateralMovement = -transform.right;
		}
		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveRight))
		{
			lateralMovement = transform.right;
		}

		if (jumpRope) moveDirection *= jumpRopeSpeedMultiplier;

		AdjustSpeedAndSensitivity(movement, lateralMovement);
		HandleSpecialMovement();
		cc.Move(moveDirection);
		secondaryMovementVelocity = new Vector3(cc.velocity.x, 0f, cc.velocity.z);
	}

	private void AdjustSpeedAndSensitivity(Vector3 movement, Vector3 lateralMovement)
	{
		bool isRunning = Singleton<InputManager>.Instance.GetActionKey(InputAction.Run) && stamina > 0f;
		playerSpeed = isRunning ? runSpeed : walkSpeed;
		sensitivity = sensitivityActive ? Mathf.Clamp((movement + lateralMovement).magnitude, 0f, 1f) : 1f;
		moveDirection = (movement + lateralMovement).normalized * playerSpeed * sensitivity * Time.deltaTime;

		if (isRunning && secondaryMovementVelocity.magnitude > 0.1f && !hugging && !sweeping)
		{
			if (!outdoorsfr || door.lockTime < 0f)
				ResetGuilt("running", 0.1f);
		}
	}

	private void HandleSpecialMovement()
	{
		if (jumpRope && CamCam.jumpHeight > 0.1f)
		{
			moveDirection *= jumpRopeSpeedMultiplier;
		}
		else if (sweeping && !bootsActive)
		{
			moveDirection = gottaSweep.velocity * Time.deltaTime + moveDirection * 0.3f;
		}
		else if (hugging && !bootsActive)
		{
			moveDirection = (firstPrize.velocity * 1.2f * Time.deltaTime + (new Vector3(firstPrizeTransform.position.x, height, firstPrizeTransform.position.z) + new Vector3(Mathf.RoundToInt(firstPrizeTransform.forward.x), 0f, Mathf.RoundToInt(firstPrizeTransform.forward.z)) * 3f - transform.position)) * principalBugFixer;
		}
	}
	#endregion

	#region Stamina & Guilt & Health
	private void StaminaCheck()
	{
		if (!isSliding) staminaPending = stamina;

		if (!sweeping && Singleton<InputManager>.Instance.GetActionKey(InputAction.Run) && stamina > 0f && cc.velocity.magnitude > 0.1f)
		{
			stamina -= staminaDrop * Time.fixedDeltaTime;
		}
		else if (stamina < maxStamina && cc.velocity.magnitude < 0.1f)
		{
			stamina += staminaRise * Time.fixedDeltaTime;
		}

		_ = staminaPending / maxStamina * 136f;

		if (staminaPending > 100f && staminaPending <= 200f)
		{
			_ = 136f + (staminaPending - 100f) / 100f * 7f;
		}

		SliderCustomization();
	}
	private void HealthCheck()
	{
		if (!hpisSliding) healthPending = health;

		_ = healthPending / maxHealth * 136f;

		if (healthPending > 100f && healthPending <= 200f)
		{
			_ = 136f + (healthPending - 100f) / 100f * 7f;
		}
		healthbar.value = Mathf.MoveTowards(healthbar.value, health / maxHealth * 100f, 50f * Time.deltaTime * 6f);
		if (health <= 0f)
		{
			if (!titlecardtotem)
			{
				gameOver = true;
				RenderSettings.skybox = blackSky;
				StartCoroutine(KeepTheHudOff());
			}
			if (titlecardtotem)
			{
				totemshit();
			}
		}
	}

	private void SliderCustomization()
	{
		if (AdditionalGameCustomizer.Instance != null)
		{
			switch (AdditionalGameCustomizer.Instance.StaminaStyle)
			{
				case AdditionalGameCustomizer.StaminaDisplay.Old:
					staminaBar1.value = stamina / maxStamina * 100f;
					break;
				case AdditionalGameCustomizer.StaminaDisplay.PreOld:
					staminaBar2.value = stamina / maxStamina * 100f;
					break;
				case AdditionalGameCustomizer.StaminaDisplay.Normal:
					staminaBar3.value = Mathf.MoveTowards(staminaBar3.value, stamina / maxStamina * 100f, 50f * Time.deltaTime * 6f);
					break;
				case AdditionalGameCustomizer.StaminaDisplay.Vertical:
					staminaBar4.value = Mathf.MoveTowards(staminaBar4.value, stamina / maxStamina * 100f, 50f * Time.deltaTime * 6f);
					break;
				case AdditionalGameCustomizer.StaminaDisplay.Circle:
					staminaBar5.value = Mathf.MoveTowards(staminaBar5.value, stamina / maxStamina * 100f, 50f * Time.deltaTime * 6f);
					break;
			}
		}
	}

	private IEnumerator StaminometerSlide()
	{
		isSliding = true;
		while (Mathf.Abs(stamina - staminaPending) > 0f)
		{
			staminaPending = Mathf.MoveTowards(staminaPending, stamina, slideSpeed);
			yield return null;
		}
		staminaPending = stamina;
		isSliding = false;
		yield break;
	}
	private IEnumerator HealthSlide()
	{
		hpisSliding = true;
		while (Mathf.Abs(health - healthPending) > 0f)
		{
			healthPending = Mathf.MoveTowards(healthPending, health, healthslideSpeed);
			yield return null;
		}
		healthPending = health;
		hpisSliding = false;
		yield break;
	}

	private void GuiltCheck()
	{
		if (guilt > 0f)
		{
			guilt -= Time.deltaTime;
		}
	}

	public void ResetGuilt(string type, float amount)
	{
		guilt = Mathf.Max(guilt, amount);
		guiltType = type;
	}

	public void SetStamina(StaminaChangeMode mode, float value)
	{
		if (value < 0f) value = 0f;

		switch (mode)
		{
			case StaminaChangeMode.Add: stamina += value; break;
			case StaminaChangeMode.Remove: stamina -= value; break;
			case StaminaChangeMode.Multiply: stamina *= value; break;
			case StaminaChangeMode.Divide: if (value != 0f) stamina /= value; break;
			case StaminaChangeMode.Set: stamina = value; break;
		}

		if (Mathf.Abs(stamina - staminaPending) >= 10f)
		{
			StartCoroutine(StaminometerSlide());
		}
	}
	public void SetHP(HealthChangeMode mode, float fashionevalue, float invinciframes, bool ignoreIframes = false, bool playrandomizedPresetSound = false, bool dontResetIframes = true)
	{
		if (Iframes > 0f && !ignoreIframes)
		{
			return;
		}
		if (!dontResetIframes)
		{
			Iframes = invinciframes;
		}

		if (fashionevalue < 0f)
			{
				fashionevalue = 0f;
			}
		if (playrandomizedPresetSound)
		{
			audVal = (int)Random.Range(0f, GameControllerScript.Instance.HurtSounds.Length);
			GameControllerScript.Instance.audioDevice2.PlayOneShot(GameControllerScript.Instance.HurtSounds[audVal]);
		}
		switch (mode)
		{
			case HealthChangeMode.Add: health += fashionevalue; break;
			case HealthChangeMode.Remove: health -= fashionevalue; break;
			case HealthChangeMode.Multiply: health *= fashionevalue;break;
			case HealthChangeMode.Divide: if (fashionevalue != 0f) health /= fashionevalue; break;
			case HealthChangeMode.Set: health = fashionevalue; break;
		}
		if (Mathf.Abs(health - healthPending) >= 10f)
		{
			StartCoroutine(HealthSlide());
		}
		gradiHealthbarCheck();
		
	}
	public void gradiHealthbarCheck()
	{
		barcolo.color = gradi.Evaluate(health / maxHealth);
	}
	#endregion

	#region Triggers & Game Events
	private void OnTriggerEnter(Collider other)
	{
		if (!jumpRope)
		{

			if (other.transform.name == "Playtime" & !jumpRope & playtime.playCool <= 0f)
			{
				if (!invisi && !invisichalk || !invisichalk && !invisi)
				{
					ActivateJumpRope();
				}
			}
			else if (other.transform.name == "MemeMouse" & !jumpRope & playtime.playCool <= 0f)
			{
				if (!invisi && !invisichalk || !invisichalk && !invisi)
				{
					ActivateJumpRope();
				}
			}
		}

		if (other.name == "OfficeTrigger")
		{
			alsoInOffice = true;
		}
		if (other.CompareTag("porta"))
		{
			StartCoroutine(GameControllerScript.Instance.funnyportal());
		}
		if (other.CompareTag("lapporta"))
		{
			StartCoroutine(GameControllerScript.Instance.LapManag.LapPortal());
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.transform.name == "Wanderer" & !gc.debugMode & !titlecard)
		{
			SetHP(HealthChangeMode.Remove, 40 / PlayerDmgResistance, 1f, false, true, false);
		}
		if (other.transform.name == "Gotta Sweep")
		{
			sweeping = true;
			sweepingFailsave = 1f;
		}
		if (other.transform.name == "1945 tom")
		{
			sweeping = true;
			sweepingFailsave = 1f;
		}
		else if (other.transform.name == "1st Prize" & firstPrize.velocity.magnitude > 5f)
		{
			hugging = true;
			sweepingFailsave = 1f;
		}
		else if (other.transform.name == "washingmachine" & firstPrize.velocity.magnitude > 5f)
		{
			hugging = true;
			sweepingFailsave = 1f;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.name == "Gotta Sweep")
		{
			sweeping = false;
		}
		if (other.transform.name == "1945 tom")
		{
			sweeping = false;
		}
		else if (other.transform.name == "1st Prize")
		{
			hugging = false;
		}
		else if (other.transform.name == "washingmachine")
		{
			hugging = false;
		}
		if (other.name == "OfficeTrigger")
		{
			alsoInOffice = false;
			ResetGuilt("escape", door.lockTime);
		}
	}

	public IEnumerator KeepTheHudOff()
	{
		while (gameOver)
		{
			hud.SetActive(false);
			if (GameObject.Find("JumpRope(Clone)") != null)
			{
				GameObject.Find("JumpRope(Clone)").SetActive(false);
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	public void ActivateJumpRope()
	{
		playtime.audioDevice.PlayOneShot(playtime.aud_ReadyGo);
		//UIPopupTextManagerWithMovement.Show("MemeBoiLines_2", Color.red, playtime.eeek.transform, playtime.aud_ReadyGo.length, 0f);
		GameSet = Instantiate(jumpRopeGame);
		GameSet.GetComponent<JumpRopeScript>().cs = CamCam;
		CamCam.jumpHeight = 0f;
		GameSet.GetComponent<JumpRopeScript>().ps = this;
		GameSet.GetComponent<JumpRopeScript>().playtime = playtime;
		jumpRope = true;
		frozenPosition = transform.position;
	}

	public void DeactivateJumpRope()
	{
		if (GameSet != null)
		{
			Destroy(GameSet);
			GameSet = null;
		}
		jumpRope = false;
	}

	public async void ActivateBoots()
	{
		bootsActive = true;
		await Task.Delay(60000);
		bootsActive = false;
	}
	#endregion

	public bool IsInDistance(Transform target, float distance) => Vector3.Distance(transform.position, target.transform.position) < distance;

	public bool CanInteract(Transform target)
	{
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, LocalRange))
		{
			if (raycastHit.collider.transform == target)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	#region Serialized Fields
	[Header("References")]
	public GameControllerScript gc;
	[SerializeField] public DoorScript door;
	public PlaytimeScript playtime;
	[SerializeField] public CharacterController cc;
	[SerializeField] private NavMeshAgent gottaSweep, firstPrize;
	[SerializeField] private Transform firstPrizeTransform, PlayerTransform;
	public Transform targetToForcelyLookAt;
	[SerializeField] private GameObject jumpRopeGame, hud;
	[SerializeField] private Material blackSky;
	[SerializeField] private CameraScript CamCam;
	[SerializeField] private float staminaRate;
	private int audVal;

	[Header("Staminometer References")]
	[SerializeField] private Slider staminaBar1;
	[SerializeField] private Slider staminaBar2, staminaBar3, staminaBar4, staminaBar5;
	[Header("healthstuff Refrences")]
	[SerializeField] private Slider healthbar;
	[SerializeField] private Image barcolo;
	[SerializeField] private Gradient gradi;
	[Header("totem Refrences")]
	public Animator TotemAnimator;
	public GameObject totemParticl;
	[Header("Movement Settings")]
	[SerializeField] private float jumpRopeSpeedMultiplier;
	public bool isForcedToLook, invisi, invisichalk;
	public float walkSpeed, DefaultWalkSpeed, runSpeed, DefaultRunSpeed, maxStamina, health, maxHealth, forceLookSpeed;
	[Header("Movement & Stamina Multipliers")]
	public float walkSpeedMultipler = 1f;
	public float runSpeedMultipler = 1f, staminaDropMultiple = 1f, staminaRiseMultiple = 1f;

	[Header("Stamina & Player Settings")]
	public bool sweeping;
	public bool breakwindow, train, titlecardtotem;
	public int principalBugFixer;
	public string guiltType;
	public float stamina, height, sweepingFailsave, staminaPending, healthPending, slideSpeed, healthslideSpeed, staminaDrop, DefaultstaminaDrop, staminaRise, DefaultstaminaRise, LocalRange, defaultlocalRange, Iframes, PlayerDmgResistance;
	public bool gameOver, jumpRope, hugging, isSliding, hpisSliding, bootsActive, alsoInOffice, movementLocked, killedbybaldi, killedbyfamished, outdoorsfr, IgnoreHpLimit, titlecard;

	[Header("Private Variables")]
	[SerializeField] private Vector3 frozenPosition;
	[SerializeField] private float mouseSensitivity, gravity;
	public float guilt;
	public Transform dropItemPos;
	#endregion

	#region Internal State
	private Quaternion playerRotation;
	private bool sensitivityActive;
	private float sensitivity, playerSpeed;
	private Vector3 moveDirection, secondaryMovementVelocity;
	private GameObject GameSet;

	public enum StaminaChangeMode { Add, Remove, Multiply, Divide, Set }
	public enum HealthChangeMode { Add, Remove, Multiply, Divide, Set }
	#endregion
}