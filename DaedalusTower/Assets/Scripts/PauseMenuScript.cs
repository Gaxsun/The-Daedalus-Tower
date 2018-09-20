using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

    public Button mainButton;
    public Button optionSlider;
    public Button backButton;

    public void ReturntoMain ()
    {
        SceneManager.LoadScene("Daedalus Menu");
    }

    public void menuOpen() {
        mainButton.Select();
    }

    public void OpenOptions() {
        optionSlider.Select();
    }

    public void OpenControls() {
        backButton.Select();
    }
}
