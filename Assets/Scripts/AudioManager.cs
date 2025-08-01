using UnityEngine;

/// <summary>
/// Singleton AudioManager for SFX and synchronized music tracks.
/// Handles loop synchronization so new tracks join only on main track loop restart.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX Clips")]
    public AudioClip sfx_jump;
    public AudioClip sfx_magic;

    [Header("Music Tracks (Loops)")]
    public AudioClip[] loopTracks = new AudioClip[8]; // Up to 8 channels/tracks

    private AudioSource sfxSource;
    private AudioSource[] musicSources = new AudioSource[8];

    // Synchronization fields
    private int nextToAdd = 1; // Next track index to queue for syncing
    private bool[] trackPending = new bool[8]; // Pending tracks waiting to sync on loop
    private float prevMainTime = 0f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // SFX setup
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        // Music channels setup
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i] = gameObject.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            musicSources[i].volume = 0.5f;
        }
    }

    private void Start()
    {
        // Start only the main track at the beginning (if assigned)
        if (loopTracks[0] != null)
        {
            musicSources[0].clip = loopTracks[0];
            musicSources[0].Play();
        }
    }

    private void Update()
    {
        // Only sync if main track is playing
        if (musicSources[0].isPlaying)
        {
            float currTime = musicSources[0].time;
            // If time wrapped around, loop started again
            if (currTime < prevMainTime)
            {
                SyncPendingTracks();
            }
            prevMainTime = currTime;
        }
    }

    /// <summary>
    /// Plays a given SFX clip instantly (one-shot).
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Play the jump SFX.
    /// </summary>
    public void PlayJumpSFX()
    {
        PlaySFX(sfx_jump);
    }

    /// <summary>
    /// Play the magic SFX (e.g., when looping).
    /// </summary>
    public void PlayMagicSFX()
    {
        PlaySFX(sfx_magic);
    }

    /// <summary>
    /// Queue the next music track to start on the next main loop (synchronized).
    /// </summary>
    public void NextLoopMusic()
    {
        if (nextToAdd < loopTracks.Length && loopTracks[nextToAdd] != null)
        {
            trackPending[nextToAdd] = true;
            nextToAdd++;
        }
    }

    /// <summary>
    /// Called automatically when main track restarts.
    /// Starts all pending tracks at the same time (synchronized).
    /// </summary>
    private void SyncPendingTracks()
    {
        for (int i = 1; i < musicSources.Length; i++)
        {
            if (trackPending[i] && loopTracks[i] != null)
            {
                musicSources[i].clip = loopTracks[i];
                musicSources[i].time = 0f;
                musicSources[i].Play();
                trackPending[i] = false;
            }
        }
    }

    /// <summary>
    /// Stops all music tracks (if needed).
    /// </summary>
    public void StopAllMusic()
    {
        foreach (var src in musicSources)
            src.Stop();
        // Reset sync state if needed
        for (int i = 1; i < trackPending.Length; i++)
            trackPending[i] = false;
        nextToAdd = 1;
    }
}
