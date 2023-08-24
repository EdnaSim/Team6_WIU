using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private TMP_Text percentText;

    public static SceneLoader Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        percentText = GetComponent<TMP_Text>();
        
    }

    public void LoadScene(string SceneName)
    {
        StartCoroutine(LoadSceneRoutine(SceneName));
    }

    IEnumerator LoadSceneRoutine(string SceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneName);
        while (op.isDone == false)
        {
            percentText.text =
            Mathf.Round((op.progress * 100) / 100)
            + "%";
            yield return null;
        }
    }
}
