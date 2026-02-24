using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VertexGlitchManager : Singleton<VertexGlitchManager>
{
    private float[] spectrumData = new float[256];

	private float audioSync;

    private float glitchVal;

	private float time = 0f;

	private float GameOverGlitch_Delay = 10f;

	private bool glitchingAlready;

    public AudioSource sourceToSyncIn;

    public float Speed = 1.5f;

    private bool wasGlitch;

    public bool mustGlitch;

	private bool isShakeGlitchUpdating;

    public bool Midi;
    

    public float MidiBTM, minVertexseed = 1f,maxVertexseed = 2f, randomSeedVal;
    public float global_VertexGlitchSeed,global_VertexGlitchIntensity,global_VertexGlitchIntensitySpecialCare;
    public int global_glitchColorRvalue,global_glitchColorGvalue,global_glitchColorBvalue;
#if UNITY_EDITOR
    private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeChanged;

    private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeChanged;

    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode || state == PlayModeStateChange.EnteredEditMode)
        {
            ResetAllMaterials();
        }
    }
#endif
    private void ResetAllMaterials()
    {
        if (GameControllerScript.Instance.targetMaterials == null)
        {
            return;
        }
        if (GameControllerScript.Instance.MaterialsThatNeedSpecialCare == null)
        {
            return;
        }
        global_VertexGlitchIntensity = 0;
        global_VertexGlitchIntensitySpecialCare = 0;
        global_VertexGlitchSeed = 0;
        global_glitchColorRvalue = 0;
        global_glitchColorGvalue = 0;
        global_glitchColorBvalue = 0;
    }
    public void Randomizer()
    {
        int RandomCurId = UnityEngine.Random.Range(-200, 200);
        if (RandomCurId == 0) 
        {
            Randomizer();
            Debug.Log("seed hit the 0");
            return;
        }
        else randomSeedVal = RandomCurId;
    }
	public void Glitch(float vertexseedRandomRangeMin = 1f,float vertexseedRandomRangeMax = 1f,float desiredval = 1f)
	{
		if (isShakeGlitchUpdating)
		{
			return;
		}
		if (glitchVal <= 0f)
		{
			StartCoroutine(UnGlitch(vertexseedRandomRangeMin,vertexseedRandomRangeMax));
		}
        Randomizer();
		glitchVal = desiredval;
		Shader.SetGlobalFloat("_VertexGlitchSeed", UnityEngine.Random.Range(vertexseedRandomRangeMin, vertexseedRandomRangeMax));
		Shader.SetGlobalFloat("_VertexGlitchIntensity", glitchVal);
        global_VertexGlitchIntensity = glitchVal/4;
        global_VertexGlitchIntensitySpecialCare = glitchVal*4;
        global_VertexGlitchSeed = randomSeedVal;
        global_glitchColorRvalue = UnityEngine.Random.Range(0, 32);
        global_glitchColorGvalue = UnityEngine.Random.Range(0, 32);
        global_glitchColorBvalue = UnityEngine.Random.Range(0, 32);
	}
	
	private IEnumerator UnGlitch(float vertexseedRandomRangeMin = 1f,float vertexseedRandomRangeMax = 1.1f)
	{
		yield return null;
		while (glitchVal > 0f)
		{
			glitchVal -= Time.deltaTime * (2f*ZerullClassic.Instance.midiTempo);
            
			Shader.SetGlobalFloat("_VertexGlitchIntensity", glitchVal/2);
            global_VertexGlitchIntensity = glitchVal/4;
            global_VertexGlitchIntensitySpecialCare = glitchVal*4;
            global_VertexGlitchSeed = randomSeedVal;
            global_glitchColorRvalue = UnityEngine.Random.Range(0, 32);
            global_glitchColorGvalue = UnityEngine.Random.Range(0, 32);
            global_glitchColorBvalue = UnityEngine.Random.Range(0, 32);
			yield return null;
		}
		glitchVal = 0f;
		Shader.SetGlobalFloat("_VertexGlitchIntensity", 0f);
        global_VertexGlitchIntensity = 0;
        global_VertexGlitchIntensitySpecialCare = 0;
        global_VertexGlitchSeed = 0;
        global_glitchColorRvalue = 0;
        global_glitchColorGvalue = 0;
        global_glitchColorBvalue = 0;
	}
	
	public void ShakeGlitch()
	{
		GameOverGlitch_Delay = 7f;
		isShakeGlitchUpdating = true;
		time = 0f;
	}

    public void BossStartShake()
    {
		GameOverGlitch_Delay = 3f;
        isShakeGlitchUpdating = true;
        time = 0f;
    }

    private IEnumerator Shake(float vertexseedRandomRangeMin = 1f,float vertexseedRandomRangeMax = 100f)
    {
        audioSync = GetAudioLevel(sourceToSyncIn);
        Glitch(vertexseedRandomRangeMin,vertexseedRandomRangeMax);
        yield return new WaitForSeconds(audioSync / Speed);
        glitchingAlready = false;
    }

    private float GetAudioLevel(AudioSource audioSource)
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

        float sum = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            sum += spectrumData[i];
        }

        float rmsValue = Mathf.Sqrt(sum / spectrumData.Length);
        return Mathf.Clamp01(rmsValue * 10f);
    }

    private IEnumerator ShakeMidi(float vertexseedRandomRangeMin = 1f,float vertexseedRandomRangeMax = 100f)
    {
        Glitch(vertexseedRandomRangeMin,vertexseedRandomRangeMax);
        float random = 128f / UnityEngine.Random.Range(1f, 1.3f);
        yield return new WaitForSeconds(random / MidiBTM);
        glitchingAlready = false;
    }

    private void Update()
	{
        foreach (Material mat in GameControllerScript.Instance.targetMaterials)
        {
            if (mat == null)
            {
                continue;
            }
            mat.SetFloat("_VertexGlitchSeed", global_VertexGlitchSeed);
            mat.SetFloat("_VertexGlitchIntensity", global_VertexGlitchIntensity);
            mat.SetInt("_ValueX", global_glitchColorRvalue);
            mat.SetInt("_ValueY", global_glitchColorGvalue);
            mat.SetInt("_ValueZ", global_glitchColorBvalue);
        }
        foreach (Material mat in GameControllerScript.Instance.MaterialsThatNeedSpecialCare)
        {
            if (mat == null)
            {
                continue;
            }
            mat.SetFloat("_VertexGlitchSeed", global_VertexGlitchSeed);
            mat.SetFloat("_VertexGlitchIntensity", global_VertexGlitchIntensitySpecialCare);
            mat.SetInt("_ValueX", global_glitchColorRvalue);
            mat.SetInt("_ValueY", global_glitchColorGvalue);
            mat.SetInt("_ValueZ", global_glitchColorBvalue);
            
        }
        if (mustGlitch)
        {
            if (!Midi)
            {
                if (sourceToSyncIn != null && sourceToSyncIn.isPlaying && !glitchingAlready)
                {
                    glitchingAlready = true;
                    StartCoroutine(Shake(minVertexseed, maxVertexseed));
                }
            }
            else
            {
                if (!glitchingAlready)
                {
                    glitchingAlready = true;
                    StartCoroutine(ShakeMidi());
                }
            }
        }
        if (isShakeGlitchUpdating)
        {
            time += Time.unscaledDeltaTime;
            if (time >= GameOverGlitch_Delay)
            {
                wasGlitch = true;
                isShakeGlitchUpdating = false;
                time = 0f;
            }
            Shader.SetGlobalFloat("_VertexGlitchIntensity", time * 1f);
            Shader.SetGlobalFloat("_VertexGlitchSeed", UnityEngine.Random.Range(0f, 100f));
            global_VertexGlitchIntensity = time/4f;
            global_VertexGlitchIntensitySpecialCare = time;
            global_VertexGlitchSeed = UnityEngine.Random.Range(0f, 100f);
            global_glitchColorRvalue = UnityEngine.Random.Range(0, 24*(int)time);
            global_glitchColorGvalue = UnityEngine.Random.Range(0, 24*(int)time);
            global_glitchColorBvalue = UnityEngine.Random.Range(0, 24*(int)time);
        }
        else if (wasGlitch)
        {
            wasGlitch = false;
            time = 0f;
        }
    }
}
