using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Options Menu Controller - Attach to Options Panel GameObject
/// </summary>
public class OptionsMenuController : MonoBehaviour
{
    [Header("References")]
    public MainMenuController mainMenuController;
    public Button backButton;

    [Header("Volume Slider")]
    public CustomVolumeSlider volumeSlider;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnBackClicked()
    {
        mainMenuController.ShowMainMenu();
    }
}
