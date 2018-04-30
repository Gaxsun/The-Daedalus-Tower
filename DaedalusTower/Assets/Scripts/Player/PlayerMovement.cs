using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Camera playerCam;

    public float forwardMoveSpeed;
    //public float backwardMoveSpeed;
    public float sidewaysMoveSpeed;

    public float rotateSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void forwardAxisMovement(float direction) {
        transform.position = transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z).normalized * forwardMoveSpeed * -direction * Time.deltaTime;
        transform.LookAt(transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z) * -direction);
    }

    public void sidewaysAxisMovement(float direction) {
        transform.RotateAround(playerCam.transform.position, transform.up, rotateSpeed * direction * Time.deltaTime);
        transform.LookAt(transform.position + playerCam.transform.right * direction);
    }
}
