using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript1 : MonoBehaviour
{
    public Animator transition;
    public float waitTime = 1f;

    public void LoadTransition(string SceneName)
    {
        StartCoroutine(LoadLevel(SceneName));
    }

    IEnumerator LoadLevel(string SceneName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(SceneName);
    }
}
