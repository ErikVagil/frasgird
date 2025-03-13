using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.Build;

public class PauseMenu : MonoBehaviour
{

    public UIDocument _document;
    private VisualElement _pauseMenu;
    private Button _resumeButton;
    private Button _quitButton;
    private bool isPaused = false;
    void Awake()
    {
        _document = GetComponent<UIDocument>();
        _pauseMenu = _document.rootVisualElement.Q<VisualElement>("PauseMenu");
        _resumeButton = _document.rootVisualElement.Q<Button>("ResumeButton");
        _quitButton = _document.rootVisualElement.Q<Button>("QuitButton");

        _resumeButton.RegisterCallback<ClickEvent>(evt => ResumeGame());
        _quitButton.RegisterCallback<ClickEvent>(evt => QuitGame());

        _pauseMenu.style.display = DisplayStyle.None;
    }

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
        // Quitting the game from the menu will make the time scale freeze so we have to unfreeze it when we quit
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        _pauseMenu.style.display = DisplayStyle.None;
    }
}
