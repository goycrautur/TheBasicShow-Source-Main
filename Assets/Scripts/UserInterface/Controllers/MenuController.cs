using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
	public void OnEnable()
	{
		uc.firstButton = firstButton;
		uc.SwitchMenu();
	}

	private void Update()//submit live reaction
	{
		if (Input.GetButtonDown("Cancel") && back != null && !isTbsTransisFUCK)
		{
			back.SetActive(true);
			gameObject.SetActive(false);
		}
		if (Input.GetButtonDown("Submit") && main != null)
		{
			misscirclscript.Transition();
			play.ClearQueue(true);
			play.PlaySingleClip(soun);
		}
	}

	[Header("UI Controller")]
	[SerializeField] private UIController uc;

	[Header("Buttons")]
	[SerializeField] private Selectable firstButton;
	[SerializeField] private GameObject back, main, foward;
	public bool isTbsTransisFUCK;
	[SerializeField] private CircleInOutScript misscirclscript;
	public AudioManagerLiveReaction play;
    public AudioObjectyeah soun;
	
}