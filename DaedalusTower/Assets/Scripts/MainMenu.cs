using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Button playButton;
    public Slider optionSlider;
    public Button backButton;
    public bool continueButton = false;

    //CursorLockMode wantedMode;

    //public void Awake() {
    //wantedMode = CursorLockMode.Locked;
    // }

    //public void Update() {
    //Cursor.lockState = wantedMode;
    // }


    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public Button pressToContinue;

    public void PlayGame (int sceneIndex)
	{
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        loadingScreen.SetActive(true);
        

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            progressText.text = progress * 100f + "%";
            if (progress >= 1.0f)
            {
                progressText.gameObject.SetActive(false);
                pressToContinue.gameObject.SetActive(true);
            }
            else
            {
                pressToContinue.gameObject.SetActive(false);
            }

            yield return null;

            if (continueButton)
            {
                operation.allowSceneActivation = true;
            }
            else
            {
                operation.allowSceneActivation = false;
            }
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenMenu() {
        playButton.Select();
    }

    public void OpenOptions() {
        optionSlider.Select();
    }

    public void OpenControls()
    {
        backButton.Select();
    }

    public void ContinueScene()
    {
        continueButton = true;
    }
}
