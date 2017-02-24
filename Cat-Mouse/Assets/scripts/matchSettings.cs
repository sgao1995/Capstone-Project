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
    static bool choseTeam = false;
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
        if (teamChosen)
        {
            //GameObject.Find("StartButton").GetComponent<Image>().color = Color.red;
            //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.white;
        }
        /*if (isCat)
        {
            JoinCat();
            if (GameObject.Find("CatBtn") == null)
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
            if (GameObject.Find("Mice1Btn") == null)
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
            if (GameObject.Find("Mice2Btn") == null)
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
            if (GameObject.Find("Mice3Btn") == null)
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
        }*/
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
            case "Cat":

                if (isCat == false && choseTeam == false)
                {

                    isCat = true;
                    isMice1 = false;
                    isMice2 = false;
                    isMice3 = false;
                    teamChosen = true;
                    choseTeam = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 0;
                    //GameObject.Find("Cat").GetComponent<Button>().interactable = false;
                    GameObject.Find("CatBtn").GetComponent<Image>().color = Color.blue;
                    GameObject.Find("CatBtn").transform.FindChild("CText").GetComponent<Text>().color = Color.white;
                    GameObject.Find("CatBtn").GetComponent<PhotonView>().RPC("C", PhotonTargets.OthersBuffered);
                    JoinCat();
                }
                else
                {
                    if (isCat == true && choseTeam == true)
                    {
                        Debug.Log("IT WORKS");
                        isCat = false;
                        isMice1 = false;
                        isMice2 = false;
                        isMice3 = false;
                        teamChosen = false;
                        choseTeam = false;
                        //GameObject.Find("StartButton").GetComponent<Image>().color = Color.white;
                        //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.black;
                        GameObject.Find("CatBtn").GetComponent<Image>().color = Color.white;
                        GameObject.Find("CatBtn").transform.FindChild("CText").GetComponent<Text>().color = Color.black;
                        GameObject.Find("CatBtn").GetComponent<PhotonView>().RPC("NC", PhotonTargets.OthersBuffered);
                        LeaveCat();
                    }
                }
                break;
            case "Mouse1":
                if (isMice1 == false && choseTeam == false)
                {

                    isCat = false;
                    isMice1 = true;
                    isMice2 = false;
                    isMice3 = false;
                    teamChosen = true;
                    choseTeam = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                    //GameObject.Find("Mice1").GetComponent<Button>().interactable = false;
                    GameObject.Find("Mice1Btn").GetComponent<Image>().color = Color.red;
                    GameObject.Find("Mice1Btn").transform.FindChild("M1Text").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Mice1Btn").GetComponent<PhotonView>().RPC("M1", PhotonTargets.OthersBuffered);
                    JoinMouse1();
                }
                else
                {
                    if (isMice1 == true && choseTeam == true)
                    {
                        isCat = false;
                        isMice1 = false;
                        isMice2 = false;
                        isMice3 = false;
                        teamChosen = false;
                        choseTeam = false;
                        //GameObject.Find("StartButton").GetComponent<Image>().color = Color.white;
                        //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.black;
                        GameObject.Find("Mice1Btn").GetComponent<Image>().color = Color.white;
                        GameObject.Find("Mice1Btn").transform.FindChild("M1Text").GetComponent<Text>().color = Color.black;
                        GameObject.Find("Mice1Btn").GetComponent<PhotonView>().RPC("NM1", PhotonTargets.OthersBuffered);
                        LeaveM1();
                    }
                }
                break;
            case "Mouse2":
                if (isMice2 == false && choseTeam == false)
                {

                    isCat = false;
                    isMice1 = false;
                    isMice2 = true;
                    isMice3 = false;
                    teamChosen = true;
                    choseTeam = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                    //GameObject.Find("Mice2").GetComponent<Button>().interactable = false;
                    GameObject.Find("Mice2Btn").GetComponent<Image>().color = Color.yellow;
                    GameObject.Find("Mice2Btn").transform.FindChild("M2Text").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Mice2Btn").GetComponent<PhotonView>().RPC("M2", PhotonTargets.OthersBuffered);
                    JoinMouse2();
                }
                else
                {
                    if (isMice2 == true && choseTeam == true)
                    {
                        isCat = false;
                        isMice1 = false;
                        isMice2 = false;
                        isMice3 = false;
                        teamChosen = false;
                        choseTeam = false;
                        //GameObject.Find("StartButton").GetComponent<Image>().color = Color.white;
                        //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.black;
                        GameObject.Find("Mice2Btn").GetComponent<Image>().color = Color.white;
                        GameObject.Find("Mice2Btn").transform.FindChild("M2Text").GetComponent<Text>().color = Color.black;
                        GameObject.Find("Mice2Btn").GetComponent<PhotonView>().RPC("NM2", PhotonTargets.OthersBuffered);
                        LeaveM2();
                    }
                }
                break;
            case "Mouse3":
                if (isMice3 == false && choseTeam == false)
                {

                    isCat = false;
                    isMice1 = false;
                    isMice2 = false;
                    isMice3 = true;
                    teamChosen = true;
                    choseTeam = true;
                    GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype = 1;
                    //GameObject.Find("Mice3").GetComponent<Button>().interactable = false;
                    GameObject.Find("Mice3Btn").GetComponent<Image>().color = Color.green;
                    GameObject.Find("Mice3Btn").transform.FindChild("M3Text").GetComponent<Text>().color = Color.white;
                    GameObject.Find("Mice3Btn").GetComponent<PhotonView>().RPC("M3", PhotonTargets.OthersBuffered);
                    JoinMouse3();
                }
                else
                {
                    if (isMice3 == true && choseTeam == true)
                    {
                        isCat = false;
                        isMice1 = false;
                        isMice2 = false;
                        isMice3 = false;
                        teamChosen = false;
                        choseTeam = false;
                        //GameObject.Find("StartButton").GetComponent<Image>().color = Color.white;
                        //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.black;
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
        JoinCat();
    }
    [PunRPC]
    void NC()
    {
        GameObject.Find("CatBtn").GetComponent<Button>().interactable = true;
        GameObject.Find("CatBtn").GetComponent<Image>().color = Color.white;
        GameObject.Find("CatBtn").transform.FindChild("CText").GetComponent<Text>().color = Color.black;
        LeaveCat();
        //GameObject.Find("StartButton").GetComponent<Image>().color = Color.white;
        //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.black;
    }
    [PunRPC]
    void M1()
    {
        GameObject.Find("Mice1Btn").GetComponent<Button>().interactable = false;
        GameObject.Find("Mice1Btn").GetComponent<Image>().color = Color.red;
        GameObject.Find("Mice1Btn").transform.FindChild("M1Text").GetComponent<Text>().color = Color.white;
        JoinMouse1();
    }
    [PunRPC]
    void NM1()
    {
        GameObject.Find("Mice1Btn").GetComponent<Button>().interactable = true;
        GameObject.Find("Mice1Btn").GetComponent<Image>().color = Color.white;
        GameObject.Find("Mice1Btn").transform.FindChild("M1Text").GetComponent<Text>().color = Color.black;
        LeaveM1();
        //GameObject.Find("StartButton").GetComponent<Image>().color = Color.white;
        //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.black;
    }
    [PunRPC]
    void M2()
    {
        GameObject.Find("Mice2Btn").GetComponent<Button>().interactable = false;
        GameObject.Find("Mice2Btn").GetComponent<Image>().color = Color.yellow;
        GameObject.Find("Mice2Btn").transform.FindChild("M2Text").GetComponent<Text>().color = Color.white;
        JoinMouse2();
    }
    [PunRPC]
    void NM2()
    {
        GameObject.Find("Mice2Btn").GetComponent<Button>().interactable = true;
        GameObject.Find("Mice2Btn").GetComponent<Image>().color = Color.white;
        GameObject.Find("Mice2Btn").transform.FindChild("M2Text").GetComponent<Text>().color = Color.black;
        LeaveM2();
        //GameObject.Find("StartButton").GetComponent<Image>().color = Color.white;
        //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.black;
    }
    [PunRPC]
    void M3()
    {
        GameObject.Find("Mice3Btn").GetComponent<Button>().interactable = false;
        GameObject.Find("Mice3Btn").GetComponent<Image>().color = Color.green;
        GameObject.Find("Mice3Btn").transform.FindChild("M3Text").GetComponent<Text>().color = Color.white;
        JoinMouse3();
    }
    [PunRPC]
    void NM3()
    {
        GameObject.Find("Mice3Btn").GetComponent<Button>().interactable = true;
        GameObject.Find("Mice3Btn").GetComponent<Image>().color = Color.white;
        GameObject.Find("Mice3Btn").transform.FindChild("M3Text").GetComponent<Text>().color = Color.black;
        LeaveM3();
        //GameObject.Find("StartButton").GetComponent<Image>().color = Color.white;
        //GameObject.Find("StartButton").transform.FindChild("Text").GetComponent<Text>().color = Color.black;
    }
}
