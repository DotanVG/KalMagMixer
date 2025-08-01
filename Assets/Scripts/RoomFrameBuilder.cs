using UnityEngine;

public class RoomFrameBuilder : MonoBehaviour
{
    public GameObject blockPrefab;
    public float blockSize = 1f;  // ���� �� ���� ������� (�� Pixels Per Unit = 16 ����� ������� 16 � �� 1)
    public int blocksX = 18;      // ��� ������ ����� (���� ����� �-1920 �����)
    public int blocksY = 10;      // ��� ������ ����� (���� ����� �-1080 �����)
    public float startX = -8.5f;  // ����� ����� �-X
    public float startY = -4.5f;  // ����� ����� �-Y

    void Start()
    {
        // ���� ������
        for (int x = 0; x < blocksX; x++)
        {
            Vector3 pos = new Vector3(startX + x * blockSize, startY, 0);
            Instantiate(blockPrefab, pos, Quaternion.identity, transform);
        }

        // ���� ������
        for (int x = 0; x < blocksX; x++)
        {
            Vector3 pos = new Vector3(startX + x * blockSize, startY + (blocksY - 1) * blockSize, 0);
            Instantiate(blockPrefab, pos, Quaternion.identity, transform);
        }

        // ��� ����� �����
        for (int y = 1; y < blocksY - 1; y++)
        {
            // �����
            Vector3 leftPos = new Vector3(startX, startY + y * blockSize, 0);
            Instantiate(blockPrefab, leftPos, Quaternion.identity, transform);

            // ����
            Vector3 rightPos = new Vector3(startX + (blocksX - 1) * blockSize, startY + y * blockSize, 0);
            Instantiate(blockPrefab, rightPos, Quaternion.identity, transform);
        }
    }
}
