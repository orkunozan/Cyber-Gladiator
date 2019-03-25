using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthLoot : MonoBehaviour {

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            GetComponent<PhotonView>().RPC("DeActiveHealth", PhotonTargets.All, false);
        
        }
      
    }









    [PunRPC]
    void DeActiveHealth( bool setActive)
    {
        
        gameObject.SetActive(setActive);
    }


}
