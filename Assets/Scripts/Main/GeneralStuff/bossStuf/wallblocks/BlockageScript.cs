using UnityEngine;
using System;

public class BlockageScript : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles,colliders;

    public int colliderId;

    public bool active;
	public bool inTrigger;
	
	[SerializeField] private float coolDown;
	
	private int index;
	private void Update()
	{
        if (active && coolDown <= 0f)
		{
			active = false;
			for (int j = 0; j < obstacles.Length; j++)obstacles[j].SetActive(false);
			coolDown = 10f;
		}
        if (coolDown > 0 && !inTrigger)coolDown -= Time.deltaTime;
    }
	
    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			inTrigger = true;
			Debug.Log("touche'd le wallblocks");
			if (coolDown <= 0 && !active && UnityEngine.Random.Range(0, ZerullClassic.Instance.health < 11 ? 10 : ZerullClassic.Instance.health) > (ZerullClassic.Instance.health < 11 ? 10 : ZerullClassic.Instance.health) - (obstacles.Length+1))
			{
				coolDown = 10f;
				active = true;
				index = UnityEngine.Random.Range(0, obstacles.Length);
				do
				{
					index = UnityEngine.Random.Range(0, obstacles.Length);
				}
				while (index == colliderId);
				obstacles[index].SetActive(true);
				Debug.Log("wallblock spawned");
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player")) inTrigger = false;
	}
}
