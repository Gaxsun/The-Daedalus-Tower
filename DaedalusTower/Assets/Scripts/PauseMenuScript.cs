using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

    public void ReturntoMain ()
    {
        SceneManager.LoadScene("Daedalus Menu");
    }

}
