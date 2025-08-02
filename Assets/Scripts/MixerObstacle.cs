using UnityEngine;

/// <summary>
/// Controls visibility and collision of obstacles per track/channel.
/// </summary>
public class MixerObstacle : MonoBehaviour
{
    public int trackIndex = 1; // 1-based index; assign in Inspector per group

    private Collider2D[] colliders;
    private SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>(includeInactive: true);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
    }

    /// <summary>
    /// Enable/disable collider and set visual opacity (1=active, 0.3=inactive).
    /// </summary>
    public void SetActiveObstacle(bool active)
    {
        foreach (var col in colliders)
            col.enabled = active;

        foreach (var sr in spriteRenderers)
        {
            var color = sr.color;
            color.a = active ? 1f : 0.3f;
            sr.color = color;
        }
    }
}
