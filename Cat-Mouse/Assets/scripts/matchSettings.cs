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
        /*PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.automaticallySyncScene = true;
        if (PhotonNetwork.isMasterClient == false)
        {
            GameObject StartBtn = GameObject.Find("StartButton");
            StartBtn.SetActive(false);
            Debug.Log("you are not Master");
        }
        Debug.Log(PhotonNetwork.playerList.Length);*/

    }

    void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        
    }
    void OnLeftRoom()
    {
        Debug.Log("still leaving...");
        PhotonNetwork.isMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("lobby");
    }
    void JoinCat()
    {
        GameObject.Find("CatBtn").GetComponent<PhotonView>().RPC("setPid", PhotonTargets.AllBuffered, "CText",PhotonNetwork.player.ID.ToString());
    }
    void LeaveCat()
    {
        GameObject.Find("CatBtn").GetComponent<PhotonView>().RPC("unsetPid", PhotonTargets.AllBuffered, "CText");
    }

    void JoinMouse1()
    {
        GameObject.Find("Mice1Btn").GetComponent<PhotonView>().RPC("setPid", PhotonTargets.AllBuffered, "M1Text", PhotonNetwork.player.ID.ToString());
    }
    void LeaveM1()
    {
        GameObject.Find("Mice1Btn").GetComponent<PhotonView>().RPC("unsetPid", PhotonTargets.AllBuffered, "M1Text");
    }
    void JoinMouse2()
    {
        GameObject.Find("Mice2Btn").GetComponent<PhotonView>().RPC("setPid", PhotonTargets.AllBuffered, "M2Text", PhotonNetwork.player.ID.ToString());
    }
    void LeaveM2()
    {
        GameObject.Find("Mice2Btn").GetComponent<PhotonView>().RPC("unsetPid", PhotonTargets.AllBuffered, "M2Text");
    }
    void JoinMouse3()
    {
        GameObject.Find("Mice3Btn").GetComponent<PhotonView>().RPC("setPid", PhotonTargets.AllBuffered, "M3Text", PhotonNetwork.player.ID.ToString());
    }
    void LeaveM3()
    {
        GameObject.Find("Mice3Btn").GetComponent<PhotonView>().RPC("unsetPid", PhotonTargets.AllBuffered, "M3Text");
    }
    [PunRPC]
    void setPid(string button, string pid)
    {
        GameObject.Find(button).GetComponent<Text>().text = "Player " + pid;
        Debug.Log("HEY IT WORKS" + pid);

    }
    [PunRPC]
    void unsetPid(string button)
    {
        GameObject.Find(button).GetComponent<Text>().text = "Spot Open";
        Debug.Log("HEY IT WORKS2");

    }
    // Update is called once per frame
    void Update()
    {
        //GameObject.Find("scriptLobby").GetComponent<lobby>().playerCount= PhotonNetwork.playerList.Length;
        if (teamChosen)
        {
            //GameObject.Find("StartButton").GetComponent<Image>().color = Color.red;
            //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.white;
        }
    }
    public void ButtonEvents(string Event)
    {
        switch (Event)
        {
            case "StartBtn":
                if (teamChosen == true/*&&PhotonNetwork.playerList.Length == 2*/)
                {
                    PhotonNetwork.LoadLevel("catmousegame3");
                }
                else
                {
                    Debug.Log("Choose a Team");
                }
                // }
                break;
            case "LeaveBtn":
                Debug.Log("LeavingRoom");
                LeaveRoom();
                break;
            case "Cat":

                if (isCat == false && teamChosen == false)
                {

                    isCat = true;
                    isMice1 = false;
                    isMice2 = false;
                    isMice3 = false;
                    teamChosen = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 0;
                    GameObject.Find("CatBtn").GetComponent<Image>().color = Color.blue;
                    GameObject.Find("CatBtn").transform.FindChild("CText").GetComponent<Text>().color = Color.white;
                    GameObject.Find("CatBtn").GetComponent<PhotonView>().RPC("C", PhotonTargets.OthersBuffered);
                    JoinCat();
                }
                else
                {
                    if (isCat == true && teamChosen == true)
                    {
                        Debug.Log("IT WORKS");
                        isCat = false;
                        isMice1 = false;
                        isMice2 = false;
                        isMice3 = false;
                        teamChosen = false;
                        GameObject.Find("CatBtn").GetComponent<Image>().color = Color.white;
                        GameObject.Find("CatBtn").transform.FindChild("CText").GetComponent<Text>().color = Color.black;
                        GameObject.Find("CatBtn").GetComponent<PhotonView>().RPC("NC", PhotonTargets.OthersBuffered);
                        LeaveCat();
                    }
                }
                break;
            case "Mouse1":
                if (isMice1 == false && teamChosen == false)
                {

                    isCat = false;
                    isMice1 = true;
                    isMice2 = false;
                    isMice3 = false;
                    teamChosen = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                    GameObject.Find("Mice1Btn").GetComponent<Image>().color = Color.red;
                    GameObject.Find("Mice1Btn").transform.FindChild("M1Text").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Mice1Btn").GetComponent<PhotonView>().RPC("M1", PhotonTargets.OthersBuffered);
                    JoinMouse1();
                }
                else
                {
                    if (isMice1 == true && teamChosen == true)
                    {
                        isCat = false;
                        isMice1 = false;
                        isMice2 = false;
                        isMice3 = false;
                        teamChosen = false;
                        GameObject.Find("Mice1Btn").GetComponent<Image>().color = Color.white;
                        GameObject.Find("Mice1Btn").transform.FindChild("M1Text").GetComponent<Text>().color = Color.black;
                        GameObject.Find("Mice1Btn").GetComponent<PhotonView>().RPC("NM1", PhotonTargets.OthersBuffered);
                        LeaveM1();
                    }
                }
                break;
            case "Mouse2":
                if (isMice2 == false && teamChosen == false)
                {

                    isCat = false;
                    isMice1 = false;
                    isMice2 = true;
                    isMice3 = false;
                    teamChosen = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                    GameObject.Find("Mice2Btn").GetComponent<Image>().color = Color.yellow;
                    GameObject.Find("Mice2Btn").transform.FindChild("M2Text").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Mice2Btn").GetComponent<PhotonView>().RPC("M2", PhotonTargets.OthersBuffered);
                    JoinMouse2();
                }
                else
                {
                    if (isMice2 == true && teamChosen == true)
                    {
                        isCat = false;
                        isMice1 = false;
                        isMice2 = false;
                        isMice3 = false;
                        teamChosen = false;
                        GameObject.Find("Mice2Btn").GetComponent<Image>().color = Color.white;
                        GameObject.Find("Mice2Btn").transform.FindChild("M2Text").GetComponent<Text>().color = Color.black;
                        GameObject.Find("Mice2Btn").GetComponent<PhotonView>().RPC("NM2", PhotonTargets.OthersBuffered);
                        LeaveM2();
                    }
                }
                break;
            case "Mouse3":
                if (isMice3 == false && teamChosen == false)
                {

                    isCat = false;
                    isMice1 = false;
                    isMice2 = false;
                    isMice3 = true;
                    teamChosen = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                    GameObject.Find("Mice3Btn").GetComponent<Image>().color = Color.green;
                    GameObject.Find("Mice3Btn").transform.FindChild("M3Text").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Mice3Btn").GetComponent<PhotonView>().RPC("M3", PhotonTargets.OthersBuffered);
                    JoinMouse3();
                }
                else
                {
                    if (isMice3 == true && teamChosen == true)
                    {
                        isCat = false;
                        isMice1 = false;
                        isMice2 = false;
                        isMice3 = false;
                        teamChosen = false;
                        GameObject.Find("Mice3Btn").GetComponent<Image>().color = Color.white;
                        GameObject.Find("Mice3Btn").transform.FindChild("M3Text").GetComponent<Text>().color = Color.black;
                        GameObject.Find("Mice3Btn").GetComponent<PhotonView>().RPC("NM3", PhotonTargets.OthersBuffered);
                        LeaveM3();
                    }
                }
                break;
        }
    }
    [PunRPC]
    void C()
    {
        GameObject.Find("CatBtn").GetComponent<Button>().interactable = false;
        GameObject.Find("CatBtn").GetComponent<Image>().color = Color.blue;
        GameObject.Find("CatBtn").transform.FindChild("CText").GetComponent<Text>().color = Color.white;
    }
    [PunRPC]
    void NC()
    {
        GameObject.Find("CatBtn").GetComponent<Button>().interactable = true;
        GameObject.Find("CatBtn").GetComponent<Image>().color = Color.white;
        GameObject.Find("CatBtn").transform.FindChild("CText").GetComponent<Text>().color = Color.black;
    }
    [PunRPC]
    void M1()
    {
        GameObject.Find("Mice1Btn").GetComponent<Button>().interactable = false;
        GameObject.Find("Mice1Btn").GetComponent<Image>().color = Color.red;
        GameObject.Find("Mice1Btn").transform.FindChild("M1Text").GetComponent<Text>().color = Color.white;
    }
    [PunRPC]
    void NM1()
    {
        GameObject.Find("Mice1Btn").GetComponent<Button>().interactable = true;
        GameObject.Find("Mice1Btn").GetComponent<Image>().color = Color.white;
        GameObject.Find("Mice1Btn").transform.FindChild("M1Text").GetComponent<Text>().color = Color.black;
    }
    [PunRPC]
    void M2()
    {
        GameObject.Find("Mice2Btn").GetComponent<Button>().interactable = false;
        GameObject.Find("Mice2Btn").GetComponent<Image>().color = Color.yellow;
        GameObject.Find("Mice2Btn").transform.FindChild("M2Text").GetComponent<Text>().color = Color.white;
    }
    [PunRPC]
    void NM2()
    {
        GameObject.Find("Mice2Btn").GetComponent<Button>().interactable = true;
        GameObject.Find("Mice2Btn").GetComponent<Image>().color = Color.white;
        GameObject.Find("Mice2Btn").transform.FindChild("M2Text").GetComponent<Text>().color = Color.black;
    }
    [PunRPC]
    void M3()
    {
        GameObject.Find("Mice3Btn").GetComponent<Button>().interactable = false;
        GameObject.Find("Mice3Btn").GetComponent<Image>().color = Color.green;
        GameObject.Find("Mice3Btn").transform.FindChild("M3Text").GetComponent<Text>().color = Color.white;
    }
    [PunRPC]
    void NM3()
    {
        GameObject.Find("Mice3Btn").GetComponent<Button>().interactable = true;
        GameObject.Find("Mice3Btn").GetComponent<Image>().color = Color.white;
        GameObject.Find("Mice3Btn").transform.FindChild("M3Text").GetComponent<Text>().color = Color.black;
    }
}
