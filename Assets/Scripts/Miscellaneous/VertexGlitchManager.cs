using System.Collections;
using UnityEngine;

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

    public float MidiBTM, minVertexseed = 1f,maxVertexseed = 100f;

	public void Glitch(float vertexseedRandomRangeMin = 1f,float vertexseedRandomRangeMax = 100f,float desiredval = 1f)
	{
		if (isShakeGlitchUpdating)
		{
			return;
		}
		if (glitchVal <= 0f)
		{
			StartCoroutine(UnGlitch());
		}
		glitchVal = desiredval;
		Shader.SetGlobalFloat("_VertexGlitchSeed", Random.Range(vertexseedRandomRangeMin, vertexseedRandomRangeMax));
		Shader.SetGlobalFloat("_VertexGlitchIntensity", glitchVal * 3f);
	}
	
	private IEnumerator UnGlitch()
	{
		yield return null;
		while (glitchVal > 0f)
		{
			glitchVal -= Time.deltaTime * 4f;
			Shader.SetGlobalFloat("_VertexGlitchIntensity", glitchVal * 3f);
			yield return null;
		}
		glitchVal = 0f;
		Shader.SetGlobalFloat("_VertexGlitchIntensity", 0f);
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
        float random = 128f / Random.Range(1f, 1.3f);
        yield return new WaitForSeconds(random / MidiBTM);
        glitchingAlready = false;
    }

    private void Update()
	{
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
            Shader.SetGlobalFloat("_VertexGlitchSeed", Random.Range(0f, 100f));
        }
        else if (wasGlitch)
        {
            wasGlitch = false;
            time = 0f;
        }
    }
}
