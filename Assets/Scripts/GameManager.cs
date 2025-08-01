using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentLoop = 0; // ���� �������� (���)
    public GameState state = GameState.Playing;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // ��� ������ ���� ��� �����
    }

    public void NextLoop()
    {
        if (currentLoop == 1)
        {
            AudioManager.Instance.StartMainMusic();
        }
        currentLoop++;
        // ���� ������ �� ������, �����, ���� ����� ���'
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfx_magic);
        AudioManager.Instance.NextLoopMusic();
        Debug.Log("Loop: " + currentLoop);
    }

    public void SetGameState(GameState newState)
    {
        state = newState;
        // ���� UI, ���� �����, ��� ��� ������ ���'
    }
}

public enum GameState
{
    Playing,
    Paused,
    Win,
    Lose
}
