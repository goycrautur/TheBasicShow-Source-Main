using System;
using UnityEngine;

public class NumBallScript : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static NumBallScript Instance;
    #endregion
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        OldTran = base.transform.position;
        OldTran = new Vector3(OldTran.x, 5f, OldTran.z);
        targetPosition = base.transform.position + new Vector3(0f, -2f, 0f);
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    public void die()
    {
        if (this.isActiveAndEnabled)
        {
            Instantiate(GameControllerScript.Instance.popparti, transform.position, Quaternion.identity);
        }
    }
    private void Update()
    {
        if (Vector3.Distance(base.transform.position, mathMachine.transform.position) >= GameControllerScript.Instance.player.LocalRange)
        {
            base.transform.position = Vector3.Lerp(base.transform.position, base.transform.position, Time.deltaTime);
        }
        if (Input.GetMouseButtonDown(0) && !IsPickup)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f));
            GameObject x = null;
            Transform x2 = null;
            float num = GameControllerScript.Instance.player.LocalRange;
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, GameControllerScript.Instance.player.LocalRange) && raycastHit.transform.tag == "Item")
            {
                x = raycastHit.transform.gameObject;
                float distance = raycastHit.distance;
                x2 = raycastHit.transform;
            }
            if (x != null && x == base.gameObject && x2 == base.transform && (raycastHit.transform.gameObject.Equals(base.transform.gameObject) & Vector3.Distance(player.position, base.transform.position) < GameControllerScript.Instance.player.LocalRange))
            {
                mathMachine.Pickup(this.num, base.gameObject);
                IsPickup = true;
            }
        }
        if (IsPickup)
        {
            base.GetComponent<balloonFloatScript>().enabled = false;
            base.GetComponent<CapsuleCollider>().enabled = false;
            Vector3 forward = player.forward;
            Vector3 position = player.position + forward * 4f - new Vector3(0f, 2f, 0f);
            base.transform.position = position;
            Quaternion rotation = Quaternion.LookRotation(player.position - base.transform.position);
            base.transform.rotation = rotation;
        }
        else
        {
            base.GetComponent<balloonFloatScript>().enabled = true;
            base.GetComponent<CapsuleCollider>().enabled = true;
        }
        if (!IsPickup)
        {
            Ray ray2 = Camera.main.ScreenPointToRay(new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f));
            GameObject x3 = null;
            Transform x4 = null;
            float num2 = GameControllerScript.Instance.player.LocalRange;
            RaycastHit raycastHit2;
            if (Physics.Raycast(ray2, out raycastHit2, GameControllerScript.Instance.player.LocalRange) && raycastHit2.transform.tag == "Item")
            {
                x3 = raycastHit2.transform.gameObject;
                float distance2 = raycastHit2.distance;
                x4 = raycastHit2.transform;
            }
            if (x3 != null && x4 == base.transform)
            {
                if (raycastHit2.transform.gameObject.Equals(base.transform.gameObject) & Vector3.Distance(player.position, base.transform.position) < GameControllerScript.Instance.player.LocalRange)
                {
                    targetPosition = new Vector3(base.transform.position.x, 2f, base.transform.position.z);
                    base.transform.position = Vector3.MoveTowards(base.transform.position, targetPosition, lerpSpeed * Time.deltaTime);
                    return;
                }
                base.transform.position = Vector3.MoveTowards(base.transform.position, new Vector3(base.transform.position.x, 5f, base.transform.position.z), lerpSpeed * Time.deltaTime);
                return;
            }
            else
            {
                base.transform.position = Vector3.MoveTowards(base.transform.position, new Vector3(base.transform.position.x, 5f, base.transform.position.z), lerpSpeed * Time.deltaTime);
            }
        }
    }

    public void DownBall()
    {
        if (IsPickup)
        {
            Vector3 forward = player.forward;
            Vector3 position = player.position + forward * -4f;
            base.transform.position = position;
            base.GetComponent<balloonFloatScript>().enabled = true;
            base.GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            base.transform.position = OldTran;
        }
        IsPickup = false;
    }
    private Transform player;
    private Vector3 OldTran;
    public int num,RoomID;
    private float lerpSpeed = 20f;
    [SerializeField]
    private MathMachineScript mathMachine;
    private Vector3 targetPosition;
    public bool IsPickup, popped, subtitletrigger;
    public AudioClip pop;
}

