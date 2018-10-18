using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class resspawnTracker : MonoBehaviour {

    public bool hasDiedBefore = false;
    public bool lvRestarted = false;
    public bool hasReachedBossRoom = false;
    public Vector3 respawnLocation;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        print(SceneManager.GetActiveScene().name == "Daedalus Menu");
        if (SceneManager.GetActiveScene().name == "Daedalus Menu") {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        if (hasReachedBossRoom) {
            respawnLocation = GameObject.FindWithTag("outsideBossRoomRespawn").transform.position;
        }
        if (lvRestarted && hasDiedBefore && respawnLocation != null && hasReachedBossRoom && respawnLocation != new Vector3(0,0,0)) {
            GameObject.FindWithTag("Player").transform.position = respawnLocation;
            
        }
        lvRestarted = false;
    }
}
