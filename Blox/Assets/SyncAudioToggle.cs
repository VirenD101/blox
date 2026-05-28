using UnityEngine;
using UnityEngine.UI;

public class SyncAudioToggle : MonoBehaviour
{
    private void Start()
    {
        Toggle toggleComponent = GetComponent<Toggle>();

        // Check if our persistent manager exists in the level space
        if (toggleComponent != null && AudioManager.Instance != null)
        {
            // Set the checkbox UI state to match the real global volume state
            toggleComponent.isOn = AudioManager.Instance.IsMuted;
        }
    }
}