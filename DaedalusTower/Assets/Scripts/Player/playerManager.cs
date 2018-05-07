using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour {

    //Holds current player config
    public GameObject currentWeapon;
    public GameObject weaponPosition;

	// Use this for initialization
	void Start () {
        // Place selected weapon in player's hand
        Instantiate(currentWeapon, weaponPosition.transform);        
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
