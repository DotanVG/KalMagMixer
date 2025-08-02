using UnityEngine;

/// <summary>
/// Controls game state, loop progression, music, and activates correct obstacle groups.
/// Now also forces player respawn if inside any enabled obstacle after a loop change.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int currentLoop = 0;
    public int CurrentLoop => currentLoop;

    public GameState state = GameState.Playing;

    [Header("Obstacle Groups Per Loop (size 8)")]
    public GameObject[] obstaclesGroups; // Assign: 0=Yellow, 1=Red, ..., 7=Purple

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Hide all obstacle groups at start.
        foreach (var group in obstaclesGroups)
            if (group != null)
                group.SetActive(false);
    }

    /// <summary>
    /// Progress to the next loop/stage.
    /// </summary>
    public void NextLoop()
    {
        currentLoop++;

        // Activate all obstacle groups up to current loop (cumulative).
        for (int i = 0; i < obstaclesGroups.Length; i++)
        {
            if (obstaclesGroups[i] != null)
                obstaclesGroups[i].SetActive(i < currentLoop);
        }

        // --- NEW: Check if player is overlapping any newly enabled obstacle and respawn if so ---
        Player.PlayerController.TryForceRespawnIfOverlapping();

        // Start/unmute music tracks as needed.
        if (currentLoop == 1)
        {
            AudioManager.Instance.StartFirstLoopMusic();
        }
        else if (currentLoop > 1)
        {
            AudioManager.Instance.NextLoopMusic();
        }

        // Play magic SFX.
        AudioManager.Instance.PlayMagicSFX();

        // --- FORCE ALL RELEVANT TRACKS/BUTTONS/OBSTACLES ON ---
        var mixerManager = FindObjectOfType<MixerManager>();
        if (mixerManager != null)
        {
            for (int i = 1; i <= currentLoop; i++)
                mixerManager.ForceTrackOn(i);
        }
    }

    public void SetGameState(GameState newState)
    {
        state = newState;
        // TODO: UI updates etc.
    }
}

public enum GameState
{
    Playing,
    Paused,
    Win,
    Lose
}
