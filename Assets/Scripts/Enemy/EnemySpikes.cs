using UnityEngine;

/// <summary>
/// Triggers player respawn and loop restart when touched.
/// </summary>
public class EnemySpikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.PlayerController player = other.GetComponent<Player.PlayerController>();
            if (player != null)
            {
                player.Respawn();
            }
        }
    }
}
