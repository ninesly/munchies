using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SimpleLevelLoader : MonoBehaviour
{

    [SerializeField] int timeToWait = 3;
    [SerializeField] Image screenDimmer;
    int currentSceneIndex;
    float startingTime;
    float endingTime;

    void Start()
    {
        SetUpVars();
        if (currentSceneIndex == 0) StartCoroutine(WaitForTime());
        if (screenDimmer) Dim();

    }

    private void Dim()
    {
        var fuze = 0;
        var alpha = screenDimmer.color.a / timeToWait;
        var color = screenDimmer.color;
        startingTime = Time.time;
        endingTime = startingTime + Time.deltaTime * timeToWait;

        for (float currentTime = startingTime; currentTime < endingTime && fuze < 50; currentTime =+ Time.deltaTime)
        {
            color.a -= alpha;
            screenDimmer.color = color;
            fuze++;
        }

        Debug.LogWarning("fuze: " + fuze);
    }

    private void SetUpVars()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timeToWait);
        LoadStartScreen();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadStartScreen()
    {
        SceneManager.LoadScene("Start Screen");
    }

    public void LoadCreditsScreen()
    {
        SceneManager.LoadScene("Credits Screen");
    }

    public void LoadOptionsScreen()
    {
        SceneManager.LoadScene("Options Screen");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
