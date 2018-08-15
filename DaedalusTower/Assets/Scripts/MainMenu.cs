using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Button playButton;
    public Slider optionSlider;
    public Button backButton;

    public void PlayGame ()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
