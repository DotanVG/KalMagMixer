using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure your player GameObject is tagged "Player"
        {
            Player.PlayerController player = other.GetComponent<Player.PlayerController>();
            if (player != null)
            {
                player.Respawn();
            }
        }
    }
}
