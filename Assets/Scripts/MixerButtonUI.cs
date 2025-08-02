using UnityEngine;

/// <summary>
/// Controls the mixer button visual state for each music channel.
/// </summary>
public class MixerButtonUI : MonoBehaviour
{
    public int trackIndex; // 1-based index (track 1..8)
    private SpriteRenderer sr;
    private bool isOn = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        SetActiveState(false); // Start OFF (transparent)
    }

    /// <summary>
    /// Set the mixer button ON/OFF (opaque/transparent).
    /// </summary>
    public void SetActiveState(bool active)
    {
        isOn = active;
        Color c = sr.color;
        c.a = active ? 1f : 0.3f;
        sr.color = c;
    }

    /// <summary>
    /// Get whether the button is ON.
    /// </summary>
    public bool IsOn() => isOn;
}
