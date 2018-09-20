using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resspawnTracker : MonoBehaviour {

    public bool hasDiedBefore;
    public GameObject respawnLocation;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        DontDestroyOnLoad(this.gameObject);
        print(hasDiedBefore);
    }
}
