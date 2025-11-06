using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class WarningScreenScript : MonoBehaviour
{
    private void Start()
    {
        if (WarningType.Length > 0 && displayImage != null)
        {
            if (WarningType[0].usesSprite && !WarningType[0].usesGameObject)
            {
                displayImage.sprite = WarningType[0].sprites;
            }
            if (WarningType[0].usesGameObject && !WarningType[0].usesSprite)
            {
                WarningType[0].gameObjec.SetActive(true);
            }
        }
        Cursor.LockCursor();
        the.GetComponent<Animator>().SetTrigger("yooo");
        if (!DiscordRPC_stuff.current.cantConnect)
        {
            DiscordRPC_stuff.current.UpdateStatus("warning screen", "yum", "van", "the crx");
        }
    }
    private IEnumerator bootass()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Cursor.UnlockCursor();
        SceneManager.LoadScene(BootUp);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Advance();
        }
    }

    private void Advance()
    {
        current++;
        if (current < WarningType.Length)
        {
            if (WarningType[current].usesSprite && !WarningType[current].usesGameObject)
            {
                displayImage.sprite = WarningType[current].sprites;
            }
            if (WarningType[current].usesGameObject && !WarningType[current].usesSprite)
            {
                if (WarningType[current - 1].gameObjec != null)
                {
                    WarningType[current - 1].gameObjec.SetActive(false);
                }
                WarningType[current].gameObjec.SetActive(true);
            }
        }
        else
        if (!clicked)
        {
            {
                clicked = true;
                the.GetComponent<Animator>().SetTrigger("nooo");
                StartCoroutine(bootass());
            }
        }
    }


    [Header("Scene Settings"), SerializeField] private string BootUp;

    [Header("UI References")]
    [SerializeField] public GameObject the;
    [SerializeField] private CursorControllerScript Cursor;
    [SerializeField] private Image displayImage;
    [SerializeField] private warnType[] WarningType;
    [Serializable]
	public class warnType
    {
        public bool usesSprite,usesGameObject;
        public Sprite sprites;
		public GameObject gameObjec;
	}
    private int current = 0;
    private bool clicked;
}
