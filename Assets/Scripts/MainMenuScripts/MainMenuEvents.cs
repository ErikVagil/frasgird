using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    public Animator transition;
    public float waitTime = 2f;

    private Button _StartButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _StartButton = _document.rootVisualElement.Q("StartGameButton") as Button;

        _StartButton.RegisterCallback<ClickEvent>(OnPlayGameClick);
    }

    private void OnDisable()
    {
        _StartButton.UnregisterCallback<ClickEvent>(OnPlayGameClick);
    }

    private void OnPlayGameClick(ClickEvent evt)
    {

        // Debug.Log("Pressed the start button!");
        StartCoroutine(FadeOutUI());


    }

    IEnumerator FadeOutUI()
{
    VisualElement uiRoot = _document.rootVisualElement;
    float fadeDuration = 1.5f;
    float elapsedTime = 0f;

    while (elapsedTime < fadeDuration)
    {
        elapsedTime += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
        uiRoot.style.opacity = alpha;
        yield return null;
    }

    // Hide UI after fade-out
    uiRoot.style.display = DisplayStyle.None;

    // Start the transition animation
    StartCoroutine(LoadGame());
}

    IEnumerator LoadGame()
    {
        transition.SetTrigger("Start");

        // Destroy(_document.gameObject);
        yield return new WaitForSeconds(waitTime);
        yield return null;

        SceneManager.LoadScene(1);
    }

}
