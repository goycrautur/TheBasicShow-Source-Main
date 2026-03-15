using System.Collections;
using System.Collections.Generic;
using MidiPlayerTK;
using UnityEngine;

public class MusicManagerMaes : Singleton<MusicManagerMaes>
{
	public delegate void MidiEventFunction(MPTKEvent midiEvent);

	public delegate void MidiLoopFunction(string midiName);

	public delegate void MidiTransitionComplete();

	private IEnumerator speedModulator;

	[SerializeField]
	private MidiFilePlayer midiPlayer;

	[SerializeField]
	private MidiFilePlayer reservedPlayer;

	[SerializeField]
	private MidiStreamPlayer midiSynth;

	[SerializeField]
	private AudioSource midiSource;

	[SerializeField]
	private AudioSource synthSource;

	[SerializeField]
	private AudioSource[] fileSoure = new AudioSource[0];

	[SerializeField]
	private AudioSource[] musicChannelSource = new AudioSource[16];

	private List<string> midiQueue = new List<string>();

	private List<string> toQueue = new List<string>();

	private int currentSource;

	private bool otherSourceQueued;

	private bool midiCorrupted;

	private List<MusicFileData> fileQueue = new List<MusicFileData>();

	private List<fluid_voice> heldVoices = new List<fluid_voice>();

	public MPTKEvent testEvent;

	private float speed;

	private bool speedModulatorRunning;

	private bool transitionReady;

	private bool transitionWaiting;

	private bool transitionStarting;

	private bool transitionPlaying;

	private bool fileLoop;

	private bool filePlaying;

	private bool midiPlaying;

	private bool midiPaused;

	public bool bossTransitionWaiting;

	public MidiFilePlayer MidiPlayer
	{
		get
		{
			if (midiPlayer == null)
			{
				Debug.LogError("MusicManager: midiPlayer is null. Make sure it's assigned in the Inspector.");
			}
			return midiPlayer;
		}
	}

	public MidiFilePlayer ReservedPlayer
	{
		get
		{
			if (reservedPlayer == null)
			{
				Debug.LogError("MusicManager: reservedPlayer is null. Make sure it's assigned in the Inspector.");
			}
			return reservedPlayer;
		}
	}

	public bool MidiPlaying => midiPlaying;

	public MidiStreamPlayer MidiSynth
	{
		get
		{
			if (midiSynth == null)
			{
				Debug.LogError("MusicManager: midiSynth is null. Make sure it's assigned in the Inspector.");
			}
			return midiSynth;
		}
	}

	public static event MidiEventFunction OnMidiEvent;

	public static event MidiLoopFunction OnMidiLoop;

	public static event MidiTransitionComplete OnMidiTransitionComplete;

	private void Start()
	{
		// Check if required objects are assigned
		if (midiPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer is not assigned in the Inspector!");
			return;
		}

		if (reservedPlayer == null)
		{
			Debug.LogError("MusicManager: reservedPlayer is not assigned in the Inspector!");
			return;
		}

		if (midiSource == null)
		{
			Debug.LogError("MusicManager: midiSource is not assigned in the Inspector!");
			return;
		}

		if (synthSource == null)
		{
			Debug.LogError("MusicManager: synthSource is not assigned in the Inspector!");
			return;
		}
		SetIgnoreListenerPause();
	}
	public void SetIgnoreListenerPause(bool hi = true)
	{
		midiPlayer.MPTK_KeepNoteOff = hi;
		midiSource.ignoreListenerPause = hi;
		synthSource.ignoreListenerPause = hi;

		if (musicChannelSource != null)
		{
			AudioSource[] array = musicChannelSource;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					array[i].ignoreListenerPause = hi;
				}
			}
		}
		if (fileSoure != null)
		{
			AudioSource[] array = fileSoure;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					array[i].ignoreListenerPause = hi;
				}
			}
		}
	}

	private void Update()
	{
		if (midiPlayer != null && midiSynth != null && midiPlayer.MPTK_IsPlaying)
		{
			if (midiPlayer.Channels != null)
			{
				for (int i = 0; i < midiPlayer.Channels.Length; i++)
				{
					midiSynth.MPTK_ChannelForcedPresetSet(i, midiPlayer.MPTK_ChannelPresetGetIndex(i));
				}
			}
		}
		if (fileQueue.Count > 0)
		{
			if (!otherSourceQueued)
			{
				fileSoure[currentSource].loop = false;
				currentSource = 1 - currentSource;
				fileSoure[currentSource].clip = fileQueue[0].clip;
				fileSoure[currentSource].outputAudioMixerGroup = fileQueue[0].mixer;
				fileQueue.RemoveAt(0);
				fileSoure[currentSource].PlayScheduled((double)fileSoure[1 - currentSource].clip.samples / (double)fileSoure[1 - currentSource].clip.frequency - (double)fileSoure[1 - currentSource].timeSamples / (double)fileSoure[1 - currentSource].clip.frequency + AudioSettings.dspTime);
				otherSourceQueued = true;
			}
			else if (!fileSoure[1 - currentSource].isPlaying)
			{
				otherSourceQueued = false;
			}
		}
		else
		{
			if (fileLoop)
			{
				fileSoure[currentSource].loop = true;
				fileLoop = false;
			}
			if (!fileSoure[currentSource].isPlaying)
			{
				filePlaying = false;
			}
		}
	}

	public void PlayMidi(string song, bool loop)
	{
		// Check if required objects are null
		if (midiPlayer == null || reservedPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer or reservedPlayer is null. Make sure they are assigned in the Inspector.");
			return;
		}

		reservedPlayer.MPTK_Stop();
		HangMidi(stop: false);
		SetSpeed(1f);
		SetTranspose(0);
		midiPlayer.transpose = 0;
		midiPlayer.MPTK_MidiName = song;
		midiPlayer.MPTK_RePlay();
		midiPlaying = true;
		midiPaused = false;

		// Check if Channels and MPTK_Channels are available
		if (midiPlayer.Channels != null && midiPlayer.MPTK_Channels != null)
		{
			for (int i = 0; i < midiPlayer.Channels.Length; i++)
			{
				midiPlayer.MPTK_Channels[i].Enable = true;
			}
		}
		else
		{
			Debug.LogWarning("MusicManager: MIDI player channels are not available yet.");
		}

		SetLoop(loop);
	}

	public void QueueMidi(string song, bool emptyQueue)
	{
		if (emptyQueue)
		{
			midiQueue.Clear();
		}
		toQueue.Add(song);
	}
	public void StartFileQueue()
	{
		if (fileQueue.Count > 0)
		{
			currentSource = 1 - currentSource;
			fileSoure[currentSource].clip = fileQueue[0].clip;
			fileSoure[currentSource].outputAudioMixerGroup = fileQueue[0].mixer;
			fileQueue.RemoveAt(0);
			fileSoure[currentSource].Play();
			filePlaying = true;
		}
	}

	public void StopFile()
	{
		fileSoure[0].Stop();
		fileSoure[1].Stop();
		fileQueue.Clear();
		filePlaying = false;
		fileLoop = false;
	}

	public void PauseMidi(bool pause)
	{
		if (midiPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer is null. Cannot pause/unpause MIDI.");
			return;
		}

		if (midiPlaying)
		{
			if (pause && !midiPaused)
			{
				midiPlayer.MPTK_Pause();
				midiPaused = true;
			}
			else if (!pause && midiPaused)
			{
				midiPlayer.MPTK_UnPause();
				midiPaused = false;
			}
		}
	}

	public void StopMidi()
	{
		if (midiPlayer == null || reservedPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer or reservedPlayer is null. Cannot stop MIDI.");
			return;
		}

		midiPlayer.MPTK_Stop();
		midiPlayer.MPTK_MidiAutoRestart = false;
		StopModulation();
		reservedPlayer.MPTK_Stop();
		midiPlaying = false;
	}

	public void KillMidi()
	{
		StopMidi();
		if (midiPlayer != null && reservedPlayer != null)
		{
			midiPlayer.MPTK_StopSynth();
			reservedPlayer.MPTK_StopSynth();
		}
	}

	public void HangMidi(bool stop)
	{
		HangMidi(stop, keepDrums: false);
	}

	public void HangMidi(bool stop, bool keepDrums)
	{
		// Check if required objects are null
		if (midiPlayer == null || reservedPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer or reservedPlayer is null. Make sure they are assigned in the Inspector.");
			return;
		}

		foreach (fluid_voice heldVoice in heldVoices)
		{
			if (heldVoice != null)
			{
				heldVoice.DurationTick = 0L;
			}
		}
		heldVoices.Clear();

		// Check if MPTK_Channels are available before accessing them
		if (midiPlayer.MPTK_Channels != null && reservedPlayer.MPTK_Channels != null)
		{
			for (int i = 0; i < 16; i++)
			{
				if (i != 9)
				{
					midiPlayer.MPTK_Channels[i].Enable = !stop;
					reservedPlayer.MPTK_Channels[i].Enable = !stop;
				}
				else
				{
					midiPlayer.MPTK_Channels[i].Enable = (!stop || keepDrums);
					reservedPlayer.MPTK_Channels[i].Enable = (!stop || keepDrums);
				}
			}
		}
		else
		{
			Debug.LogWarning("MusicManager: MPTK_Channels are not available yet.");
		}

		if (!stop)
		{
			return;
		}
		if (midiPlayer.ActiveVoices != null)
		{
			foreach (fluid_voice activeVoice in midiPlayer.ActiveVoices)
			{
				activeVoice.DurationTick = -1L;
				heldVoices.Add(activeVoice);
			}
		}
		if (reservedPlayer.ActiveVoices != null)
		{
			foreach (fluid_voice activeVoice2 in reservedPlayer.ActiveVoices)
			{
				activeVoice2.DurationTick = -1L;
				heldVoices.Add(activeVoice2);
			}
		}
	}

	public void SetLoop(bool val)
	{
		if (midiPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer is null. Cannot set loop.");
			return;
		}
		midiPlayer.MPTK_MidiAutoRestart = val;
	}

	public void SetSpeed(float speed)
	{
		if (midiPlayer == null || reservedPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer or reservedPlayer is null. Cannot set speed.");
			return;
		}
		midiPlayer.MPTK_Speed = speed;
		reservedPlayer.MPTK_Speed = speed;
		this.speed = speed;
	}

	public void ModulateSpeed(float rate, float increase, float increaseIncrease, float limit, bool waitForLoop)
	{
		if (speedModulatorRunning)
		{
			StopCoroutine(speedModulator);
		}
		speedModulator = SpeedModulator(rate, increase, increaseIncrease, limit, waitForLoop);
		StartCoroutine(speedModulator);
		speedModulatorRunning = true;
	}

	public void StopModulation()
	{
		if (speedModulatorRunning)
		{
			StopCoroutine(speedModulator);
		}
	}

	public void SetTranspose(int key)
	{
		if (midiPlayer == null || reservedPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer or reservedPlayer is null. Cannot set transpose.");
			return;
		}

		if (key >= -24 && key <= 24)
		{
			midiPlayer.MPTK_Transpose = key;
			reservedPlayer.MPTK_Transpose = key;
		}
	}

	public void StartTransition()
	{
		bossTransitionWaiting = true;
	}

	private IEnumerator SpeedModulator(float rate, float increase, float increaseIncrease, float limit, bool waitForLoop)
	{
		if (midiPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer is null. Cannot modulate speed.");
			yield break;
		}

		speed = midiPlayer.MPTK_Speed;
		while (speed < limit)
		{
			float time = rate;
			while (time > 0f)
			{
				if (!midiPlayer.MPTK_IsPaused)
				{
					time -= Time.deltaTime;
				}
				yield return null;
			}
			speed += increase;
			SetSpeed(speed);
			increase += increaseIncrease;
		}
		speed = limit;
		SetSpeed(limit);
	}

	public void StartExponentialModulator()
	{
		if (speedModulatorRunning)
		{
			StopCoroutine(speedModulator);
		}
		speedModulator = ExponentialModulator();
		StartCoroutine(speedModulator);
		speedModulatorRunning = true;
	}

	private IEnumerator ExponentialModulator()
	{
		float time = 0f;
		while (speed < 10f)
		{
			SetSpeed(speed);
			speed = 0.01f * Mathf.Pow(1.109f, 2f * time) + 0.4f;
			time += Time.deltaTime;
			yield return null;
		}
		speed = 10f;
		SetSpeed(speed);
	}

	public void MidiEvent(List<MPTKEvent> midiEvents)
	{
		if (midiPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer is null. Cannot process MIDI events.");
			return;
		}

		for (int i = 0; i < midiEvents.Count; i++)
		{
			if (MusicManagerMaes.OnMidiEvent != null)
			{
				MusicManagerMaes.OnMidiEvent(midiEvents[i]);
			}
			if (midiEvents[i].Command == MPTKCommand.MetaEvent && midiEvents[i].Meta == MPTKMeta.TextEvent)
			{
				if (!bossTransitionWaiting)
				{
					if (midiEvents[i].Info == "Loop")
					{
						MidiPlayer.MPTK_TickCurrent = 0L;
					}
				}
				else
				{
					midiPlayer.MPTK_TickCurrent = 4992L;
					bossTransitionWaiting = false;
				}
			}
			if (!midiCorrupted)
			{
				continue;
			}
			MPTKEvent mPTKEvent = midiEvents[i];
			midiSynth.MPTK_PlayEvent(mPTKEvent);
		}
		if (toQueue.Count > 0)
		{
			if (reservedPlayer != null)
			{
				midiQueue.AddRange(toQueue);
				toQueue.Clear();
				reservedPlayer.MPTK_Volume = 0f;
				reservedPlayer.transpose = 0;
				reservedPlayer.MPTK_MidiName = midiQueue[0];
				reservedPlayer.MPTK_Play();
				reservedPlayer.MPTK_Pause();
				reservedPlayer.MPTK_TickCurrent = 0L;
			}
			else
			{
				Debug.LogError("MusicManager: reservedPlayer is null. Cannot queue MIDI.");
			}
		}
	}

	public void MidiLoop()
	{
		if (midiPlayer == null || reservedPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer or reservedPlayer is null. Cannot handle MIDI loop.");
			return;
		}

		if (MusicManagerMaes.OnMidiLoop != null)
		{
			MusicManagerMaes.OnMidiLoop(midiPlayer.MPTK_MidiName);
		}
		if (!midiPlayer.MPTK_MidiAutoRestart)
		{
			midiPlaying = false;
		}
		if (midiQueue.Count > 0)
		{
			reservedPlayer.MPTK_Volume = 1f;
			reservedPlayer.MPTK_UnPause();
			reservedPlayer.MPTK_MidiAutoRestart = true;
			midiPlayer.MPTK_Stop();
			midiQueue.RemoveAt(0);
			MusicManagerMaes.OnMidiTransitionComplete?.Invoke();
		}
	}

	public void TransitionFinished()
	{
		if (midiPlayer == null || reservedPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer or reservedPlayer is null. Cannot finish transition.");
			return;
		}

		midiPlayer.MPTK_RePlay();
		reservedPlayer.MPTK_RePlay();
		reservedPlayer.MPTK_Pause();
		reservedPlayer.MPTK_MidiAutoRestart = true;
		transitionPlaying = false;
	}

	public void SetCorruption(bool val)
	{
		if (midiPlayer == null)
		{
			Debug.LogError("MusicManager: midiPlayer is null. Cannot set corruption.");
			return;
		}

		midiCorrupted = val;
		if (val)
		{
			midiPlayer.MPTK_DirectSendToPlayer = false;
		}
		else
		{
			midiPlayer.MPTK_DirectSendToPlayer = true;
		}
	}
}