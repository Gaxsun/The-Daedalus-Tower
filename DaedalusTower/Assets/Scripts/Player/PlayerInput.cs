﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    CursorLockMode wantedMode;

    public bool controlsEnabled = true;
    bool pausePrimed;

    // Use this for initialization
    void Start () {
        //Lock Cursor
        wantedMode = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {

        Cursor.lockState = wantedMode;

        //Pause();      

        if (controlsEnabled) {
            inputs();
        }  
         
	}

    private void inputs() {
        /*
         * 
         * 
         * 
         * Keyboard Input (obsolete)
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

        if (Input.GetAxis("Mouse X") != 0) {
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

        if (Input.GetAxis("X") != 0) {
            GetComponent<PlayerMovement>().playerAttack();
        }

        if (Input.GetAxis("A") != 0) {
            GetComponent<PlayerMovement>().Jump();
        }

        if (Input.GetAxis("RightBumper") != 0) {
            if (GetComponent<PlayerMovement>().dashEnabled) {
                GetComponent<PlayerMovement>().DashStart();
            }           
        }

        if (Input.GetAxis("RightStickX") != 0) {
            GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().cameraRotate(Input.GetAxis("RightStickX"));
        }
    }

    /*private void Pause() {
        if (Input.GetAxisRaw("StartButton") != 0) {
            pausePrimed = true; 
        }
        
        if (pausePrimed && Input.GetAxisRaw("StartButton") == 0) {
            if (Time.timeScale == 1) {
                Time.timeScale = 0F;
                GetComponent<playerManager>().inventoryWindow.enabled = true;
                controlsEnabled = false;
            } else {
                
                Time.timeScale = 1F;
                GetComponent<playerManager>().inventoryWindow.enabled = false;
                controlsEnabled = true;
            }
            pausePrimed = false;
        }
    }*/
}
