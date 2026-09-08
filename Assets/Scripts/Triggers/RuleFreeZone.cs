using UnityEngine;

public class RuleFreeZone : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.timeScale != 0f) player.outdoorsfr = true;
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.timeScale != 0f) player.outdoorsfr = true;
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Time.timeScale != 0f) player.outdoorsfr = false;
    }
    private PlayerScript player => GameControllerScript.Instance.player;
}