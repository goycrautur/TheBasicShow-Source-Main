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
		
		float speed = 350f;
		float acceleration = 50f;
		float spinDistance = 20f;
		Vector3 currentAngle = playerTransform.forward;
		float TotalTime = 10f;
		float time = 0f;
        float echoDistance = 0f;
		Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(waneisangry, TotalTime);
        Transform[] array = echos;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(true);
		}
		time = TotalTime;
		while (time > 0f)
		{
			currentAngle = Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.up) * currentAngle;
			transform.position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) + currentAngle * spinDistance;
            for (int j = 0; j < echos.Length; j++)
			{
				Vector3 echoAngle = Quaternion.AngleAxis(echoDistance * j * -1f * Time.deltaTime, Vector3.up) * currentAngle;
				echos[j].transform.position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) + echoAngle * (spinDistance + j + 1f);
			}
			speed += acceleration * Time.deltaTime;
            echoDistance += 300f * Time.deltaTime;
			time -= Time.deltaTime;
			if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(TotalTime, time);
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