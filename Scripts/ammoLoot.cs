using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoLoot : MonoBehaviour {


    


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<PhotonView>().RPC("DeActiveAmmo", PhotonTargets.All, false);
        }
        
    }



    [PunRPC]
    void DeActiveAmmo(bool setActive)
    {

        gameObject.SetActive(setActive);
    }
}
