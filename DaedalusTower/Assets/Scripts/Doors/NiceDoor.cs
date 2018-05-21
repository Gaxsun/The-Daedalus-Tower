using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiceDoor : MonoBehaviour {
    private bool openSesame;
    private float closedHeight;

    public float openHeight;
    public float liftSpeed;

    public bool triggered;
	// Use this for initialization
	void Start () {
        openSesame = false;
        closedHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (openSesame && transform.position.y < openHeight) {
            transform.position += transform.up * liftSpeed * Time.deltaTime;
        }else if(openSesame == false && transform.position.y > closedHeight){
            transform.position -= transform.up * liftSpeed * Time.deltaTime;
        }else if(openSesame == false){
            transform.position = new Vector3(transform.position.x, closedHeight, transform.position.z);
        }
        GetComponent<SpawnTrigger>().triggered = triggered;
	}

    private void OnTriggerEnter(Collider other) {
        openSesame = true;
        triggered = true;
    }

    private void OnTriggerStay(Collider other) {
        openSesame = true;
        triggered = true;
    }

    private void OnTriggerExit(Collider other) {
        openSesame = false;
        triggered = false;
    }
}
