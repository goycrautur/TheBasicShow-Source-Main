using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start() => offset = transform.position - player.transform.position;

    private void Update()
    {
        if (!ps.gc.KF.gamePaused)
        {
            lookBehind = Singleton<InputManager>.Instance.GetActionKey(InputAction.LookBehind) ? 180 : 0;
        }
        jumpfloatThing = player.transform.position.y;
        if(TempShakeAmount > 0f)
        {
			TempShakeAmount -= Time.deltaTime;
        }
		else
        {
			TempShakeAmount = 0f;
        }
    }

    private void LateUpdate()
    {
        Vector3 shake = new Vector3(Random.Range(-ShakeAmount + -TempShakeAmount, ShakeAmount + TempShakeAmount), Random.Range(-ShakeAmount + -TempShakeAmount, ShakeAmount + TempShakeAmount), Random.Range(-ShakeAmount + -TempShakeAmount, ShakeAmount + TempShakeAmount));
        if (!ps.gameOver)
        {
            transform.SetPositionAndRotation(new Vector3(player.transform.position.x, jumpfloatThing,player.transform.position.z) + offset + shake + Vector3.zero, player.transform.rotation * Quaternion.Euler(0f, lookBehind, 0f));
        }
        else
        {
            if (ps.killedbybaldi)
            {
                transform.position = baldi.position + baldi.forward * 2f + Vector3.up * GameOverOffset;
                transform.LookAt(baldi.position + Vector3.up * 5f);
            }
            if (ps.killedbyfamished)
            {
                transform.position = famished.position + baldi.forward * 2f + Vector3.up * GameOverOffset;
                transform.LookAt(famished.position + Vector3.up * 5f);
            }
            if (ps.killedbyhim)
            {
                transform.position = him.position + baldi.forward * 16f + Vector3.up * GameOverOffset;
                transform.LookAt(him.position + Vector3.up * 5f);
            }
        }
    }

    [Header("Player References")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerScript ps;

    [Header("Camera Settings")]
    [SerializeField] private float GameOverOffset;
    public Vector3 offset;
    public Camera MainCamera,XrayCamera;

    [Header("Jump Rope Physics")]
    [SerializeField] private float initVelocity, velocity, gravity;
    public float jumpfloatThing = 4.88f;
    public float ShakeAmount;
	public float TempShakeAmount;
    

    [Header("Baldi References")]
    [SerializeField] private Transform baldi;
    [SerializeField] private Transform famished, him;
    
    private int lookBehind;
    private Vector3 jumpHeightV3;
}