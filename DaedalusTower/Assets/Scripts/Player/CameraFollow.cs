using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float cameraDistance;

    public float minCameraDistance;
    public float cameraPadding;
    
    public Vector3 initOffset;
    public Vector3 springOffset;
    public Vector3 rotateOffset;

    public float rotateSpeed;
    private bool cameraCheck;

	// Use this for initialization
	void Start () {
        initOffset.z = -cameraDistance;
        transform.position = player.transform.position + initOffset;
        rotateOffset = initOffset;
        springOffset = initOffset;
        cameraCheck = false;
        }
	
	// Update is called once per frame
	void Update () {
        springArm();
        if(Mathf.Sqrt(springOffset.x * springOffset.x + springOffset.z * springOffset.z) > cameraDistance) {
            cameraCheck = true;
            springArm();
        }
        transform.position = player.transform.position + springOffset;
        transform.LookAt(player.transform);
        //print(Mathf.Sqrt(springOffset.x * springOffset.x + springOffset.z * springOffset.z));
        //print(Mathf.Sqrt(initOffset.x * initOffset.x + initOffset.z * initOffset.z));
    }

    public void cameraRotate(float mouseValueX) {
        transform.RotateAround(player.transform.position, player.transform.up, rotateSpeed * Time.deltaTime * mouseValueX);
        rotateOffset.x = transform.position.x - player.transform.position.x;
        rotateOffset.z = transform.position.z - player.transform.position.z;

        if (rotateOffset.x > cameraDistance + cameraPadding) {
            rotateOffset.x = cameraDistance;
        } else if (rotateOffset.x < -cameraDistance - cameraPadding) {
            rotateOffset.x = -cameraDistance;
        }

        if (rotateOffset.z > cameraDistance + cameraPadding) {
            rotateOffset.z = cameraDistance;
        } else if (rotateOffset.z < -cameraDistance - cameraPadding) {
            rotateOffset.z = -cameraDistance;
        }

        springArm();

        if (springOffset.x > cameraDistance + cameraPadding) {
            springOffset.x = cameraDistance;
        } else if (springOffset.x < -cameraDistance - cameraPadding) {
            springOffset.x = -cameraDistance;
        }

        if (springOffset.z > cameraDistance + cameraPadding) {
            springOffset.z = cameraDistance;
        } else if (springOffset.z < -cameraDistance - cameraPadding) {
            springOffset.z = -cameraDistance;
        }
    }

    public void playerCounterRotate() {
        rotateOffset.x = transform.position.x - player.transform.position.x;
        rotateOffset.z = transform.position.z - player.transform.position.z;
        if (rotateOffset.x > cameraDistance + cameraPadding) {
            rotateOffset.x = cameraDistance;
        } else if (rotateOffset.x < -cameraDistance - cameraPadding) {
            rotateOffset.x = -cameraDistance;
        }

        if (rotateOffset.z > cameraDistance + cameraPadding) {
            rotateOffset.z = cameraDistance;
        } else if (rotateOffset.z < -cameraDistance - cameraPadding) {
            rotateOffset.z = -cameraDistance;
        }

        springArm();

        if (springOffset.x > cameraDistance + cameraPadding) {
            springOffset.x = cameraDistance;
        } else if (springOffset.x < -cameraDistance - cameraPadding) {
            springOffset.x = -cameraDistance;
        }

        if (springOffset.z > cameraDistance + cameraPadding) {
            springOffset.z = cameraDistance;
        } else if (springOffset.z < -cameraDistance - cameraPadding) {
            springOffset.z = -cameraDistance;
        }
    }

    public void springArm() {
        RaycastHit hit;
        Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.blue);
        if (Physics.Raycast(player.transform.position, transform.position - player.transform.position, out hit, cameraDistance)) {
            Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.red);
            if (hit.collider.tag == "terrain") {
                cameraCheck = true;
                springOffset.x = hit.point.x - player.transform.position.x;
                springOffset.z = hit.point.z - player.transform.position.z;
                
            } else {
                springOffset.x = rotateOffset.x;
                springOffset.z = rotateOffset.z;
            }
        } else {
            if (cameraCheck) {
                cameraDistanceReset();
                springOffset.x = initOffset.x;
                springOffset.z = initOffset.z;
                print(springOffset.z);
                cameraCheck = false;
            }else {
                springOffset.x = rotateOffset.x;
                springOffset.z = rotateOffset.z;
            }
        }
    }

    private void cameraDistanceReset() {
        Vector3 xPoint;
        Vector3 zPoint;
        float newX;
        float newZ;
        
        xPoint = new Vector3(Mathf.Sqrt(cameraDistance*cameraDistance - rotateOffset.z*rotateOffset.z), 0, rotateOffset.z);
        zPoint = new Vector3(rotateOffset.x, 0, Mathf.Sqrt(cameraDistance * cameraDistance - rotateOffset.x * rotateOffset.x));

        if(rotateOffset.x < 0) {
            xPoint.x *= -1;
        }
        if(rotateOffset.z < 0) {
            zPoint.z *= -1;
        }

        //Find Midpoint on circle at cameraDistance
        newX = cameraDistance * (xPoint.x + zPoint.x) / (Mathf.Sqrt((xPoint.x + zPoint.x) * (xPoint.x + zPoint.x) + (xPoint.z + zPoint.z) * (xPoint.z + zPoint.z)));
        if (newX == 0) {
            newZ = zPoint.z;
        } else {
            newZ = (xPoint.z + zPoint.z) / (xPoint.x + zPoint.x) * newX;
        }
        
        initOffset = new Vector3(newX, initOffset.y, newZ);

        if (initOffset.x > cameraDistance + cameraPadding) {
            initOffset.x = cameraDistance;
        } else if (initOffset.x < -cameraDistance - cameraPadding) {
            initOffset.x = -cameraDistance;
        }

        if (springOffset.z > cameraDistance + cameraPadding) {
            initOffset.z = cameraDistance;
        } else if (springOffset.z < -cameraDistance - cameraPadding) {
            initOffset.z = -cameraDistance;
        }
    }
}
