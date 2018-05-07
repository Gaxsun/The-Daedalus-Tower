using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSpawner : MonoBehaviour {
    public GameObject[] enemies;
    private GameObject[] fighters;

    public bool arenaBegin;
    public bool arenaClear;
    private bool spawned;

	// Use this for initialization
	void Start () {
        fighters = enemies;
        arenaBegin = false;
        arenaClear = false;
        spawned = true;
	}
	
	// Update is called once per frame
	void Update () {
        bool corpsePile = true;
        if (arenaBegin && spawned) {
            for (int i = 0; i < enemies.Length; i++) {
                fighters[i] = Instantiate(enemies[i], transform);
            }
            spawned = false;
        }else if(arenaBegin && arenaClear == false) {
            foreach(GameObject enemy in enemies) {
                if(enemy != null) {
                    corpsePile = false;
                }
            }
            if (corpsePile) {
                arenaClear = true;
            }
        }
	}
}
