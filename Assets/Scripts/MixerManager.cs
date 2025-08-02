using System.Linq;
using UnityEngine;

/// <summary>
/// Manages keyboard input for toggling music tracks and updates UI/buttons/obstacles accordingly.
/// </summary>
public class MixerManager : MonoBehaviour
{
    private MixerButtonUI[] allButtons;
    private AudioManager audioManager;

    private void Start()
    {
        allButtons = FindObjectsOfType<MixerButtonUI>();
        audioManager = AudioManager.Instance;

        // Start all buttons OFF and transparent
        foreach (var button in allButtons)
            button.SetActiveState(false);
    }

    private void Update()
    {
        int currentLoop = GameManager.Instance.CurrentLoop;

        // Only allow toggling tracks that are unlocked (up to currentLoop)
        for (int i = 1; i <= currentLoop; i++)
        {
            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i);
            if (Input.GetKeyDown(key))
            {
                ToggleTrack(i);
            }
        }
    }

    /// <summary>
    /// Force a channel ON visually and audibly, used by GameManager at new loop.
    /// </summary>
    public void ForceTrackOn(int trackIndex)
    {
        // Music
        var src = audioManager.GetMusicSource(trackIndex - 1); // 0-based
        if (src != null)
            src.volume = 0.5f;

        // Mixer button UI
        foreach (var btn in allButtons)
            if (btn.trackIndex == trackIndex)
                btn.SetActiveState(true);

        // Obstacles
        MixerObstacle[] allObstacles = FindObjectsOfType<MixerObstacle>();
        foreach (var obs in allObstacles)
            if (obs.trackIndex == trackIndex)
                obs.SetActiveObstacle(true);
    }

    /// <summary>
    /// Toggle track ON/OFF and update visuals/obstacles.
    /// </summary>
    private void ToggleTrack(int trackIndex)
    {
        var src = audioManager.GetMusicSource(trackIndex - 1);
        if (src == null) return;

        bool isNowMuted = src.volume > 0.1f;
        src.volume = isNowMuted ? 0f : 0.5f;

        // Update corresponding mixer button
        foreach (var btn in allButtons)
            if (btn.trackIndex == trackIndex)
                btn.SetActiveState(!isNowMuted);

        // Update obstacles
        MixerObstacle[] allObstacles = FindObjectsOfType<MixerObstacle>();
        foreach (var obs in allObstacles)
            if (obs.trackIndex == trackIndex)
                obs.SetActiveObstacle(!isNowMuted);
    }
}
