using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawner : MonoBehaviour {
    public GameObject[] enemies;
    public GameObject triggerDoor;

    private bool spawn;

    // Use this for initialization
    void Start() {
        spawn = true;
    }

    // Update is called once per frame
    void Update() {
        if (spawn && triggerDoor.GetComponent<NiceDoor>().triggered) {
            for (int i = 0; i < enemies.Length; i++) {
                Instantiate(enemies[i], transform);
            }
            spawn = false;
        }
    }
}
