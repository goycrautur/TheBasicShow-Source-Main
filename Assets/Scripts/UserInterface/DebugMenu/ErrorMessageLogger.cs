using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class ErrorMessageLogger : MonoBehaviour
{
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private AudioManagerLiveReaction errorSource;
    [SerializeField] private AudioObjectyeah errorSound;
    public List<string> textshit = new List<string>();
    private Coroutine logDissappearCoroutine;

    private void OnEnable() => Application.logMessageReceived += LogError;

    private void OnDisable() => Application.logMessageReceived -= LogError;

    private void LogError(string logString, string stackTrace, LogType type)
    {
        //if (type == LogType.Error || type == LogType.Exception)
        //{
            //errorText.color = Color.red;
            if (textshit.Count >= 5) textshit.RemoveAt(0);
            textshit.Add(logString + "\n" + stackTrace + "\n\n");
            string combinedText = "";
            foreach (string tex in textshit) combinedText += tex + "\n";
            errorText.text = combinedText;
            if (!errorSource.audioDevice.isPlaying)errorSource.PlaySingleClip(errorSound);
            if (logDissappearCoroutine != null)StopCoroutine(logDissappearCoroutine);
            logDissappearCoroutine = StartCoroutine(LogDissappear());
        //}
    }

    private IEnumerator LogDissappear()
    {
        yield return new WaitForSeconds(10f);
        float elapsed = 0f;
        Color color = errorText.color;
        color.a = 1f;
        errorText.color = color;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / 1f);
            errorText.color = color;
            yield return null;
        }
        color.a = 0f;
        errorText.color = color;
        logDissappearCoroutine = null;
        yield break;
    }
}