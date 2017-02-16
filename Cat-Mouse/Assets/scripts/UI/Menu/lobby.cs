using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class lobby : Photon.MonoBehaviour
{
    const string VER = "0.0.1";
    Spawn[] s;
    public Maze mazePrefab;
    private Maze mazeInstance;
    List<int> allPuzzleTypes = new List<int>();
    List<int> activePuzzleTypes = new List<int>();
    public string roomName;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(VER);
        roomName = "Room " + UnityEngine.Random.Range(0, 20);
        PhotonNetwork.isMessageQueueRunning = true;
    }
    void OnGUI()
    {
    }
    void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        Invoke("Refresh", 0.1f);
        Refresh();
    }
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed");
        PhotonNetwork.CreateRoom(null);
    }
    int playerWhoIsIt;
    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if (PhotonNetwork.playerList.Length == 1)
        {
            playerWhoIsIt = PhotonNetwork.player.ID;
        }

        Debug.Log("playerWhoIsIt: " + playerWhoIsIt);
        PhotonNetwork.LoadLevel("Room");
    }

    private List<GameObject> roomPF = new List<GameObject>();
    public GameObject roomPreFab;
    //button events for lobby menu:
    public void ButtonEvents(string Event)
    {
        switch (Event)
        {
            case "CreateBtn":
                if (PhotonNetwork.JoinLobby())
                {
                    RoomOptions roomOpt = new RoomOptions();
                    roomOpt.MaxPlayers = 4;

                    PhotonNetwork.CreateRoom(roomName, roomOpt, TypedLobby.Default);
                }
                //PhotonNetwork.JoinRandomRoom();
                break;
            case "RefreshBtn":
                if (PhotonNetwork.JoinLobby())
                {
                    Refresh();
                }
                break;
            //case "JoinBtn":
                
                //PhotonNetwork.JoinRoom();
              //  break;
        }
    }
    void Refresh()
    {
        if (roomPF.Count > 0)
        {
            for (int i = 0; i < roomPF.Count; i++)
            {
                Destroy(roomPF[i]);
            }
            roomPF.Clear();
        }
        for (int i = 0; i < PhotonNetwork.GetRoomList().Length; i++)
        {
            GameObject r = Instantiate(roomPreFab);
            r.transform.SetParent(roomPreFab.transform.parent);
            r.GetComponent<RectTransform>().localScale = roomPreFab.GetComponent<RectTransform>().localScale;
            r.GetComponent<RectTransform>().position = new Vector3(roomPreFab.GetComponent<RectTransform>().position.x, roomPreFab.GetComponent<RectTransform>().position.y - (i * 55), roomPreFab.GetComponent<RectTransform>().position.z);
            r.transform.FindChild("RText").GetComponent<Text>().text = PhotonNetwork.GetRoomList()[i].name;
            r.transform.FindChild("RText2").GetComponent<Text>().text = "Waiting";
            r.transform.FindChild("RText3").GetComponent<Text>().text = PhotonNetwork.playerList.Length +"/4";
            string roomName = r.transform.FindChild("RText").GetComponent<Text>().text;
            r.GetComponent<Button>().onClick.AddListener(() => { PhotonNetwork.JoinRoom(roomName); });
            r.SetActive(true);
            roomPF.Add(r);
        }
    }

    void join(string r)
    {
        PhotonNetwork.JoinRoom(r);
    }
}
