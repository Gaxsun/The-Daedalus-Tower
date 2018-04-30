using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float cameraDistance;
    public float defaultCameraDistance;

    public float minCameraDistance;
    public float cameraPadding;
    
    public Vector3 initOffset;
    public Vector3 springOffset;
    public Vector3 rotateOffset;

    public float rotateSpeed;

	// Use this for initialization
	void Start () {
        transform.position = player.transform.position + initOffset;
        rotateOffset = initOffset;
        springOffset = initOffset;
        }
	
	// Update is called once per frame
	void Update () {
        springArm();
        transform.position = player.transform.position + springOffset;
        transform.LookAt(player.transform);
        
    }

    public void cameraRotate(float mouseValueX) {
        transform.RotateAround(player.transform.position, player.transform.up, rotateSpeed * Time.deltaTime * mouseValueX);
        rotateOffset.x = transform.position.x - player.transform.position.x;
        rotateOffset.z = transform.position.z - player.transform.position.z;
        if (rotateOffset.x > cameraDistance) {
            rotateOffset.x = cameraDistance;
        } else if (rotateOffset.x < -cameraDistance) {
            rotateOffset.x = -cameraDistance;
        }

        if (rotateOffset.z > cameraDistance) {
            rotateOffset.z = cameraDistance;
        } else if (rotateOffset.z < -cameraDistance) {
            rotateOffset.z = -cameraDistance;
        }

        springArm();

        if (springOffset.x > cameraDistance) {
            springOffset.x = cameraDistance;
        } else if (springOffset.x < -cameraDistance) {
            springOffset.x = -cameraDistance;
        }

        if (springOffset.z > cameraDistance) {
            springOffset.z = cameraDistance;
        } else if (springOffset.z < -cameraDistance) {
            springOffset.z = -cameraDistance;
        }
    }


    private void springArm() {
        RaycastHit hit;
        Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.blue);
        if (Physics.Raycast(player.transform.position, transform.position - player.transform.position, out hit, defaultCameraDistance)) {
            Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.red);
            if (hit.collider.tag == "terrain") {
                springOffset.x = hit.point.x - player.transform.position.x;
                springOffset.z = hit.point.z - player.transform.position.z;
                
            } else {
                springOffset.x = rotateOffset.x;
                springOffset.z = rotateOffset.z;
            }
        } else {
            cameraDistanceReset();
            springOffset.x = initOffset.x;
            springOffset.z = initOffset.z;
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
        newZ = (xPoint.z + zPoint.z) / (xPoint.x + zPoint.x) * newX;
        
        initOffset = new Vector3(newX, initOffset.y, newZ);

        if (initOffset.x > cameraDistance) {
            initOffset.x = cameraDistance;
        } else if (initOffset.x < -cameraDistance) {
           initOffset.x = -cameraDistance;
        }

        if (springOffset.z > cameraDistance) {
            initOffset.z = cameraDistance;
        } else if (springOffset.z < -cameraDistance) {
            initOffset.z = -cameraDistance;
        }
    }



    /*public void cameraSet() {
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z)) > cameraDistance) {
            transform.position = transform.position + new Vector3(transform.forward.x, 0, transform.forward.z).normalized * player.GetComponent<PlayerMovement>().forwardMoveSpeed * 2 * Time.deltaTime;
        } else if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z)) < cameraDistance - cameraPadding) {
            transform.position = transform.position - new Vector3(transform.forward.x, 0, transform.forward.z).normalized * player.GetComponent<PlayerMovement>().forwardMoveSpeed * 2 * Time.deltaTime;
        } else if (cameraDistance < minCameraDistance) {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }
        transform.position = player.transform.position + springOffset;
        transform.LookAt(player.transform);
    }
    */
}
