using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaDoor : MonoBehaviour {
    private bool openSesame;
    private float closedHeight;

    public float openHeight;
    public float liftSpeed;

    public GameObject[] arenaSpawners;
    public bool arenaBegin;
    private bool arenaEnd;

    // Use this for initialization
    void Start() {
        openSesame = false;
        arenaEnd = false;
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
        if (arenaBegin && arenaEnd == false) {
            openSesame = false;
            bool corpsePile = true;
            foreach (GameObject spawner in arenaSpawners) {
                spawner.GetComponent<ArenaSpawner>().arenaBegin = arenaBegin;
                if (spawner.GetComponent<ArenaSpawner>().arenaClear == false) {
                    corpsePile = false;
                }
            }
            if (corpsePile) {
                arenaEnd = true;
            }
            transform.position = new Vector3(transform.position.x, closedHeight, transform.position.z);
        }
        if (arenaEnd && transform.position.y < openHeight) {
            openSesame = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            openSesame = true;
        }
    }
}
