using UnityEngine;

public class RuleFreeZone : MonoBehaviour
{
    public void Start() => defaultStamDrainMult = player.staminaDropMultiple;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.timeScale != 0f)
        {
            player.outdoorsfr = true;
            player.staminaDropMultiple = 0;
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
            player.staminaDropMultiple = 0;
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
            player.staminaDropMultiple = defaultStamDrainMult;
        }
    }
    private PlayerScript player => GameControllerScript.Instance.player;
    private float defaultStamDrainMult;
}