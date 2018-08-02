using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaDoor : MonoBehaviour {
    private bool openSesame;
    private float closedHeight;

    public float openHeight;
    public float liftSpeed;

    public GameObject[] arenaSpawners;
    public GameObject[] otherDoors;
    public bool arenaBegin;
    public bool staggeredSpawns;
    private bool arenaEnd;

    // Use this for initialization
    void Start() {
        openSesame = false;
        arenaEnd = false;
        closedHeight = transform.position.y;
    }

    // Ben was here, dab
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
            if (staggeredSpawns) {
                GetComponent<SpawnTrigger>().triggered = true;
                foreach(GameObject spawner in arenaSpawners) {
                    if(spawner.GetComponent<BasicSpawner>().corpsePile == false) {
                        corpsePile = false;
                    }
                }
            } else {
                foreach (GameObject spawner in arenaSpawners) {
                    spawner.GetComponent<ArenaSpawner>().arenaBegin = arenaBegin;
                    if (spawner.GetComponent<ArenaSpawner>().arenaClear == false) {
                        corpsePile = false;
                    }
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

        foreach (GameObject door in otherDoors) {
            door.GetComponent<ArenaDoor>().openSesame = openSesame;
            door.GetComponent<ArenaDoor>().arenaBegin = arenaBegin;
            door.GetComponent<ArenaDoor>().arenaEnd = arenaEnd;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            openSesame = true;
        }
    }
}
