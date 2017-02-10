using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class matchSettings : Photon.PunBehaviour
{

	// Use this for initialization
	void Start () {
        PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.automaticallySyncScene = true;
        //if (PhotonNetwork.isMasterClient) {
         //   GameObject StartBtn = GameObject.Find("Start Button");
        //    StartBtn.SetActive(true);
        //}
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ButtonEvents(string Event)
    {
        switch (Event)
        {
            case "StartBtn":
                //if (PhotonNetwork.JoinLobby())
               // {
                    PhotonNetwork.LoadLevel("catmousegame3");
               // }
                break;
        }      
    }
    private List<GameObject> roomPF = new List<GameObject>();
    public GameObject roomPreFab;
    
}
