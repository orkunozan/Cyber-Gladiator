using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonCharacterView :Photon.MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(photonView.isMine)
        {
            GetComponent < karakter >().enabled= true;
            GetComponentInChildren<Camera>().enabled = true;
            
        }
		
	}
}
