using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
    Vector3 rPosition = new Vector3(0,0,0);
    Quaternion rRotation = Quaternion.identity;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine)
        {

        }else
        {
            transform.position = Vector3.Lerp(transform.position, this.rPosition, 10f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.rRotation, 10f * Time.deltaTime);
        }
	}
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }else
        {
            this.rPosition = (Vector3)stream.ReceiveNext();
            this.rRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
