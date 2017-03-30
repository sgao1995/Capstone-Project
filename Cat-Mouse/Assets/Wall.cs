using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void DestroyWall()
    {
        transform.GetComponent<PhotonView>().RPC("DestroyWallRPC", PhotonTargets.AllBuffered);
    }
    [PunRPC]
    void DestroyWallRPC()
    {
        Destroy(this.gameObject);

    }
    // Update is called once per frame
    void Update () {
	
	}
}
