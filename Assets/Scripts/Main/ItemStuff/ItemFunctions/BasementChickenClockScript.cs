using UnityEngine;
using TMPro;
using System;

public class BasementChickenClockScript : MonoBehaviour
{
    #region Per-Frame Logic
    private void Update()
    {
        if (timeLeft.CountdownWithDeltaTime() != 0)UpdateDisplay();
        else if (!rang) TriggerAlarm();
        if (lifeSpan.CountdownWithDeltaTime() == 0) Destroy(gameObject);

        if (Time.timeScale != 0 && trigger.ScreenRaycastMatchesCollider(out _, GameControllerScript.Instance.player.LocalRange,KeyFunctions.hi.PlayerClickablesLayer.value) && !rang && (Input.GetMouseButtonDown(0) || Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact)))
        {
            CyclePreset();
        }
    }
    #endregion

    #region Alarm Control
    private void CyclePreset()
    {
        currentPreset = (currentPreset + 1) % timePresets.Length;
        timeLeft = timePresets[currentPreset];
        lifeSpan = timePresets[currentPreset] + 5;
        windAud.ClearQueue(true);
        windAud.PlaySingleClip(wind);
    }

    private void TriggerAlarm()
    {
        rang = true;
        gameObject.tag = "Untagged";
        Singleton<OtherMainStuffManager>.Instance.HearingShit(12f, this.transform, new Vector3(0f,0f,0f), "all",false);
        CameraScript.Instance.TempShakeAmount += 2f;
        foreach (NPC ennPeeCee in FindObjectsOfType<NPC>())
		{
            if (Vector3.Distance(transform.position, ennPeeCee.transform.position) <= 100)
            {
                ennPeeCee.DrainHp(50);
                ennPeeCee.Stun(10);
            }
		}
        foreach (basicshowWindowScript w in FindObjectsOfType<basicshowWindowScript>()) 
        {
            if (!w.broken) if (Vector3.Distance(transform.position, w.transform.position) <= 100) w.SetWindowState(true, 6f, 0f, 2);
        }
        audioDevice.ClearQueue(true);
        audioDevice.PlaySingleClip(ring);
    }

    private void UpdateDisplay()
    {
        var index = Mathf.Clamp((int)(timeLeft / 5), 0, 11);
        currentPreset = index;
        TimeText.text = $"{Math.Round(timeLeft, 1)}";
    }
    #endregion

    #region Serialized Configuration
    [Header("Audio")]
    [SerializeField] private AudioObjectyeah ring;
    [SerializeField] private AudioObjectyeah wind;
    [SerializeField] private AudioManagerLiveReaction audioDevice,windAud;
    [Header("Settings")]
    [SerializeField] private int[] timePresets = { 15, 30, 45, 60 };
    [SerializeField] private float timeLeft;
    [SerializeField] private float lifeSpan;

    [Header("Visuals")]
    [SerializeField] private SphereCollider trigger;
    #endregion

    #region Internal State
    [HideInInspector] public bool rang;
    public int currentPreset;
    public TMP_Text TimeText;
    #endregion
}