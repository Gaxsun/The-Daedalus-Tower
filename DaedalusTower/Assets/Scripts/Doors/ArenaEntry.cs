using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEntry : MonoBehaviour {
    private  Vector3 doorStartPos;
    private Vector3 startPos;
	// Use this for initialization
	void Start () {
        doorStartPos = transform.parent.localPosition;
        startPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(startPos.x, (doorStartPos.y - transform.parent.localPosition.y)/transform.parent.localScale.y, startPos.z);
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GetComponentInParent<ArenaDoor>().arenaBegin = true;
        }
    }
}
