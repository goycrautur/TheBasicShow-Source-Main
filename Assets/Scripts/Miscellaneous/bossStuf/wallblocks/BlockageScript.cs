using UnityEngine;
using System;

public class BlockageScript : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles,colliders;

    public int colliderId;

    public bool active;
	
	private float coolDown;
	
	private int index;
	private void Update()
	{
        if (active && Vector3.Distance(GameControllerScript.Instance.player.transform.position, base.transform.position) >= 120 && coolDown <= 0f)
		{
			active = false;
			for (int j = 0; j < obstacles.Length; j++)
			{
				obstacles[j].SetActive(false);
			}
			coolDown = 40f;
		}
        if (coolDown > 0)
		{
			coolDown -= Time.deltaTime;
		}
    }
	
    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && coolDown <= 0 && !active && UnityEngine.Random.Range(0, ZerullClassic.Instance.health) > ZerullClassic.Instance.health - (obstacles.Length+1))
		{
			active = true;
			index = UnityEngine.Random.Range(0, obstacles.Length);
			do
			{
				index = UnityEngine.Random.Range(0, obstacles.Length);
			}
			while (index == colliderId);
			obstacles[index].SetActive(true);
        }
	}
}
