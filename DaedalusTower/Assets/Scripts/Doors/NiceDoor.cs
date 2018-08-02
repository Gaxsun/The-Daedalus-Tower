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
        closedHeight = transform.localPosition.y;
	}
	
	// Update is called once per frame
	//niceone
	void Update () {
        if (openSesame && transform.localPosition.y < openHeight) {
            transform.localPosition += transform.up * liftSpeed * Time.deltaTime;
        }else if(openSesame == false && transform.localPosition.y > closedHeight){
            transform.localPosition -= transform.up * liftSpeed * Time.deltaTime;
        }else if(openSesame == false){
            transform.localPosition = new Vector3(transform.localPosition.x, closedHeight, transform.localPosition.z);
        }
        GetComponent<SpawnTrigger>().triggered = triggered;
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Projectile") {
            openSesame = true;
            if (other.tag == "Player") {
                triggered = true;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag != "Projectile") {
            openSesame = true;
            if (other.tag == "Player") {
                triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag != "Projectile") {
            openSesame = false;
            if (other.tag == "Player") {
                triggered = false;
            }
        }
    }
}
