using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverScreen : MonoBehaviour
{
    public UIDocument gameOverUI;

    private VisualElement _gameOverScreen;
    public UIDocument colonyUI;
    private Button continueButton;

    private void Awake()
    {
        if (gameOverUI == null)
        {
            Debug.LogError("Game over UI not assigned");
            return;
        }

        _gameOverScreen = gameOverUI.rootVisualElement.Q<VisualElement>("GameOverScreen");

        if (_gameOverScreen == null)
        {
            Debug.LogError("Game over screen element not found ");
            return;
        }

        _gameOverScreen.style.display = DisplayStyle.None;
        continueButton = gameOverUI.rootVisualElement.Q("Continue") as Button;

        if (continueButton != null)
        {
            continueButton.clicked += CreateNewGameOnContinue;
        }
    }

    private void CreateNewGameOnContinue()
    {
        Colony.Instance = new Colony(); // Reset colony state
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload
    }

    private void Update()
    {
        if (Colony.Instance == null)
        {
            Debug.LogError("Colony instance not found");
            return;
        }

        if (Colony.Instance.Population <= 9)
        {
            _gameOverScreen.style.display = DisplayStyle.Flex;
            colonyUI.rootVisualElement.style.display = DisplayStyle.None;

        }
        else
        {
            _gameOverScreen.style.display = DisplayStyle.None;
        }
    }
}
