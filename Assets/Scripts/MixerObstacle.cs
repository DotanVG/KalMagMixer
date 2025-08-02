using UnityEngine;

public class MixerObstacle : MonoBehaviour
{
    public int trackIndex = 0; // set this in the Inspector for each obstacle group

    private Collider2D[] colliders;
    private SpriteRenderer[] spriteRenderers;

    void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>(includeInactive: true);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
    }

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
