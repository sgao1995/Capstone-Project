using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

public class GameOver : MonoBehaviour {
    float timer;
    public Text text;
	// Use this for initialization
	void Start () {
        //PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.automaticallySyncScene = true;
        transform.GetComponent<PhotonView>().RPC("updateWinText", PhotonTargets.AllBuffered);
        //updateWinText();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 15.0)
        {
            transform.GetComponent<PhotonView>().RPC("LoadLobby", PhotonTargets.AllBuffered);
        }
    }
    [PunRPC]
    void updateWinText()
    {
        text = GameObject.Find("Text").GetComponent<Text>();
        string winner = GameObject.Find("WinObj").GetComponent<WinScript>().getWinner();
        text.text = winner + " wins!";
    }
    [PunRPC]
    void LoadLobby()
    {
        /*   if (!PhotonNetwork.isMasterClient)
           {
               Debug.LogError("PhotonNetwork : only master client can load level");
           }*/
     
        Debug.Log("PhotonNetwork : Loading Level : ");
        SceneManager.LoadScene("lobby", LoadSceneMode.Single);
    }
}
