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
	private Powerup powerupsList;
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
		SpawnMonsters();
       
        mazeInstance = Instantiate(mazePrefab) as Maze;
        var mazeScript = mazeInstance.GetComponent<Maze>();
        if(mazeScript != null)
        {	
            mazeScript.StartMazeCreation(); 
        }
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
	void SpawnMonsters()
	{
		Spawn monsterSpawn = s[1];
        GameObject monster = (GameObject)PhotonNetwork.Instantiate("Monster", monsterSpawn.transform.position, monsterSpawn.transform.rotation, 0);
        monster.GetComponent<MonsterAI>().enabled=true;
	}
	public void destroy(){
		
	}
}
