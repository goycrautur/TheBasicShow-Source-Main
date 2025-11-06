using System;
using UnityEngine;

public class litearllyAnBLockScript : MonoBehaviour
{
	private void Start()
	{
		gc = FindObjectOfType<GameControllerScript>();
	}
	public void PlaceWall()
	{
		MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>(true);
		int num = UnityEngine.Random.Range(0, wallTypes.Length);
		MeshRenderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material = wallTypes[num].mat;
		}
		gameObject.GetComponent<AudioSource>().PlayOneShot(wallTypes[num].audio);
	}

	private void Update()
	{
		lifeSpan -= Time.deltaTime;
		if (lifeSpan < 0f)
		{
			Destroy(base.gameObject, 0f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "_DoorOut")
		{
			gc.audioDevice.PlayOneShot(NuhUH);
			ItemManager.Instance.AddItem(ItemManager.Instance.GetItem(1));
			Destroy(gameObject, 0f);
		}
	}

	// Token: 0x04000044 RID: 68
	public float lifeSpan;

	// Token: 0x04000045 RID: 69
	private GameControllerScript gc;

	// Token: 0x04000046 RID: 70
	public AudioClip NuhUH;
	public wallType[] wallTypes;

	// Token: 0x020000E5 RID: 229
	[Serializable]
	public class wallType
	{
		// Token: 0x040006CB RID: 1739
		[SerializeField]
		public Material mat;

		// Token: 0x040006CC RID: 1740
		[SerializeField]
		public AudioClip audio;
	}
}