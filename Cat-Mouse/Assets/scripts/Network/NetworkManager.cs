using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour
{
    const string VER = "0.0.1";
    // Use this for initialization
    //private const string room = "room";
 public Maze mazePrefab;
	private Maze mazeInstance;
    Spawn[] s;
    //private RoomInfo[] rList;
	void Start () {
        PhotonNetwork.ConnectUsingSettings(VER);
        s = GameObject.FindObjectsOfType<Spawn>();	
	}
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

    }
    void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed");
        PhotonNetwork.CreateRoom(null);
    }
    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        SpawnCat();
        
       
        mazeInstance = Instantiate(mazePrefab) as Maze;
        var mazeScript = mazeInstance.GetComponent<Maze>();
        if(mazeScript != null)
        {	
            mazeScript.StartMazeCreation(); 
        }
        
        IntVector2 spawnPoint = new IntVector2(0, 0);
    }
    void SpawnCat()
    {
        Spawn mys = s[Random.Range(0,s.Length)];
        GameObject myCat = (GameObject)PhotonNetwork.Instantiate("Cat", mys.transform.position, mys.transform.rotation, 0);
        myCat.GetComponent<CatMovement>().enabled=true;
        myCat.transform.FindChild("CatCam").gameObject.SetActive(true);
        //myCat.GetComponent<NetworkPlayer>().enabled = false;
        //enables minimap:
        myCat.GetComponent<Minimap>().enabled = true;
    }
}
