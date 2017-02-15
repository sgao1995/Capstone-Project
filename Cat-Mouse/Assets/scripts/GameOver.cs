using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

public class GameOver : MonoBehaviour {
    float timer;
    public Text text;
	// Use this for initialization
	void Start () {
        text= GameObject.Find("Text").GetComponent<Text>();
        updateWinText();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 15.0)
        {
            LoadLobby();
        }
    }
    void updateWinText()
    {
        string winner = GameObject.Find("WinObj").GetComponent<WinScript>().getWinner();
        text.text = winner + " wins!";
    }
    void LoadLobby()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : only master client can load level");
        }
        Debug.Log("PhotonNetwork : Loading Level : ");
        SceneManager.LoadScene("lobby", LoadSceneMode.Single);
    }
}
