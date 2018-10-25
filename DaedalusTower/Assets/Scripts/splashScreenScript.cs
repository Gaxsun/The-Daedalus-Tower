using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splashScreenScript : MonoBehaviour {

    public float timerLength = 5;
    public float timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= timerLength) {
            SceneManager.LoadScene("Daedalus Menu");
        }
	}
}
