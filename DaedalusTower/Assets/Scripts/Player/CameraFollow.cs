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
        if (rotateOffset.x > 6) {
            rotateOffset.x = 6;
        } else if (rotateOffset.x < -6) {
            rotateOffset.x = -6;
        }

        if (rotateOffset.z > 6) {
            rotateOffset.z = 6;
        } else if (rotateOffset.z < -6) {
            rotateOffset.z = -6;
        }
        springArm();
    }


    private void springArm() {
        RaycastHit hit;
        Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.blue);
        if (Physics.Raycast(player.transform.position, transform.position - player.transform.position, out hit, defaultCameraDistance)) {
            Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.red);
            if (hit.collider.tag == "terrain") {
                springOffset.x = hit.point.x - player.transform.position.x;
                springOffset.z = hit.point.z - player.transform.position.z;
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
        print(springOffset.z * springOffset.z - springOffset.x * springOffset.x + springOffset.z * springOffset.z); // KILL MEEEEEEE
        xPoint = new Vector3(Mathf.Sqrt(springOffset.z*springOffset.z - springOffset.x * springOffset.x + springOffset.z * springOffset.z), 0, springOffset.z);
        zPoint = new Vector3(springOffset.x, 0, Mathf.Sqrt(springOffset.x*springOffset.x - springOffset.x * springOffset.x + springOffset.z * springOffset.z));

        //Find Midpoint on circle at cameraDistance
        newX = cameraDistance * (xPoint.x + zPoint.x) / (Mathf.Sqrt((xPoint.x + zPoint.x) * (xPoint.x + zPoint.x) + (xPoint.z + zPoint.z) * (xPoint.z + zPoint.z)));
        newZ = (xPoint.z + zPoint.z) / (xPoint.x + zPoint.x) * newX;

        initOffset = new Vector3(newX, initOffset.y, newZ);
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
