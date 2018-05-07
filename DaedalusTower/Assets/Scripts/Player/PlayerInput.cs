using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    CursorLockMode wantedMode;

	// Use this for initialization
	void Start () {
        //Lock Cursor
        wantedMode = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {

        Cursor.lockState = wantedMode;

        /*
         * 
         * 
         * 
         * Keyboard Input
         * 
         * 
         * 
         * 
         */ 
        
        if (Input.GetAxis("Vertical") != 0) {
            GetComponent<PlayerMovement>().forwardAxisMovement(Input.GetAxis("Vertical") * -1);
        }
        if (Input.GetAxis("Horizontal") != 0) {
            GetComponent<PlayerMovement>().sidewaysAxisMovement(Input.GetAxis("Horizontal"));
        }
        
        if(Input.GetAxis("Mouse X") != 0) {
            GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().cameraRotate(Input.GetAxis("Mouse X"));
        }

        if (Input.GetMouseButtonDown(0)) {
            gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = true;
        }


        /*
         * 
         * 
         * 
         * 
         * Controller Input
         * 
         * 
         * 
         */
        if (Input.GetAxis("LeftStickY") != 0 || Input.GetAxis("LeftStickX") != 0) {
            GetComponent<PlayerMovement>().playRun();
        } else {
            GetComponent<PlayerMovement>().playIdle();
        }

         GetComponent<PlayerMovement>().forwardAxisMovement(Input.GetAxis("LeftStickY"));
         GetComponent<PlayerMovement>().sidewaysAxisMovement(Input.GetAxis("LeftStickX"));
        
        if(Input.GetAxis("X") != 0) {
            GetComponent<PlayerMovement>().playerAttack();
        }

        if(Input.GetAxis("RightStickX") != 0) {
            GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().cameraRotate(Input.GetAxis("RightStickX"));
        } 
	}
}
