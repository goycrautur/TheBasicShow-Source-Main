using System;
using System.Collections;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    private Transform cameraTransform;

    private Vector3 initSpawnPoint;

    public bool pickedUp,boshit,thrown;

    [SerializeField]
    private AudioManagerLiveReaction auDevice;

    [SerializeField]
    private AudioObjectyeah throwSound;
    [SerializeField]
    private SpriteRenderer theSpriteREND;
    [SerializeField]
    private float rotateOffset, projectileDamage = 1f, ProjectileSpeed = 30f;

    private float lifeSpan = 10f;

    private void Start()
    {
        initSpawnPoint = base.transform.position;
        cameraTransform = GameControllerScript.Instance.PlayerCamera.transform;
    }

    private void Update()
    {
        
        if (pickedUp)
        {
            transform.localEulerAngles = cameraTransform.localEulerAngles + rotateOffset * Vector3.up;
            if (Input.GetMouseButton(0) || Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact))
            {
                if (ZerullClassic.Instance.playSoundWhenProjectileThrown && auDevice != null) auDevice.PlaySingleClip(throwSound);
                pickedUp = false;
                thrown = true;
                Throw();
            }
        }
        else if (thrown)
        {
            transform.position += transform.forward * ProjectileSpeed * Time.deltaTime;
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0f) Respawn();
            foreach (basicshowWindowScript w in FindObjectsOfType<basicshowWindowScript>()) if (!w.broken) if (Vector3.Distance(this.transform.position, w.transform.position) <= 4) w.SetWindowState(true, 6f, 0f, 0, true, 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<ZerullBossScript>() != null && thrown && !boshit)
        {
            StartCoroutine(StunBoss());
            IEnumerator StunBoss()
            {
                if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false;
                if (GetComponent<SpriteRenderer>() != null) GetComponent<SpriteRenderer>().enabled = false;
                while (ZerullClassic.Instance.maxHealth == ZerullClassic.Instance.health-1 && !ZerullClassic.Instance.realBossStarted && ZerullClassic.Instance.GetBoss().hitted || ZerullClassic.Instance.isbroyapping)
                {
                    yield return null;
                }
                boshit = true;
                ZerullClassic.Instance.OnHit(ZerullClassic.Instance.zs.hit.audClip.length,projectileDamage);
                Destroy(base.gameObject);
            }
        }
        if (other.tag == "Player" && !pickedUp & !thrown)
        {
            if (ZerullClassic.Instance.currentProjectile == null)
            {
                pickedUp = true;
                thrown = false;
                ZerullClassic.Instance.currentProjectile = base.gameObject;
                if (GetComponent<Billboard>() != null)
                {
                    wasBillboard = true;
                    GetComponent<Billboard>().enabled = false;
                }
                if (theSpriteREND != null) theSpriteREND.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }

    private void LateUpdate()
    {
        if (pickedUp)
        {
            transform.position = cameraTransform.position + cameraTransform.forward * 8f;
        }
    }

    private void Throw()
    {
        if (theSpriteREND != null) theSpriteREND.color = new Color(1f, 1f, 1f, 1f);
        ZerullClassic.Instance.objects -= 1;
        transform.position = cameraTransform.position;
        transform.rotation = cameraTransform.rotation;
        if (ZerullClassic.Instance.currentProjectile != null) ZerullClassic.Instance.currentProjectile = null;
        return;
    }

    private void Respawn()
    {
        auDevice.ClearQueue(true);
        thrown = false;
        pickedUp = false;
        lifeSpan = 10f;
        transform.rotation = Quaternion.identity;
        transform.position = initSpawnPoint;
        if (wasBillboard)
        {
            GetComponent<Billboard>().enabled = true;
            wasBillboard = false;
        }
    }

    private bool wasBillboard;
}