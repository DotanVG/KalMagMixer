using UnityEngine;

public class RoomFrameBuilder : MonoBehaviour
{
    public GameObject blockPrefab;
    public float blockSize = 1f;  // גודל של בלוק ביוניטס (אם Pixels Per Unit = 16 וגודל הספרייט 16 – זה 1)
    public int blocksX = 18;      // כמה בלוקים ברוחב (בערך מתאים ל-1920 פיקסל)
    public int blocksY = 10;      // כמה בלוקים בגובה (בערך מתאים ל-1080 פיקסל)
    public float startX = -8.5f;  // נקודת התחלה ב-X
    public float startY = -4.5f;  // נקודת התחלה ב-Y

    void Start()
    {
        // שורה תחתונה
        for (int x = 0; x < blocksX; x++)
        {
            Vector3 pos = new Vector3(startX + x * blockSize, startY, 0);
            Instantiate(blockPrefab, pos, Quaternion.identity, transform);
        }

        // שורה עליונה
        for (int x = 0; x < blocksX; x++)
        {
            Vector3 pos = new Vector3(startX + x * blockSize, startY + (blocksY - 1) * blockSize, 0);
            Instantiate(blockPrefab, pos, Quaternion.identity, transform);
        }

        // קיר שמאלי וימני
        for (int y = 1; y < blocksY - 1; y++)
        {
            // שמאלי
            Vector3 leftPos = new Vector3(startX, startY + y * blockSize, 0);
            Instantiate(blockPrefab, leftPos, Quaternion.identity, transform);

            // ימני
            Vector3 rightPos = new Vector3(startX + (blocksX - 1) * blockSize, startY + y * blockSize, 0);
            Instantiate(blockPrefab, rightPos, Quaternion.identity, transform);
        }
    }
}
