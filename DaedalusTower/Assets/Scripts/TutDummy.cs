using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutDummy : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (FindObjectOfType<TextBoxManager>() && FindObjectOfType<TextBoxManager>().trainingDummy != gameObject) {
            FindObjectOfType<TextBoxManager>().trainingDummy = gameObject;
        }
        if (GetComponent<Enemy>().health <= 0) {
            Destroy(gameObject);
        }
        if (GameObject.FindGameObjectWithTag("respawnTracker").GetComponent<resspawnTracker>().hasDiedBefore) {
            GetComponent<Enemy>().health = 0;
        }
    }
}
