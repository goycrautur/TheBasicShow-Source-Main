using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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
		barcolo.color = gradi.Evaluate(health / maxHealth);
		ApplyGravity();
		HandleMouseMovement();
		if (!movementLocked) PlayerMove();
		GuiltCheck();
		InitializeMiscellaneous();
		randomAssStufV2();
	}
	#endregion
	public void randomAssStufV2()
    {
		titlecardtotem = false;
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
			
			foreach (JumpRopeScript jumpro in jumpropes)
        	{
            	if (jumpro != null)
				{
					jumpro.End(false);
				}
        	}
			HudManager.Instance.colorVarSetter(false);
			if (ZerullClassic.Instance.RealBossStarted || FamishedModeController.Instance.OneBounceFamis)
			{
				SetHP(HealthChangeMode.Add, 0.05f, 0f, true, false);
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
					if (Vector3.Distance(PlayerTransform.position, w.transform.position) <= windowbreakDistance)
					{
						w.Window(true, true, 6f);
					}
				}
			}
		}
		if (door.lockTime > 0f)
		{
			ResetGuilt("escape", 1f);
		}
    }
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
			grav.y -= gravity * Time.deltaTime * curgrav;
			curgrav += 0.02f;
		}

		cc.Move(grav * Time.deltaTime);
		if (cc.isGrounded)
		{
			curgrav = 1f;
		}
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
		if (DisableCamMove) return;
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
		isMoving = false;
		Vector3 movement = Vector3.zero;
		Vector3 lateralMovement = Vector3.zero;

		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveForward))
		{
			movement = transform.forward;
			isMoving = true;
		}
		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveBackward))
		{
			movement = -transform.forward;
			isMoving = true;
		}
		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveLeft))
		{
			lateralMovement = -transform.right;
			isMoving = true;
		}
		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveRight))
		{
			lateralMovement = transform.right;
			isMoving = true;
		}

		AdjustSpeedAndSensitivity(movement, lateralMovement);
		HandleSpecialMovement();
		cc.Move(moveDirection + pModManag.Addend);
		secondaryMovementVelocity = new Vector3(cc.velocity.x, 0f, cc.velocity.z);
	}

	private void AdjustSpeedAndSensitivity(Vector3 movement, Vector3 lateralMovement)
	{
		bool isRunning = Singleton<InputManager>.Instance.GetActionKey(InputAction.Run) && stamina > 0f;
		if (!OverridePlayerSpeed) playerSpeed = isRunning ? runSpeed : walkSpeed;
		sensitivity = sensitivityActive ? Mathf.Clamp((movement + lateralMovement).magnitude, 0f, 1f) : 1f;
		moveDirection = (movement + lateralMovement).normalized * playerSpeed * pModManag.Multiplier * sensitivity * Time.deltaTime;

		if (isRunning && secondaryMovementVelocity.magnitude > 0.1f && !hugging && !sweeping)
		{
			if (!outdoorsfr)
			{
				if (door.lockTime <= 0f)
				{
				ResetGuilt("running", 0.1f);
				}
			}
		}
	}

	private void HandleSpecialMovement()
	{
		
		if (sweeping && !bootsActive)
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
			guilt -= Time.deltaTime * 2;
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

		killedbybaldi = false;
		killedbyfamished = false;
		killedbyhim = false;
		
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
			lowBudgetAudioManagementShit.Instance.HurtSource.PlayOneShot(GameControllerScript.Instance.HurtSounds[audVal]);
		}
		switch (mode)
		{
			case HealthChangeMode.Add: 
				health += fashionevalue; 
				break;
			case HealthChangeMode.Remove: 
				scoreSystemManager.Instance.AddScore(-10*(int)fashionevalue,true);
				health -= fashionevalue; 
				break;
			case HealthChangeMode.Multiply: health *= fashionevalue;break;
			case HealthChangeMode.Divide: 
				scoreSystemManager.Instance.AddScore(-10*(int)fashionevalue,true);
				if (fashionevalue != 0f) health /= fashionevalue; 
				break;
			case HealthChangeMode.Set: health = fashionevalue; break;
		}
		
		if (Mathf.Abs(health - healthPending) >= 10f)
		{
			StartCoroutine(HealthSlide());
		}
	}
	#endregion

	#region Triggers & Game Events
	private void OnTriggerEnter(Collider other)
	{
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
		else if (other.transform.name == "1st Prize" & firstPrize.velocity.magnitude > 5f && other.transform.GetComponent<FirstPrizeScript>().crazyTime <= 0)
		{
			hugging = true;
			sweepingFailsave = 1f;
		}
		else if (other.transform.name == "washingmachine" & firstPrize.velocity.magnitude > 5f && other.transform.GetComponent<FirstPrizeScript>().crazyTime <= 0)
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
		}
	}

	public IEnumerator KeepTheHudOff()
	{
		while (gameOver)
		{
			hud.SetActive(false);
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	public async void ActivateBoots(int time = 60000,bool isboots = true)
	{
		if (isboots) isactuallyusingboots = true;
		bootsActive = true;
		await Task.Delay(time);
		if (isboots) isactuallyusingboots = false;
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
	[SerializeField] public CharacterController cc;
	[SerializeField] private NavMeshAgent gottaSweep, firstPrize;
	[SerializeField] private Transform firstPrizeTransform;
	public Transform PlayerTransform,targetToForcelyLookAt;
	public GameObject hud;
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
	[SerializeField] private Gradient gradi,KarmaGradi;
	
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
	public bool breakwindow, train, titlecardtotem,isactuallyusingboots,OverridePlayerSpeed,OverridePlayerRange;
	public int principalBugFixer;
	public string guiltType;
	public float stamina, height, sweepingFailsave, staminaPending, healthPending, slideSpeed, healthslideSpeed, staminaDrop, DefaultstaminaDrop, staminaRise, DefaultstaminaRise, LocalRange, defaultlocalRange, Iframes, PlayerDmgResistance, windowbreakDistance = 20f;
	public bool gameOver, hugging, isSliding, hpisSliding, bootsActive, alsoInOffice, movementLocked, killedbybaldi, killedbyfamished, killedbyhim, outdoorsfr, IgnoreHpLimit, titlecard, isMoving,DisableCamMove;
	public Vector3 frozenPosition;

	[Header("Private Variables")]
	[SerializeField] private float mouseSensitivity, gravity;
	public float guilt;
	public Transform dropItemPos;
	#endregion

	#region Internal State
	private Quaternion playerRotation;
	private bool sensitivityActive;
	public float sensitivity, playerSpeed,curgrav = 1f;
	private Vector3 moveDirection, secondaryMovementVelocity;
	private GameObject GameSet;

	public enum StaminaChangeMode { Add, Remove, Multiply, Divide, Set }
	public enum HealthChangeMode { Add, Remove, Multiply, Divide, Set }
	public List<JumpRopeScript> jumpropes = new List<JumpRopeScript>();
	public PlayerModifiers pModManag;
	#endregion
}