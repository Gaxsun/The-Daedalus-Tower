using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {

    CursorLockMode wantedMode;

    public bool controlsEnabled = true;
    bool pausePrimed;
    float lockTimer;

    private bool tutorial = true;

    [Header("LockOn Settings")]
    public float lockDelay;
    bool liveTarget;
    RaycastHit lockTarget;
    public GameObject lockCircle;
    GameObject currentCircle;
    GameObject targetEnemy;
    public Color targetFade;

    public AudioClip[] inputSounds;
    public AudioSource inputSoundsSource;
   
    public Color targetActive;

    [Header("UI Objects")]
    public GameObject textBox;
    public Button pauseAutoSelect;
    // Use this for initialization
    void Start () {
        //Lock Cursor
        pausePrimed = true;

        if (GameObject.FindWithTag("respawnTracker") != null && GameObject.FindWithTag("respawnTracker").GetComponent<resspawnTracker>().hasDiedBefore) {
            pausePrimed = false;
        }

        wantedMode = CursorLockMode.Locked;
        Cursor.visible = false;
        lockTimer = 0;

        if (GameObject.FindWithTag("respawnTracker") != null && GameObject.FindWithTag("respawnTracker").GetComponent<resspawnTracker>().hasDiedBefore) {
            //introTut.enabled = false;
        }
    }
	


	// Update is called once per frame
	void Update () {

        GetComponent<playerManager>().can.enabled = controlsEnabled;
        
        Cursor.lockState = wantedMode;
        Pause();      

        if (controlsEnabled) {
            inputs();
        }
        lockingOn();
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
            if(inputSoundsSource.isPlaying == false && GetComponent<PlayerMovement>().isAirBorne == false)
            {
                inputSoundsSource.clip = inputSounds[0];
                inputSoundsSource.loop = true;
                inputSoundsSource.Play();
            }
        } else {
            GetComponent<PlayerMovement>().playIdle();
            if(inputSoundsSource.clip == inputSounds[0])
            {
                inputSoundsSource.Stop();
                inputSoundsSource.clip = null;
                inputSoundsSource.loop = false;
            }
        }

        GetComponent<PlayerMovement>().forwardAxisMovement(Input.GetAxis("LeftStickY"));
        GetComponent<PlayerMovement>().sidewaysAxisMovement(Input.GetAxis("LeftStickX"));

        //change these to get button down
        if (Input.GetButtonDown("X")) {
            GetComponent<Animator>().SetInteger("nextAttack", 1);

            int randNum = Mathf.RoundToInt(Random.Range(0, 30));

            if (randNum >= 20 && inputSoundsSource.isPlaying == false)
            {
                inputSoundsSource.Stop();
                inputSoundsSource.clip = inputSounds[3];
                inputSoundsSource.loop = false;
                inputSoundsSource.Play();
            }
            else if (randNum >= 10 && inputSoundsSource.isPlaying == false)
            {
                inputSoundsSource.Stop();
                inputSoundsSource.clip = inputSounds[4];
                inputSoundsSource.loop = false;
                inputSoundsSource.Play();
            }

        }

        if(Input.GetButtonDown("Y")) {
            GetComponent<Animator>().SetInteger("nextAttack", 2);

            //Random.InitState(Mathf.RoundToInt(Time.time) * Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z));
            int randNum = Mathf.RoundToInt(Random.Range(0, 50));
            print(randNum);
            if (randNum >= 39 && inputSoundsSource.clip != inputSounds[2])
            {
                inputSoundsSource.Stop();
                inputSoundsSource.clip = inputSounds[2];
                inputSoundsSource.loop = false;
                inputSoundsSource.Play();
            }

        }

        if (Input.GetAxis("A") != 0) {
            GetComponent<PlayerMovement>().Jump();
        }

        if (Input.GetButtonDown("B")) {
            if (GetComponent<playerManager>().powerOfGods == GetComponent<playerManager>().powerOfGodsMax) {
                GetComponent<playerManager>().powerOfGodsActive = true;
                GetComponent<playerManager>().GetComponent<PlayerMovement>().moddableSpeed = GetComponent<PlayerMovement>().moddableSpeed * GetComponent<playerManager>().powerOfGodsSpeedBoost;
                GetComponent<playerManager>().GetComponent<PlayerMovement>().speed = GetComponent<PlayerMovement>().speed * GetComponent<playerManager>().powerOfGodsSpeedBoost;
                GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().baseDamage = Mathf.RoundToInt(GetComponent<playerManager>().powerOfGodsDamageBoost * GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().baseDamage);
                GetComponent<playerManager>().powerOfGods -= GetComponent<playerManager>().powerOfGodsDecayRate * Time.deltaTime;
                //GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().baseDamage = GetComponent<Weapon>().baseDamage * GetComponent<playerManager>().powerOfGodsDamageBoost;
            } else {
                GetComponent<playerManager>().endGodPower();
            }
            
        }

        if (Input.GetAxis("RightBumper") != 0) {
            if (GetComponent<PlayerMovement>().dashEnabled) {
                GetComponent<PlayerMovement>().DashStart();
            }           
        }

        if (Input.GetAxis("RightStickX") != 0 && GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn == false) {
            GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().cameraRotate(Input.GetAxis("RightStickX"));
        }

        if (Input.GetAxis("RightStickClick") != 0) {
        
            if (Time.time > (lockTimer + lockDelay)) {
                lockTimer = Time.time;
                if (GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn) {
                    GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn = false;
                } else {
                    if (liveTarget && lockTarget.collider.gameObject.tag == "enemy") {
                        GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockTarget = lockTarget.collider.gameObject;
                        GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn = true;
                    } else if (liveTarget && lockTarget.collider.gameObject.tag == "mistwalker") {
                        GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockTarget = lockTarget.collider.gameObject;
                        GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn = true;
                    }
                }
            }
        }
    }

    private void lockingOn() {
        Debug.DrawRay(GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().playerLocation, new Vector3(GetComponent<PlayerMovement>().playerCam.transform.forward.x, 0, GetComponent<PlayerMovement>().playerCam.transform.forward.z) * 30, Color.yellow, 1);
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        if (Physics.BoxCast(GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().playerLocation, transform.localScale / 4, new Vector3(GetComponent<PlayerMovement>().playerCam.transform.forward.x, 0, GetComponent<PlayerMovement>().playerCam.transform.forward.z),
                                out lockTarget, transform.rotation, 30, layerMask) && lockTarget.collider.gameObject.tag == "enemy") {
            liveTarget = true;
            if (!currentCircle) {
                targetEnemy = lockTarget.collider.gameObject;
                currentCircle = Instantiate(lockCircle, targetEnemy.transform.position, Quaternion.identity);
            }
        } else if (Physics.BoxCast(GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().playerLocation, transform.localScale / 4, new Vector3(GetComponent<PlayerMovement>().playerCam.transform.forward.x, 0, GetComponent<PlayerMovement>().playerCam.transform.forward.z),
                                    out lockTarget, transform.rotation, 30) && lockTarget.collider.gameObject.tag == "mistwalker") {
            liveTarget = true;
            if (!currentCircle) {
                targetEnemy = lockTarget.collider.gameObject;
                currentCircle = Instantiate(lockCircle, new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y+2, targetEnemy.transform.position.z), Quaternion.identity);
            }
        } else {
            liveTarget = false;
        }

        if (currentCircle) {
            if(lockTarget.collider.gameObject != targetEnemy && !GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn) {
                targetEnemy = lockTarget.collider.gameObject;
            }
            currentCircle.transform.position = new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y + 2, targetEnemy.transform.position.z);
            currentCircle.transform.LookAt(GetComponent<PlayerMovement>().playerCam.transform);
        }
        if (!GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn && !liveTarget) {
            Destroy(currentCircle);
        } else if (!GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn) {
            currentCircle.GetComponent<TextMesh>().color = targetFade;
        } else {
            currentCircle.GetComponent<TextMesh>().color = targetActive;

        }
    }
    private void Pause() {
        if (Input.GetAxisRaw("StartButton") != 0) {
            pausePrimed = true; 
        }
        
        if (pausePrimed && Input.GetAxisRaw("StartButton") == 0) {
            if (Time.timeScale == 1) {
                Time.timeScale = 0F;
                GetComponent<playerManager>().pauseMenu.enabled = true;
                pauseAutoSelect.Select();
                controlsEnabled = false;
            } else {
                if (tutorial)
                {
                    GetComponent<playerManager>().introTut.enabled = false;
                    textBox.SetActive(true);
                    tutorial = false;
                }
                
                Time.timeScale = 1F;
                GetComponent<playerManager>().pauseMenu.enabled = false;
                controlsEnabled = true;
            }
            pausePrimed = false;
        }
    }
}
