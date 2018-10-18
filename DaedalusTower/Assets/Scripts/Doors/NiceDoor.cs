using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiceDoor : MonoBehaviour {
    private bool openSesame;
    private float closedHeight;

    private bool corpsePile;
    public float openHeight;
    public float liftSpeed;

    public bool triggered;
    public GameObject[] preSpawners;
	// Use this for initialization
	void Start () {
        openSesame = false;
        closedHeight = transform.localPosition.y;
	}

    // Update is called once per frame
    //niceone
    void Update() {
        if (openSesame && transform.localPosition.y < openHeight && corpsePile) {
            transform.localPosition += transform.up * liftSpeed * Time.deltaTime;
            GetComponent<OcclusionPortal>().open = true;
        } else if (openSesame == false && transform.localPosition.y > closedHeight) {
            transform.localPosition -= transform.up * liftSpeed * Time.deltaTime;
        } else if (openSesame == false) {
            transform.localPosition = new Vector3(transform.localPosition.x, closedHeight, transform.localPosition.z);
            GetComponent<OcclusionPortal>().open = false;
        }
        if (GetComponent<SpawnTrigger>()) {
            GetComponent<SpawnTrigger>().triggered = triggered;
        }
        corpsePile = true;
        foreach (GameObject spawner in preSpawners) {
            if (spawner.GetComponent<BasicSpawner>().corpsePile == false) {
                corpsePile = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        openSesame = true;
        if (other.tag == "Player") {
            triggered = true;
        }

    }

    private void OnTriggerStay(Collider other) {
        openSesame = true;
        if (other.tag == "Player") {
            triggered = true;
        }

    }

    private void OnTriggerExit(Collider other) {
        openSesame = false;
        if (other.tag == "Player") {
            triggered = false;
        }

    }
}
