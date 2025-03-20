using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.Build;

public class PauseMenu : MonoBehaviour
{

    // Document to reference from the inspector. (Pause menu GameObject)
    public UIDocument _document;
    private VisualElement _pauseMenu;
    private Button _resumeButton;
    private Button _quitButton;
    private bool isPaused = false;
    void Awake()
    {
        // Get the UIDocument (I think this is the visual tree since we are using the names found in the uxml file)
        _document = GetComponent<UIDocument>();

        // Assign all the buttons
        _pauseMenu = _document.rootVisualElement.Q<VisualElement>("PauseMenu");
        _resumeButton = _document.rootVisualElement.Q<Button>("ResumeButton");
        _quitButton = _document.rootVisualElement.Q<Button>("QuitButton");

        // Register the event that will happen when the buttons get clicked
        _resumeButton.RegisterCallback<ClickEvent>(evt => ResumeGame());
        _quitButton.RegisterCallback<ClickEvent>(evt => QuitGame());

        // Hide the pause menu when game is on going
        _pauseMenu.style.display = DisplayStyle.None;
    }

    // When ESC key gets pressed
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;

        // Freeze the game
        Time.timeScale = 0f;

        // Show the UI
        _pauseMenu.style.display = DisplayStyle.Flex;
    }

    private void QuitGame()
    {
        // Pausing the game will make the time scale freeze so we have to unfreeze it when we quit
        Time.timeScale = 1f;
        // Load the Main Menu
        SceneManager.LoadScene("MainMenu");
    }

    private void ResumeGame()
    {
        isPaused = false;

        // Unfreeze game when resumingg
        Time.timeScale = 1f;

        // Hide the UI
        _pauseMenu.style.display = DisplayStyle.None;
    }
}
