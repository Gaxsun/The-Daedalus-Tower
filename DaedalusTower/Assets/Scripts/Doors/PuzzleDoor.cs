using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour {
    private bool openSesame;
    private float closedHeight;

    public float openHeight;
    public float liftSpeed;

    public string password;
    public string input;
    public GameObject[] displayNumbers;

    // Use this for initialization
    void Start() {
        openSesame = false;
        closedHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update() {
        if (openSesame && transform.position.y < openHeight) {
            transform.position += transform.up * liftSpeed * Time.deltaTime;
        } else if (openSesame == false && transform.position.y > closedHeight) {
            transform.position -= transform.up * liftSpeed * Time.deltaTime;
        } else if (openSesame == false) {
            transform.position = new Vector3(transform.position.x, closedHeight, transform.position.z);
        }
        input = "";
        foreach (GameObject number in displayNumbers) {
            input = input + number.GetComponent<TextMesh>().text;
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "Player" && input == password) {
            openSesame = true;
            BoxCollider[] buttonRegister = GetComponentsInChildren<BoxCollider>();
            foreach(BoxCollider collider in buttonRegister) {
                collider.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            openSesame = false;
        }
    }
}
