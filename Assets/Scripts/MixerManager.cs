using UnityEngine;

/// <summary>
/// Lets you toggle music tracks 1-8 (keys 1-8, both top row and numpad),
/// and disables/enables obstacles with the matching trackIndex.
/// </summary>
public class MixerManager : MonoBehaviour
{
    void Update()
    {
        // For each key 1-8, check if either AlphaN or KeypadN was pressed
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            HandleTrackToggle(1);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            HandleTrackToggle(2);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            HandleTrackToggle(3);
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            HandleTrackToggle(4);
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            HandleTrackToggle(5);
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            HandleTrackToggle(6);
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
            HandleTrackToggle(7);
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            HandleTrackToggle(8);
    }

    /// <summary>
    /// Toggle mute state for the given track (1-based), and update obstacles with matching trackIndex.
    /// </summary>
    /// <param name="trackIdx">1-based track index (as set in MixerObstacle)</param>
    private void HandleTrackToggle(int trackIdx)
    {
        Debug.Log($"Key {trackIdx} pressed, toggling trackIndex={trackIdx}");

        // Music: get AudioSource for this track (subtract 1 for zero-based array)
        var src = AudioManager.Instance.GetMusicSource(trackIdx - 1);
        if (src == null)
        {
            Debug.LogWarning($"No AudioSource for track {trackIdx - 1}");
            return;
        }

        bool isNowMuted = src.volume > 0.1f;
        src.volume = isNowMuted ? 0f : 0.5f;
        Debug.Log($"Track {trackIdx} {(isNowMuted ? "muted" : "unmuted")}");

        // Obstacles: toggle all with this track index
        MixerObstacle[] allObstacles = FindObjectsOfType<MixerObstacle>();
        foreach (var obs in allObstacles)
        {
            if (obs.trackIndex == trackIdx)
            {
                obs.SetActiveObstacle(!isNowMuted);
            }
        }
    }
}
