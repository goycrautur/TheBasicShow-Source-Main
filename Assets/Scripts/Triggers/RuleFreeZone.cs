using UnityEngine;

public class RuleFreeZone : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.timeScale != 0f)
        {
            player.outdoorsfr = true;
            if (player.stamina <= (player.maxStamina * 1.75f))
			{
				player.stamina += player.staminaRise*1.25f * Time.deltaTime;
			}
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.timeScale != 0f)
        {
            player.outdoorsfr = true;
            if (player.stamina <= (player.maxStamina * 1.75f))
			{
				player.stamina += player.staminaRise*1.25f * Time.deltaTime;
			}
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Time.timeScale != 0f)
        {
            player.outdoorsfr = false;
        }
    }

    [Header("References")]
    [SerializeField] private PlayerScript player;
}