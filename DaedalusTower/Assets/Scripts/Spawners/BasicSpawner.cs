using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawner : MonoBehaviour {
    public GameObject[] enemies;
    private GameObject[] fighters;

    public GameObject spawnTrigger;

    private bool spawn;
    public bool corpsePile;

    // Use this for initialization
    void Start() {
        fighters = enemies;
        spawn = true;
        corpsePile = false;
    }

    // Update is called once per frame
    void Update() {
        if (spawn && spawnTrigger.GetComponent<SpawnTrigger>().triggered) {
            for (int i = 0; i < enemies.Length; i++) {
                fighters[i] = Instantiate(enemies[i], transform);
            }
            spawn = false;
        }
        corpsePile = true;
        foreach (GameObject enemy in fighters) {
            if (enemy != null) {
                corpsePile = false;
            }
        }
    }
}
