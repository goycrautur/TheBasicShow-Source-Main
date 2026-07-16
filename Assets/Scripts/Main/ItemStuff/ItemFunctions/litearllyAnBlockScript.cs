using System;
using UnityEngine;
public class litearllyAnBlockScript : MonoBehaviour
{
	public AudioManagerLiveReaction audioDeviceMain;
	private void Start()
	{
		okdothething();
	}
	private void Update()
	{
		lifeSpan -= Time.deltaTime;
		if (lifeSpan < 0f)
		{
			Destroy(base.gameObject, 0f);
		}
	}
	private void okdothething()
	{
		MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>(true);
		int num = UnityEngine.Random.Range(0, blockTypes.Length);
		MeshRenderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++) array[i].material = blockTypes[num].mat;
		audioDeviceMain.PlaySingleClip(blockTypes[num].audio);
	}

	public float lifeSpan;
	public GameControllerScript gc;
	public blockType[] blockTypes;
	[Serializable]
	public class blockType
	{
		[SerializeField] public Material mat;
		[SerializeField] public AudioObjectyeah audio;
	}
}