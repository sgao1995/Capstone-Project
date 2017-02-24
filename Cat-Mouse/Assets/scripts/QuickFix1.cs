using UnityEngine;
using System.Collections;

public class QuickFix1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.automaticallySyncScene = true;
        if (PhotonNetwork.isMasterClient == false)
        {
            GameObject StartBtn = GameObject.Find("StartButton");
            StartBtn.SetActive(false);
            Debug.Log("you are not Master");
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
