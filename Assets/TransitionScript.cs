using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    public Animator transition;
    public float waitTime = 1f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            LoadTransition();
        }
    }

    public void LoadTransition()
    {
        CleanupManager.Instance.Cleanup();

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == 1)
        {
            StartCoroutine(LoadLevel(2));
        }
        else
        {
            StartCoroutine(LoadLevel(1));
        }
    }

    IEnumerator LoadLevel(int idx)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(idx);
    }
}
