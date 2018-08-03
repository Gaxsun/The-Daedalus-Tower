using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour {
    private bool openSesame;
    private float closedHeight;

    public float openHeight;
    public float liftSpeed;

    // Use this for initialization
    void Start() {
        openSesame = false;
        closedHeight = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update() {
        if (openSesame && transform.localPosition.y < openHeight) {
            transform.localPosition += transform.up * liftSpeed * Time.deltaTime;
        } else if (openSesame == false && transform.localPosition.y > closedHeight) {
            transform.localPosition -= transform.up * liftSpeed * Time.deltaTime;
        } else if (openSesame == false) {
            transform.localPosition = new Vector3(transform.localPosition.x, closedHeight, transform.localPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "enemy") {
            openSesame = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "enemy") {
            openSesame = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "enemy") {
            openSesame = false;
        }
    }
}
