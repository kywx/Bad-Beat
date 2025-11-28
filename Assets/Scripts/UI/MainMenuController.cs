using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Main Menu Controller - Attach to a Canvas GameObject
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button continueButton;
    public Button optionsButton;
    public Button quitButton;

    [Header("Continue Button Styling")]
    public Color disabledTint = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    [Header("Scene Settings")]
    public string gameSceneName = "GameScene";

    private void Start()
    {
        // Setup button listeners
        startButton.onClick.AddListener(OnStartClicked);
        optionsButton.onClick.AddListener(OnOptionsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);

        // Disable continue button
        continueButton.interactable = false;
        ColorBlock cb = continueButton.colors;
        cb.disabledColor = disabledTint;
        continueButton.colors = cb;

        // Show main menu, hide options
        ShowMainMenu();
    }

    private void OnStartClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnOptionsClicked()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
}