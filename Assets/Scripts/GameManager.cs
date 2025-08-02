using UnityEngine;

/// <summary>
/// Controls game state, loop progression, music stage transitions,
/// and activates only the current loop's obstacle group.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Loop index: 0 before any music, then 1 = first loop, etc.
    private int currentLoop = 0;
    public int CurrentLoop => currentLoop; // Read-only for other scripts/UI.

    public GameState state = GameState.Playing;

    [Header("Obstacle Groups Per Loop (size 8)")]
    public GameObject[] obstaclesGroups; // Assign: 0=Yellow, 1=Red, ..., 7=Purple

    private void Awake()
    {
        // Singleton pattern: only one allowed.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("GameManager Awake. currentLoop = " + currentLoop);
    }

    private void Start()
    {
        // At game start, only show no obstacles.
        for (int i = 0; i < obstaclesGroups.Length; i++)
        {
            if (obstaclesGroups[i] != null)
                obstaclesGroups[i].SetActive(false);
        }
    }

    /// <summary>
    /// Progress to the next loop/stage:
    /// - Show the next group of obstacles, hide the others.
    /// - Start/unmute music tracks as needed.
    /// </summary>
    public void NextLoop()
    {
        currentLoop++;

        Debug.Log($"NextLoop() called. currentLoop = {currentLoop}");

        // Enable only the group for this loop, hide others.
        for (int i = 0; i < obstaclesGroups.Length; i++)
        {
            if (obstaclesGroups[i] != null)
                obstaclesGroups[i].SetActive(i == currentLoop - 1); // i==currentLoop-1 is visible, rest hidden
        }

        // Music logic: first loop starts music, others unmute more tracks
        if (currentLoop == 1)
        {
            Debug.Log("Trigger: StartFirstLoopMusic()");
            AudioManager.Instance.StartFirstLoopMusic();
        }
        else if (currentLoop > 1)
        {
            Debug.Log("Trigger: NextLoopMusic()");
            AudioManager.Instance.NextLoopMusic();
        }

        // Play magic SFX every loop.
        AudioManager.Instance.PlayMagicSFX();
    }

    /// <summary>
    /// Change the current game state (pause, win, etc.)
    /// </summary>
    public void SetGameState(GameState newState)
    {
        state = newState;
        // TODO: UI, player movement, etc.
    }
}

/// <summary>
/// Enum for game state.
/// </summary>
public enum GameState
{
    Playing,
    Paused,
    Win,
    Lose
}
