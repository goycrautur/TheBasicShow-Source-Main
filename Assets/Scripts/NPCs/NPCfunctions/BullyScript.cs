using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BullyScript : MonoBehaviour
{
    #region Unity Lifecycle
    private void Start()
    {
        audioDevice = GetComponent<AudioSource>();
        StartCoroutine(WaitAndActivate());
    }
    public void OnEnable()
    {

        GameControllerScript.Instance.buliScr.Add(this);
    }
    public void OnDisable()
    {
        GameControllerScript.Instance.buliScr.Remove(this);
    }

    private void Update()
    {
        if (active)
        {
            activeTime += Time.deltaTime;
            if (activeTime >= 180f)
            {
                Reset();
                if (!audioDevice.isPlaying)
                {
                    audioDevice.PlayOneShot(aud_Bored);
                }
            }
        }

        guilt = Mathf.Max(guilt - Time.deltaTime, 0f);
        foreach (PrincipalScript prin in GameControllerScript.Instance.prinScr)
        {
            if (prin.isActiveAndEnabled)
            {
                prinvec3 = prin.transform.position;
            }
        }
        foreach (MaxcipalScript maxi in GameControllerScript.Instance.maxiScr)
        {
            if (maxi.isActiveAndEnabled)
            {
                maxvec3 = maxi.transform.position;
            }
        }
        foreach (coolSkeleton97Scrip cs97 in GameControllerScript.Instance.cs97Scr)
        {
            if (cs97.isActiveAndEnabled)
            {
                cs97vec3 = cs97.transform.position;
            }
        }
        Vector3 totalVectorShit = prinvec3 + maxvec3 + cs97vec3;
        float distance = Vector3.Distance(totalVectorShit, Obstacle.transform.position);
        Obstacle.enabled = distance >= ignoreDistance;

        extraStuff();
    }

    private void extraStuff()
    {
        if (!active) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        if (distanceToPlayer >= 30f) return;

        if (transform.position.RaycastFromPosition(directionToPlayer, out RaycastHit rch))
        {
            if (rch.collider.CompareTag("Player"))
            {
                if (!spoken)
                {
                    int num = Random.Range(0, aud_Taunts.Length);
                    audioDevice.PlayOneShot(aud_Taunts[num]);
                    spoken = true;
                }
            }
            guilt = 10f;
        }
    }
    #endregion

    #region Activation
    private IEnumerator WaitAndActivate()
    {
        while (true)
        {
            waitTime = Random.Range(10f,2f);
            yield return new WaitForSeconds(waitTime);

            if (!active)
            {
                Activate();
            }
        }
    }

    private void Activate()
    {
        transform.position = wanderer.SetNewTargetForAgent(null, "hall") + Vector3.up * 5f;

        while ((transform.position - player.position).magnitude < 20f)
        {
            transform.position = wanderer.SetNewTargetForAgent(null, "hall") + Vector3.up * 5f;
        }

        active = true;
    }

    private void Reset()
    {
        transform.position += Vector3.down * 20f;
        active = false;
        activeTime = 0f;
        spoken = false;
    }
    #endregion

    #region Player Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Push(player);
            HandlePlayerPush(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.transform.GetComponent<PrincipalScript>() != null && guilt > 0f) || (other.transform.GetComponent<MaxcipalScript>() != null && guilt > 0f))
        {
            Reset();
        }
    }

    private void HandlePlayerPush(Collider playerCollider)
    {
        PlayerScript playerScript = playerCollider.GetComponent<PlayerScript>();
        if (playerScript.bootsActive && ItemManager.Instance.HasNoItems()) return;

        bool hasItems = false;

        for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
        {
            if (ItemManager.Instance.Inventory[i].ItemID != 0)
            {
                hasItems = true;
                break;
            }
        }

        if (!hasItems)
        {
            if (!audioDevice.isPlaying)
            {
                audioDevice.PlayOneShot(aud_Denied);
            }

            Vector3 pushDirection = (playerCollider.transform.position - transform.position).normalized;
            pushCoroutine = StartCoroutine(SmoothPushBack(playerCollider.transform, pushDirection, 16f, 16f / 32f));
        }
        else
        {
            int num = Mathf.RoundToInt(Random.Range(0, ItemManager.Instance.Inventory.Length));

            while (ItemManager.Instance.Inventory[num].ItemID == 0)
            {
                num = Mathf.RoundToInt(Random.Range(0, ItemManager.Instance.Inventory.Length));
            }

            if (ItemManager.Instance.Inventory[num].ItemInstance != null && !ItemManager.Instance.Inventory[num].ItemInstance.unableToGetStealed)
            {
                Destroy(ItemManager.Instance.Inventory[num].ItemInstance.gameObject);
            }

            ItemManager.Instance.ClearItem(num,false);
            ItemManager.Instance.UpdateItemUI();

            int num2 = Random.Range(0, aud_Thanks.Length);
            audioDevice.PlayOneShot(aud_Thanks[num2]);

            Reset();
        }
    }

    public void StopPushingPlayer()
    {
        if (pushCoroutine != null)
        {
            StopCoroutine(pushCoroutine);
            pushCoroutine = null;
        }
    }

    private IEnumerator SmoothPushBack(Transform playerTransform, Vector3 pushDirection, float pushDistance, float duration)
    {
        pushDirection.y = 0f;
        pushDirection.Normalize();

        Vector3 startPosition = playerTransform.position;
        Vector3 targetPosition = startPosition + pushDirection * pushDistance;
        float elapsedTime = 0f;

        if (Physics.Raycast(startPosition, pushDirection, out RaycastHit hit, pushDistance))
        {
            targetPosition = hit.point - pushDirection * 0.1f;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            playerTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        playerTransform.position = targetPosition;
    }
    #endregion

    #region Serialized Field States
    [Header("Player and Movement")]
    [SerializeField] private Transform player;
    [SerializeField] private AILocationSelectorScript wanderer;

    [Header("Timings")]
    [SerializeField] private float waitTime;
    [SerializeField] private float activeTime;

    [Header("Behavior Flags")]
    [SerializeField] private bool active;
    [SerializeField] private bool spoken;

    [Header("Activation and Guilt")]
    [SerializeField] private UnityEngine.AI.NavMeshObstacle Obstacle;
    [SerializeField] private float ignoreDistance;
    [SerializeField] private Vector3 prinvec3,maxvec3,cs97vec3;
    public float guilt;

    [Header("Audio")]
    [SerializeField] private AudioClip[] aud_Taunts = new AudioClip[2];
    [SerializeField] private AudioClip[] aud_Thanks = new AudioClip[2];
    [SerializeField] private AudioClip aud_Denied, aud_Bored;

    private AudioSource audioDevice;
    private Coroutine pushCoroutine;
    #endregion
}