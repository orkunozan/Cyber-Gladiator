using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : Photon.MonoBehaviour {
    Vector3 position;
    Quaternion rotation;
    float latency = 10;
    
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position,position, latency * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, latency * Time.deltaTime);
        }

		
	}


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
