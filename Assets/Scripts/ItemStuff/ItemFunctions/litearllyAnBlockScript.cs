using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class litearllyAnBlockScript : MonoBehaviour
{
	public AudioSource audioDevice111,subsAudioDevice;
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
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material = blockTypes[num].mat;
		}
		audioDevice111.PlayOneShot(blockTypes[num].audio);
		GameControllerScript.Instance.SubsManager.summonLeSubtitle(blockTypes[num].subtitleObjectStuff.subtitleOption, blockTypes[num].subtitleObjectStuff, subsAudioDevice);
	}

	public float lifeSpan;
	public GameControllerScript gc;
	public blockType[] blockTypes;
	[Serializable]
	public class blockType
	{
		[SerializeField]
		public Material mat;
		[SerializeField]
		public AudioClip audio;
		[SerializeField]
		public subsScriptableObject subtitleObjectStuff;
	}
}