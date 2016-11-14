using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour
{
    const string VER = "0.0.1";
    // Use this for initialization
    //private const string room = "room";
    //private RoomInfo[] rList;
	void Start () {
        PhotonNetwork.ConnectUsingSettings(VER);	
	}
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

    }
    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }
    void OnJoinedRoom()
    {
        GameObject myCat = PhotonNetwork.Instantiate("Cat", Vector3.zero, Quaternion.identity, 0); 
    }
}
