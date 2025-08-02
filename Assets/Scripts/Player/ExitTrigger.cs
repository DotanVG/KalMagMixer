using UnityEngine;

/// <summary>
/// Detects when the player reaches the exit. 
/// Calls player respawn and advances the game loop.
/// </summary>
public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Make sure your player GameObject is tagged "Player"
        if (other.CompareTag("Player"))
        {
            Player.PlayerController player = other.GetComponent<Player.PlayerController>();
            if (player != null)
            {
                player.Respawn();
                GameManager.Instance.NextLoop();
            }
        }
    }
}
