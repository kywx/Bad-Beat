using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenuController : MonoBehaviour
{
    [Header("References")]
    public MainMenuController mainMenuController;
    public Button backButton;

    [Header("Volume Slider")]
    public CustomVolumeSlider volumeSlider;

    private void Awake()
    {
        Debug.Log("OptionsMenuController Awake called on: " + gameObject.name);
    }

    private void Start()
    {
        Debug.Log("OptionsMenuController Start called");

        if (backButton != null)
        {
            Debug.Log("Back button found, adding listener");
            // Remove any existing listeners first
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnBackClicked);
        }
        else
        {
            Debug.LogError("Back button is not assigned in OptionsMenuController on " + gameObject.name);
        }

        if (mainMenuController == null)
        {
            Debug.LogError("MainMenuController is not assigned in OptionsMenuController!");
        }
    }

    private void OnEnable()
    {
        Debug.Log("OptionsMenuController OnEnable called");
    }

    public void OnBackClicked()
    {
        Debug.Log("Back button clicked!");

        if (mainMenuController != null)
        {
            Debug.Log("Calling ShowMainMenu()");
            mainMenuController.ShowMainMenu();
        }
        else
        {
            Debug.LogError("MainMenuController reference is missing in OptionsMenuController!");
        }
    }

    // Alternative method if you want to connect via Inspector instead of code
    public void BackToMainMenu()
    {
        Debug.Log("BackToMainMenu called");
        OnBackClicked();
    }
}
