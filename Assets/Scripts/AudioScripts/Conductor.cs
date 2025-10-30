using UnityEngine;

public class Conductor : MonoBehaviour
{
    public float songBpm;

    public float secPerBeat; // seconds per song beat

    public float songPosition; // current song position in seconds

    public float songPositionInBeats;

    public float dspSongTime; // seconds since the song started

    public AudioSource musicSource; // audio source attached to the GameObject

    public float firstBeatOffset;

    void Start()
    {
        musicSource = GetComponent<AudioSource>();

        secPerBeat = 60f / songBpm; // calculate seconds in each beat

        // Returns the current time of the audio system.
        dspSongTime = (float)AudioSettings.dspTime;

        musicSource.Play(); // plays music on Start, NOT Awake
    }

    void Update()
    {
        // seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        // beats since song started
        songPositionInBeats = songPosition / secPerBeat;
    }
}
