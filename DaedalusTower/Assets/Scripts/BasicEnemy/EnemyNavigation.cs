using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour {
    
    public GameObject player;


    public Transform destinationPoint;
    public float minDistance;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(Vector3.Distance(player.transform.position, transform.position) <= minDistance) {
            destinationPoint.position = transform.position;
        }else {
            destinationPoint.position = player.transform.position;
        }
        transform.GetComponent<NavMeshAgent>().destination = destinationPoint.position;
    }
}
