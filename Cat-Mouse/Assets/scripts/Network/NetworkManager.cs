using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
    const string VER = "0.0.1";
	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings(VER);	
	}
    void OnJoinLobby()
    {

    }
}
