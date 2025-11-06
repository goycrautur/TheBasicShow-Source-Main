using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VapeSmokeScript : MonoBehaviour
{
    private void OnDestroy()
    {
        if (!GameControllerScript.Instance.player.invisichalk && !GameControllerScript.Instance.player.invisi) return;

        GameControllerScript.Instance.player.invisichalk = false;
    }
    private void insane()
    {

        if (!GameControllerScript.Instance.player.invisichalk && !GameControllerScript.Instance.player.invisi) return;

        if (GameControllerScript.Instance.player.invisichalk && !GameControllerScript.Instance.player.invisi)
        {
            GameControllerScript.Instance.player.invisichalk = false;
        }
    }
    private void OnTriggerStay(Collider smok)
    {
        if (smok.CompareTag("Player"))
        {
            if (!GameControllerScript.Instance.player.invisichalk)
            {
                GameControllerScript.Instance.player.invisichalk = true;
                GameControllerScript.Instance.isHiding = true;

            }
        }
    }
    private void OnTriggerExit(Collider smok)
    {
        if (smok.CompareTag("Player") && GameControllerScript.Instance.player.invisichalk)
        {
            GameControllerScript.Instance.player.invisichalk = false;
            GameControllerScript.Instance.isHiding = false;
        }
    }
    private void Update()
    {   
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 0)
        {
            insane();
            Destroy(base.gameObject);
        }
    }
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private Color ilerpmypants;
    public float timer;
}
