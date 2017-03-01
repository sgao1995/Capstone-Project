using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {
	float rotation = 0f;
	
	public void Interact(){
		Debug.Log("Picked up Key");
        transform.GetComponent<PhotonView>().RPC("destroyKey", PhotonTargets.MasterClient);
    }
    [PunRPC]
    void destroyKey()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
	// a rotating animation
	void Update(){
		rotation += 1.5f;
		Quaternion newQuat = Quaternion.Euler(0f, rotation, 0f);
		this.transform.rotation = newQuat;
	}
}
