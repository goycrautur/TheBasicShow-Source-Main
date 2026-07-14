using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

public static class Sych
{
    public static bool ScreenCenterRaycast(out RaycastHit hit,int layerMas = Physics.DefaultRaycastLayers,QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal) 
    {
        return Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), out hit,Mathf.Infinity,layerMas,triggerInteraction);
    }
    public static bool ScreenRaycastMatchesTagAndTransform(this Transform target, string tag, out RaycastHit hit, float maxDistance,int layerMas = Physics.DefaultRaycastLayers,QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal) 
    {
        return ScreenCenterRaycast(out hit,layerMas,triggerInteraction) && hit.transform == target && target.IsWithinDistance(maxDistance) && hit.transform.CompareTag(tag);
    }
    public static bool ScreenRaycastMatchesTag(string tag, out RaycastHit hit, float maxDistance,int layerMas = Physics.DefaultRaycastLayers,QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal) 
    {
        return ScreenCenterRaycast(out hit,layerMas,triggerInteraction) && hit.transform.IsWithinDistance(maxDistance) && hit.transform.CompareTag(tag);
    }

    public static bool ScreenRaycastMatchesCollider(this Collider col, out RaycastHit hit, float maxDistance,int layerMas = Physics.DefaultRaycastLayers,QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal) 
    {
        return ScreenCenterRaycast(out hit,layerMas,triggerInteraction) && hit.transform.IsWithinDistance(maxDistance) && hit.collider == col;
    }

    public static bool RaycastFromPosition(this Vector3 origin, Vector3 direction, out RaycastHit hit, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore, int Layermask = -5) 
    {
        return Physics.Raycast(origin, direction, out hit, Mathf.Infinity, Layermask, triggerInteraction);
    }

    public static bool RaycastFromPositionWithDistance(this Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance = Mathf.Infinity, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore, int Layermask = -5) 
    {
        return Physics.Raycast(origin, direction, out hit, maxDistance, Layermask, QueryTriggerInteraction.Ignore);
    }
    

    public static bool IsWithinDistance(this Transform t, float maxDistance) 
    {
        return Vector3.Distance(Camera.main.transform.position, t.position) <= maxDistance;
    }

   public static float CountdownWithDeltaTime(this ref float timer, float incrementAmount = 1f)
    {
        return timer = Mathf.Max(0f, timer - (incrementAmount * Time.deltaTime));
    }

    public static float CountdownWithUnscaledDeltaTime(this ref float timer)
    {
        return timer = Mathf.Max(0f, timer - Time.unscaledDeltaTime);
    }

    public static float IncrementOverTime(this ref float value, float incrementAmount)
    {
        return value += incrementAmount * Time.deltaTime;
    }

    public static int GetRoundedRandomInRange(float min, float max)
    {
        return UnityEngine.Random.Range(Mathf.RoundToInt(min), Mathf.RoundToInt(max));
    }
    public static bool IsWithinDistanceFrom(this Transform t, Transform reference, float maxDistance)
    {
        return (reference.position - t.position).sqrMagnitude <= (maxDistance * maxDistance);
    }

    public static bool IsReadyToMove(this NavMeshAgent agent)
    {
        if (agent == null || !agent.isOnNavMesh) return false;
        return !agent.isStopped && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
    public static void SetCursorLock(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
    public static AudioClip LoadSound(string path) // thanks daldi
    {
        var audioLoader = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.OGGVORBIS);
        audioLoader.SendWebRequest();
        while (!audioLoader.isDone) { }
        var clip = DownloadHandlerAudioClip.GetContent(audioLoader);
        return clip;
    }

    public static AudioClip[] LoadSounds(string folder)
    {
        List<AudioClip> loadedSounds = new List<AudioClip>();
        foreach (var snd in Directory.GetFiles(folder))
        {
            if (!snd.Contains("meta") && snd.Contains("ogg"))
            {
                loadedSounds.Add(LoadSound(snd));
            }
        }

        return loadedSounds.ToArray();
    }

    public static void PlayClip(this AudioSource audioSource, AudioClip clip, bool loop, float volume)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.Play();
    }
    #region Windows API Integration
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SetWindowText(IntPtr hWnd, string lpString);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetActiveWindow();

    public static T GetComponentByTag<T>(string tag) where T : Component
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tag);
        return obj != null ? obj.GetComponent<T>() : null;
    }

    public static void ShowWindowsMessageBox(string message, string title = "Message", uint type = 0x00000000)
    {
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            MessageBox(IntPtr.Zero, message, title, type);
    #else
            Debug.LogWarning("Windows MessageBox is only available on Windows.");
    #endif
    }

    public static void SetGameWindowTitle(string newTitle)
    {
    #if UNITY_EDITOR 
    Debug.Log($"letting you know that the window title DID changed, but you are in the editor, heres the title name tho:'{newTitle}'");
    return;
    #endif
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            IntPtr windowHandle = GetActiveWindow();
            if (windowHandle != IntPtr.Zero)
            {
                SetWindowText(windowHandle, newTitle);
            }
            else
            {
                Debug.LogWarning("Could not get active game window handle.");
            }
    #else
            Debug.LogWarning("SetGameWindowTitle is only available on Windows.");
    #endif
    }
    #endregion
}

public enum MSGBoxType : uint
{
    OK = 0x00000000,
    OKCancel = 0x00000001,
    AbortRetryIgnore = 0x00000002,
    YesNoCancel = 0x00000003,
    YesNo = 0x00000004,
    RetryCancel = 0x00000005,
    CancelTryContinue = 0x00000006,

    IconHand = 0x00000010,
    IconQuestion = 0x00000020,
    IconExclamation = 0x00000030,
    IconAsterisk = 0x00000040,

    DefaultButton1 = 0x00000000,
    DefaultButton2 = 0x00000100,
    DefaultButton3 = 0x00000200,
    DefaultButton4 = 0x00000300,

    SystemModal = 0x00001000,
    TaskModal = 0x00002000,
    Help = 0x00004000,
    SetForeground = 0x00010000,
    DefaultDesktopOnly = 0x00020000,
    TopMost = 0x00040000,
    Right = 0x00080000,
    RTLReading = 0x00100000
}