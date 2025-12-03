using System.Collections.Generic;
using UnityEngine;
using FluidMidi;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private SongPlayer midiPlayerPrefab, drumsMidiPrefab;
    private List<SongPlayer> midiPlayers = new List<SongPlayer>();
    private List<SongPlayer> drumsMidi = new List<SongPlayer>();
    private Transform targetTransform;
    private SongPlayer syncedDrumsMidi;
    private bool gainDrumsToObject, midiIsPaused, drumsIsPlaying, hangedMidi;
    private float savedTempo;

    public SongPlayer PlayMidi(StreamingAsset midi, bool loop)
    {
        midiPlayerPrefab.Song = midi;
        midiPlayerPrefab.ToggleLoop(loop);
        midiPlayerPrefab.Tempo = 1f;
        SongPlayer newMidi = Instantiate<SongPlayer>(midiPlayerPrefab, base.transform);
        newMidi.Song = midi;
        newMidi.ToggleLoop(loop);
        newMidi.Tempo = 1f;
        newMidi.Play();
        midiPlayers.Add(newMidi);
        return newMidi;
    }

    public SongPlayer PlayDrumsMidi(StreamingAsset midi, bool loop, SongPlayer normalMidi)
    {
        drumsIsPlaying = true;
        drumsMidiPrefab.Song = midi;
        drumsMidiPrefab.ToggleLoop(loop);
        normalMidi.DisableChannels(10);
        SongPlayer newDrumsMidi = Instantiate<SongPlayer>(drumsMidiPrefab, base.transform);
        newDrumsMidi.Song = midi;
        newDrumsMidi.ToggleLoop(loop);
        newDrumsMidi.Play();
        SeekToDrums(normalMidi, newDrumsMidi);
        drumsMidi.Add(newDrumsMidi);
        return newDrumsMidi;
    }

    public SongPlayer PlayNormalMidi(StreamingAsset midi, bool loop = true, float tempo = 1f)
    {
        midiPlayerPrefab.Song = midi;
        midiPlayerPrefab.Tempo = tempo;
        midiPlayerPrefab.ToggleLoop(loop);
        SongPlayer newMidi = Instantiate<SongPlayer>(midiPlayerPrefab, base.transform);
        newMidi.Song = midi;
        newMidi.ToggleLoop(loop);
        newMidi.Tempo = tempo;
        newMidi.Play();
        newMidi.EnableChannels(10);
        midiPlayers.Add(newMidi);
        return newMidi;
    }

    public void PauseMidi(bool pause)
    {
        if (midiPlayers.Count > 0)
        {
            if (pause && !midiIsPaused)
            {
                foreach (SongPlayer midiPlayers in midiPlayers)
                {
                    midiPlayers.Pause();
                    midiIsPaused = true;
                }
                if (drumsMidi.Count > 0)
                {
                    foreach (SongPlayer drumsMidiPlayers in drumsMidi)
                    {
                        drumsMidiPlayers.Pause();
                    }
                }
                return;
            }
            if (!pause && midiIsPaused)
            {
                foreach (SongPlayer midiPlayers in midiPlayers)
                {
                    midiPlayers.Resume();
                    midiIsPaused = false;
                }
                if (drumsMidi.Count > 0)
                {
                    foreach (SongPlayer drumsMidiPlayers in drumsMidi)
                    {
                        drumsMidiPlayers.Resume();
                    }
                }
            }
        }
    }

    public void StopMidi(bool destroyAll, SongPlayer songPlayer = null, SongPlayer drumsMidiPlayer = null)
    {
        if (destroyAll)
        {
            SongPlayer[] fluidMidiInstances = FindObjectsOfType<SongPlayer>();

            midiPlayers.Clear();
            drumsMidi.Clear();

            foreach (SongPlayer fluidMidiPlayers in fluidMidiInstances)
            {
                Destroy(fluidMidiPlayers.gameObject);
            }
            return;
        }

        if (songPlayer != null)
        {
            midiPlayers.Remove(songPlayer);
            Destroy(songPlayer.gameObject);
        }

        if (drumsMidiPlayer != null)
        {
            drumsMidi.Remove(drumsMidiPlayer);
            Destroy(drumsMidiPlayer.gameObject);
        }
    }

    public void SeekToDrums(SongPlayer songPlayer, SongPlayer drumsPlayer)
    {
        songPlayer.Seek(drumsPlayer.Ticks);
    }

    public void SetSpeed(float speed, SongPlayer songPlayer, SongPlayer drumsPlayer = null)
    {
        if (hangedMidi)
            return;

        songPlayer.Tempo = speed;
        if (drumsPlayer != null)
        {
            drumsPlayer.Tempo = speed;
        }
    }

    public void HangMidi(bool stop, SongPlayer songPlayer)
    {
        if (songPlayer.Tempo != 0.001f)
        {
            savedTempo = songPlayer.Tempo;
        }
        songPlayer.Tempo = stop ? 0.001f : savedTempo;
        hangedMidi = stop;
    }

    private void Update()
    {
        if (drumsIsPlaying && gainDrumsToObject && syncedDrumsMidi != null)
        {
            float drumsGain = 25f / (targetTransform.position - GameControllerScript.Instance.player.transform.position).magnitude;
            if (drumsGain > 1f)
            {
                drumsGain = 1f;
            }
            syncedDrumsMidi.Gain = drumsGain;
        }
    }

    public void GainDrums(Transform target, SongPlayer drumsSongPlayer)
    {
        targetTransform = target;
        gainDrumsToObject = true;
        syncedDrumsMidi = drumsSongPlayer;
    }
}