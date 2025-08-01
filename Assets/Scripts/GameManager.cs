using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentLoop = 1; // מספר האיטרציה (שלב)
    public GameState state = GameState.Playing;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // כדי שישרוד מעבר בין סצנות
    }

    public void NextLoop()
    {
        currentLoop++;
        // עדכן UI, אתחל שחקן וכו'
        Debug.Log("Loop: " + currentLoop);
        // אפשר להפעיל פה אפקטים, סאונד, קושי משתנה וכו'
    }

    public void SetGameState(GameState newState)
    {
        state = newState;
        // עדכן UI, עצור תנועה, הצג מסך ניצחון וכו'
    }
}

public enum GameState
{
    Playing,
    Paused,
    Win,
    Lose
}
