using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Camera playerCam;

    public float forwardMoveSpeed;
    //public float backwardMoveSpeed;
    public float sidewaysMoveSpeed;

    public float rotateSpeed;

    public Vector3 playerDirection;
    private Vector3 playerLocalDirection;

	// Use this for initialization
	void Start () {
        playerDirection = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void forwardAxisMovement(float direction) {
        transform.position = transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z).normalized * forwardMoveSpeed * -direction * Time.deltaTime;
        playerCam.transform.position = playerCam.transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z).normalized * forwardMoveSpeed * -direction * Time.deltaTime;
        setRotation();
    }

    public void sidewaysAxisMovement(float direction) {
        transform.RotateAround(playerCam.transform.position, transform.up, rotateSpeed * direction * Time.deltaTime);
        playerCam.GetComponent<CameraFollow>().playerCounterRotate();
        setRotation();
    }

    private void setRotation() {
        playerDirection = new Vector3((transform.forward.x - transform.position.x) * Input.GetAxis("Horizontal"),0,0));
            
            
            
            //(transform.forward - transform.position) * new Vector3(Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
        transform.rotation = Quaternion.LookRotation(playerDirection, transform.up);
    }
}
