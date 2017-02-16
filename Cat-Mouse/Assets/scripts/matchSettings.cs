using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class matchSettings : Photon.PunBehaviour
{
    static bool isCat = false;
    static bool isMice1 = false;
    static bool isMice2 = false;
    static bool isMice3 = false;
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
        Debug.Log(PhotonNetwork.playerList.Length);

    }
    void JoinCat()
    {
        GameObject.Find("CText").GetComponent<Text>().text = "Player " + PhotonNetwork.player.ID;
    }
    void LeaveCat()
    {
        GameObject.Find("CText").GetComponent<Text>().text = "Spot Open";
    }
    void JoinMouse1()
    {
        GameObject.Find("M1Text").GetComponent<Text>().text = "Player " + PhotonNetwork.player.ID;
    }
    void LeaveM1()
    {
        GameObject.Find("M1Text").GetComponent<Text>().text = "Spot Open";
    }
    void JoinMouse2()
    {
        GameObject.Find("M2Text").GetComponent<Text>().text = "Player " + PhotonNetwork.player.ID;
    }
    void LeaveM2()
    {
        GameObject.Find("M2Text").GetComponent<Text>().text = "Spot Open";
    }
    void JoinMouse3()
    {
        GameObject.Find("M3Text").GetComponent<Text>().text = "Player " + PhotonNetwork.player.ID;
    }
    void LeaveM3()
    {
        GameObject.Find("M3Text").GetComponent<Text>().text = "Spot Open";
    }
    // Update is called once per frame
    void Update()
    {
        //GameObject.Find("scriptLobby").GetComponent<lobby>().playerCount= PhotonNetwork.playerList.Length;
        if(teamChosen)
        {
            GameObject.Find("StartButton").GetComponent<Image>().color = Color.red;
            GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.white;
        }
        if (isCat)
        {
            JoinCat();
            if (GameObject.Find("Cat") == null)
            {
                Debug.Log("NOPE");
            }
            else
            {
                Debug.Log("FOUND");
            }

        }else
        {
            //LeaveCat();

        }
        if (isMice1)
        {
            JoinMouse1();
            if (GameObject.Find("Mice1") == null)
            {
                Debug.Log("NOPE");
            }
            else
            {
                Debug.Log("FOUND");
            }

        }
        else
        {
            //LeaveM1();
        }
        if (isMice2)
        {
            JoinMouse2();
            if (GameObject.Find("Mice2") == null)
            {
                Debug.Log("NOPE");
            }
            else
            {
                Debug.Log("FOUND");
            }

        }
        else
        {
            //LeaveM2();
        }
        if (isMice3)
        {
            JoinMouse3();
            if (GameObject.Find("Mice3") == null)
            {
                Debug.Log("NOPE");
            }
            else
            {
                Debug.Log("FOUND");
            }

        }
        else
        {
            //LeaveM3();
        }
    }
    public void ButtonEvents(string Event)
    {
        switch (Event)
        {
            case "StartBtn":
                //if (PhotonNetwork.JoinLobby())
                // {
                if(teamChosen == true/*&&PhotonNetwork.playerList.Length == 2*/)
                {
                    
                    PhotonNetwork.LoadLevel("catmousegame3");
                }else
                {
                    Debug.Log("Choose a Team");
                }
                // }
                break;
            /* case "TeamCat":
                     if(isCat == false)
                     {

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
                 break;*/
                case "Cat":
                if (isCat == false)
                {

                    isCat = true;
                    isMice1 = false;
                    isMice2 = false;
                    isMice3 = false;
                    teamChosen = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 0;
                }
                break;
                case "Mouse1":
                if (isMice1 == false)
                {

                    isCat = false;
                    isMice1 = true;
                    isMice2 = false;
                    isMice3 = false;
                    teamChosen = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                }
                break;
                case "Mouse2":
                if (isMice2 == false)
                {

                    isCat = false;
                    isMice1 = false;
                    isMice2 = true;
                    isMice3 = false;
                    teamChosen = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                }
                break;
                case "Mouse3":
                if (isMice3 == false)
                {

                    isCat = false;
                    isMice1 = false;
                    isMice2 = false;
                    isMice3 = true;
                    teamChosen = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                }
                break;
        }
    }

}
