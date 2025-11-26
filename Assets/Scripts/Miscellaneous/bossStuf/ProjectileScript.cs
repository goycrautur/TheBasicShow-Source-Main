using System;
using System.Collections;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    private Transform cameraTransform;

    private Vector3 initSpawnPoint;

    private bool pickedUp,boshit,thrown;

    [SerializeField]
    private AudioSource audioDevice;

    [SerializeField]
    private AudioClip throwSound;
    [SerializeField]
    private SpriteRenderer theSpriteREND;
    [SerializeField]
    private subsScriptableObject subtitlesScriptableObjectREAL;

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
                if (ZerullClassic.Instance.playSoundWhenProjectileThrown && audioDevice != null)
                {
                    audioDevice.PlayOneShot(throwSound);
                    if (subtitlesScriptableObjectREAL != null)
                    {
                    GameControllerScript.Instance.SubsManager.summonLeSubtitle(subtitlesScriptableObjectREAL.subtitleOption, subtitlesScriptableObjectREAL, 0, audioDevice);
                    }
                }
                Throw();
            }
        }
        if (thrown)
        {
            transform.position += transform.forward * ProjectileSpeed * Time.deltaTime;
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0f)
            {
                Respawn();
            }
            foreach (WindowScript w in FindObjectsOfType<WindowScript>())
		    {
			    if (!w.broken)
			    {
				    if (Vector3.Distance(this.transform.position, w.transform.position) <= 4)
				    {
				    	w.Window(true, true, 6f);
				    }
			    }
		    }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<ZerullBossScript>() != null && thrown && !boshit)
        {
            StartCoroutine(StunBoss());
            IEnumerator StunBoss()
            {
                if (GetComponent<MeshRenderer>() != null)
                {
                    GetComponent<MeshRenderer>().enabled = false;
                }
                if (GetComponent<SpriteRenderer>() != null)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                while (ZerullClassic.Instance.maxHealth == ZerullClassic.Instance.health-1 && !ZerullClassic.Instance.realBossStarted && ZerullClassic.Instance.GetBoss().hitted || ZerullClassic.Instance.isbroyapping)
                {
                    yield return null;
                }
                boshit = true;
                ZerullClassic.Instance.objects -= 1;
                ZerullClassic.Instance.OnHit(ZerullClassic.Instance.zs.hit.length,projectileDamage);
                Destroy(base.gameObject);
            }
        }
        if (other.tag == "Player" && !pickedUp & !thrown && ZerullClassic.Instance.currentProjectile == null)
            {
                if (GetComponent<Billboard>() != null)
                {
                    wasBillboard = true;
                    Destroy(GetComponent<Billboard>());
                }
                ZerullClassic.Instance.currentProjectile = base.gameObject;
                if (theSpriteREND != null)
                {
                    theSpriteREND.color = new Color(1f, 1f, 1f, 0.5f);
                }
                pickedUp = true;
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
        audioDevice.mute = false;
        if (theSpriteREND != null)
        {
            theSpriteREND.color = new Color(1f, 1f, 1f, 1f);
        }
        thrown = true;
        transform.position = cameraTransform.position;
        transform.rotation = cameraTransform.rotation;
        pickedUp = false;
        ZerullClassic.Instance.currentProjectile = null;
    }

    private void Respawn()
    {
        audioDevice.mute = true;
        audioDevice.Stop();
        thrown = false;
        pickedUp = false;
        lifeSpan = 10f;
        transform.rotation = Quaternion.identity;
        transform.position = initSpawnPoint;
        if (wasBillboard)
        {
            gameObject.AddComponent<Billboard>();
            wasBillboard = false;
        }
    }

    private bool wasBillboard;
}