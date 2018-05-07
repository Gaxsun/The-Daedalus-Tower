using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSpawner : MonoBehaviour {
    public GameObject[] enemies;
    public bool arenaBegin;

	// Use this for initialization
	void Start () {
        arenaBegin = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (arenaBegin) {
            foreach(GameObject enemy in enemies) {
                Instantiate(enemy, transform);
            }
        }
	}
}
