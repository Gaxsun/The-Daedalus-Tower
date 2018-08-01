using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitEffect : MonoBehaviour {

    public Light light1;
    public Light light2;
    float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time >= startTime + 0.1) {
            light1.intensity -= Time.deltaTime*10;
            light2.intensity -= Time.deltaTime*10;
        }

        if (Time.time >= startTime + 1) {
            Destroy(this);
        }

	}
}
