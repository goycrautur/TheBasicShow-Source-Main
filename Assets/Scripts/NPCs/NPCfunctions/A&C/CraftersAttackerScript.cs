using UnityEngine;
using System.Collections;

public class CraftersAttackerScript : MonoBehaviour
{
    public void Attack() => StartCoroutine(AttackPlayer());
	public void Update()
    {
        for (int i = 0; i < StunEchoes.Length; i++)
		{
			StunEchoes[i].SetActive(!Teleport);
		}
    }
    
	public IEnumerator AttackPlayer()
	{
		
		float speed = 450f;
		float acceleration = 50f;
		float spinDistance = 5f;
		Vector3 currentAngle = playerTransform.forward;
		float TotalTime = 15f;
		float trueTotalTime = 0f;
		float time = 0f;
        float echoDistance = 0f;
		TotalTime += 5 * LappingOfAsylumController.LapInstance.CurrentLap;
        Transform[] array = echos;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(true);
		}
		float TimeMultiplier = (float)(GameControllerScript.Instance.notebooks/10f);
		float TrueTimeMultiplier = TimeMultiplier <= 1f ? 1 : TimeMultiplier;
		trueTotalTime = TotalTime/TrueTimeMultiplier;
		Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(waneisangry, trueTotalTime);
		time = trueTotalTime;
		while (time > 0f)
		{
			spinDistance += Time.deltaTime * TrueTimeMultiplier;
			currentAngle = Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.up) * currentAngle;
			transform.position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) + currentAngle * spinDistance;
            for (int j = 0; j < echos.Length; j++)
			{
				Vector3 echoAngle = Quaternion.AngleAxis(echoDistance * j * -1f * Time.deltaTime * TrueTimeMultiplier, Vector3.up) * currentAngle;
				echos[j].transform.position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) + echoAngle * (spinDistance + j + 1f);
			}
			speed += acceleration * Time.deltaTime * TrueTimeMultiplier;
            echoDistance += 300f * Time.deltaTime * TrueTimeMultiplier;
			time -= Time.deltaTime;
			if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(trueTotalTime, time);
            }
			yield return null;
		}
		crafters.SetActive(true);
		newGauge.Hide();
        craftersScript.GiveConsequence(Teleport);
		Destroy(gameObject);
		yield break;
	}

    [Header("References & Components")]
    public Transform playerTransform;
    public GameObject crafters;
    public CraftersScript craftersScript;
	[SerializeField] private Sprite waneisangry;
	public bool Teleport;


    [Header("Echoes & Positioning")]
	[SerializeField] private Transform[] echos;
	[SerializeField] private GameObject[] StunEchoes;
}