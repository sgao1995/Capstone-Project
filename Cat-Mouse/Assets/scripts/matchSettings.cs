using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class matchSettings : Photon.PunBehaviour
{
    static bool isCat = false;
    static bool isMice = false;
    static int catLimit = 0;
    static int miceLimit = 0;
    static bool teamChosen = false;
    // Use this for initialization
    
    void Start()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.automaticallySyncScene = true;
        if (PhotonNetwork.isMasterClient==false) {
            GameObject StartBtn = GameObject.Find("StartButton");
            StartBtn.SetActive(false);
            Debug.Log("you are not Master");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonEvents(string Event)
    {
        switch (Event)
        {
            case "StartBtn":
                //if (PhotonNetwork.JoinLobby())
                // {
                if(teamChosen == true)
                {
                    //GameObject.Find("MainCanvas").transform.FindChild("Panel").transform.FindChild("StartButton").GetComponentInChildren<Image>()
                    PhotonNetwork.LoadLevel("catmousegame3");
                }else
                {
                    Debug.Log("Choose a Team");
                }
                // }
                break;
            case "TeamCat":
                    if(isCat == false)
                    {
                   // if (GameObject.Find("MainCanvas").transform.FindChild("Panel").transform.FindChild("StartButton").GetComponentInChildren<Image> == null)
                    //{
                    //    Debug.Log("NOTHING");
                   // }
                    //else
                    //{
                    //    Debug.Log("FOUND");
                   // }
                        isCat = true;
                        isMice = false;
                        Debug.Log("Player " + PhotonNetwork.player.ID + "Joined as Cat"+isCat+isMice);
                        teamChosen = true;
                        GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 0;
                    }  
                break;
            case "TeamMice":      
                    if (isMice == false)
                    {
                        isMice = true;
                        isCat = false;
                        Debug.Log("Player " + PhotonNetwork.player.ID + "Joined as Mouse" + isCat + isMice);
                        teamChosen = true;
                        GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                }
                break;
        }
    }

}
