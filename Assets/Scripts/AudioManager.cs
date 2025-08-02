using UnityEngine;

/// <summary>
/// AudioManager: Handles SFX and multi-track music with mute/unmute.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX Clips")]
    public AudioClip sfx_jump;
    public AudioClip sfx_magic;

    [Header("Music Tracks (Loops)")]
    public AudioClip[] loopTracks = new AudioClip[8]; // Assign in Inspector (size 8)

    private AudioSource sfxSource;
    private AudioSource[] musicSources = new AudioSource[8];
    private int currentUnmuted = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // SFX
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        // Music
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i] = gameObject.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            musicSources[i].volume = 0f;
        }
    }

    /// <summary>
    /// Start all music tracks in sync, but only unmute track 1.
    /// </summary>
    public void StartFirstLoopMusic()
    {
        for (int i = 0; i < loopTracks.Length; i++)
        {
            if (loopTracks[i] != null)
            {
                musicSources[i].clip = loopTracks[i];
                musicSources[i].time = 0f;
                musicSources[i].Play();
                musicSources[i].volume = (i == 0) ? 0.5f : 0f; // Only first is ON
            }
        }
        currentUnmuted = 1;
    }

    /// <summary>
    /// Unmute the next track in order.
    /// </summary>
    public void NextLoopMusic()
    {
        if (currentUnmuted < loopTracks.Length && loopTracks[currentUnmuted] != null)
        {
            musicSources[currentUnmuted].volume = 0.5f;
            currentUnmuted++;
        }
    }

    /// <summary>
    /// Stop all music and mute all tracks.
    /// </summary>
    public void StopAllMusic()
    {
        foreach (var src in musicSources)
        {
            src.Stop();
            src.volume = 0f;
        }
        currentUnmuted = 0;
    }

    // --- SFX ---
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
    public void PlayJumpSFX() => PlaySFX(sfx_jump);
    public void PlayMagicSFX() => PlaySFX(sfx_magic);

    /// <summary>
    /// Get music AudioSource by index (0-7).
    /// </summary>
    public AudioSource GetMusicSource(int trackIdx)
    {
        if (trackIdx < 0 || trackIdx >= musicSources.Length) return null;
        return musicSources[trackIdx];
    }
}
