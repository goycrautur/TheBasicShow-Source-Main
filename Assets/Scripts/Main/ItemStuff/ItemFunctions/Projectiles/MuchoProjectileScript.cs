using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MuchoProjectileScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        ChangeProjectileColor(Random.Range(0,3));
        if (shouldRotate)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = Mathf.Round(Random.Range(0f, 359f));
            transform.eulerAngles = eulerAngles;
        }

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    public void ChangeProjectileColor(int val = 0)
    {
        projectileColor = val == 0 ? "White" : val == 1 ? "Orange" : val == 2 ? "Blue" : "White";
        SpriteRend.color = colorShit[val == 0 ? 0 : val == 1 ? 1 : val == 2 ? 2 : 0];
        if (spriteRend2 != null)
        {
            spriteRend2.color = colorShit[val == 0 ? 0 : val == 1 ? 1 : val == 2 ? 2 : 0];
        }
    }
    #endregion
    private void OnTriggerStay(Collider proj)
    {
        if (proj.CompareTag("Window") && proj.GetComponent<basicshowWindowScript>() != null && !proj.GetComponent<basicshowWindowScript>().broken && projectileColor != "Blue")
        {
            proj.GetComponent<basicshowWindowScript>().SetWindowState(true, 6f, 0f, 1);
            Destroy(gameObject, 0f);
            return;
        }
        if (proj.CompareTag("Player"))
        {
            if (!GameControllerScript.Instance.debugMode & !GameControllerScript.Instance.player.titlecard)
            {
                if (projectileColor == "White")
                {
                    switch (projectileTypes)
                    {
                        case ProjectileTypesREAL.IgnoreIFrames:
                            GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, DamageAmmount / GameControllerScript.Instance.player.PlayerDmgResistance, 0.025f, false, true, false);
                            break;
                        case ProjectileTypesREAL.hammer:
                            GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, DamageAmmount / GameControllerScript.Instance.player.PlayerDmgResistance, 0, false, true, false);
                            Destroy(gameObject, 0f);
                            break;
                    }
                }
                if (projectileColor == "Blue")
                {
                    if (GameControllerScript.Instance.player.isMoving)
                    {
			            switch (projectileTypes)
                        {
                            case ProjectileTypesREAL.IgnoreIFrames:
                                GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, DamageAmmount / GameControllerScript.Instance.player.PlayerDmgResistance, 0.025f, false, true, false);
                                break;
                            case ProjectileTypesREAL.hammer:
                                GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, DamageAmmount / GameControllerScript.Instance.player.PlayerDmgResistance, 0, false, true, false);
                                Destroy(gameObject, 0f);
                                break;
                        }
                    }
                }
                if (projectileColor == "Orange")
                {
                    if (!GameControllerScript.Instance.player.isMoving)
                    {
			            switch (projectileTypes)
                        {
                            case ProjectileTypesREAL.IgnoreIFrames:
                                GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, DamageAmmount / GameControllerScript.Instance.player.PlayerDmgResistance, 0, false, true, false);
                                break;
                            case ProjectileTypesREAL.hammer:
                                GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, DamageAmmount / GameControllerScript.Instance.player.PlayerDmgResistance, 0, false, true, false);
                                Destroy(gameObject, 0f);
                                break;
                        }
                    }
                }
            }
        }
    }
    #region Per-Frame Logic
    private void Update()
    {
        rb.velocity = transform.forward * Random.Range(speed,speedalt);
        lifeSpan -= Time.deltaTime;
        if (lifeSpan < 0f)
        {
            Destroy(gameObject, 0f);
        }
    }
    #endregion

    #region Serialized Configuration
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float speedalt;

    [Header("Lifespan Settings")]
    [SerializeField] private float lifeSpan;

    [Header("Rotation Settings")]
    [SerializeField] private bool shouldRotate;
    [Header("idk")]
    [SerializeField] private float DamageAmmount;
    public ProjectileTypesREAL projectileTypes;
    public enum ProjectileTypesREAL
    {
        IgnoreIFrames,
        DontIgnoreIFrames,
        hammer,
        bobm
    }
    public string projectileColor;
    public Color[] colorShit;
    public SpriteRenderer SpriteRend,spriteRend2;
    
    #endregion
    #region Internal References
    private Rigidbody rb;
    private Vector3 direction;
    #endregion
}