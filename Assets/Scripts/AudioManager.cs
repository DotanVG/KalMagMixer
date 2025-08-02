using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX Clips + Volumes")]
    public AudioClip sfx_jump;
    [Range(0f, 1f)] public float sfx_jumpVolume = 0.2f;

    public AudioClip sfx_magic;
    [Range(0f, 1f)] public float sfx_magicVolume = 0.7f;

    [Header("Music Tracks (Loops) + Volumes")]
    public AudioClip[] loopTracks = new AudioClip[8];
    [Range(0f, 1f)] public float[] loopVolumes = new float[8]; // Set in Inspector for each track

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

        // One SFX source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        // Music sources
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i] = gameObject.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            musicSources[i].volume = 0f;
        }

        // Ensure loopVolumes array is initialized to same size as loopTracks
        if (loopVolumes.Length != loopTracks.Length)
        {
            float[] temp = new float[loopTracks.Length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = 0.5f; // Default volume
            loopVolumes = temp;
        }
    }

    public void StartFirstLoopMusic()
    {
        for (int i = 0; i < loopTracks.Length; i++)
        {
            if (loopTracks[i] != null)
            {
                musicSources[i].clip = loopTracks[i];
                musicSources[i].time = 0f;
                musicSources[i].Play();
                musicSources[i].volume = (i == 0) ? loopVolumes[0] : 0f;
            }
        }
        currentUnmuted = 1;
    }

    public void NextLoopMusic()
    {
        if (currentUnmuted < loopTracks.Length && loopTracks[currentUnmuted] != null)
        {
            musicSources[currentUnmuted].volume = loopVolumes[currentUnmuted];
            currentUnmuted++;
        }
    }

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
    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }
    public void PlayJumpSFX() => PlaySFX(sfx_jump, sfx_jumpVolume);
    public void PlayMagicSFX() => PlaySFX(sfx_magic, sfx_magicVolume);

    // --- Music source getter ---
    public AudioSource GetMusicSource(int trackIdx)
    {
        if (trackIdx < 0 || trackIdx >= musicSources.Length) return null;
        return musicSources[trackIdx];
    }
}
