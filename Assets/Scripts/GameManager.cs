using UnityEngine;

/// <summary>
/// Controls game state, loop progression, and music stage transitions.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int currentLoop = 0; // Private to avoid Inspector override issues.
    public int CurrentLoop => currentLoop; // Read-only property for code/UI if needed.

    public GameState state = GameState.Playing;

    private void Awake()
    {
        // Singleton pattern: ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist through scene changes
        Debug.Log("GameManager Awake. currentLoop = " + currentLoop);
    }

    /// <summary>
    /// Advance to the next loop (iteration).
    /// Handles starting music and unmuting additional tracks as needed.
    /// </summary>
    public void NextLoop()
    {
        currentLoop++;

        Debug.Log($"NextLoop() called. currentLoop = {currentLoop}");

        // On entering the FIRST music loop (after silent stage)
        if (currentLoop == 1)
        {
            Debug.Log("Trigger: StartFirstLoopMusic()");
            AudioManager.Instance.StartFirstLoopMusic();
        }
        // For all loops after the first, unmute the next track
        else if (currentLoop > 1)
        {
            Debug.Log("Trigger: NextLoopMusic()");
            AudioManager.Instance.NextLoopMusic();
        }

        // Play the magic SFX when looping
        AudioManager.Instance.PlayMagicSFX();
    }

    /// <summary>
    /// Change the current game state (e.g. win, pause, lose).
    /// Update UI, freeze movement, etc. as needed.
    /// </summary>
    public void SetGameState(GameState newState)
    {
        state = newState;
        // Update UI, pause movement, show win/loss screen, etc.
    }
}

/// <summary>
/// Enum representing main game states.
/// </summary>
public enum GameState
{
    Playing,
    Paused,
    Win,
    Lose
}
