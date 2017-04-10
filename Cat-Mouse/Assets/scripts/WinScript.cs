using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScript : MonoBehaviour {
    string winner;
    int catDeaths;
    int mouseDeaths;
	bool exitOpen = false;
	int numPuzzlePieces = 0;
    Text GOText;
    Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        catDeaths = 0;
        mouseDeaths = 0;
        GOText = GameObject.Find("GameOverText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (catDeaths >= 1 || exitOpen)
        {
            transform.GetComponent<PhotonView>().RPC("GameOver", PhotonTargets.AllBuffered, "The Explorers Win!");
        }
        if (mouseDeaths >= 3)
        {
            transform.GetComponent<PhotonView>().RPC("GameOver", PhotonTargets.AllBuffered, "The Hunter Wins!");
        }
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
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    public string getWinner()
    {
        return winner;
    }
	// increase number of puzzle pieces
	public void pickedUpPuzzlePiece(){
		numPuzzlePieces++;
	}
	// return number of puzzle pieces (for exit to check)
	public int numPuzzlePiecesHeld(){
		return numPuzzlePieces;
	}
	public void openExit(){
		exitOpen = true;
	}
    public void setCatDeaths()
    {
        catDeaths++;
    }
    public int getCatDeaths()
    {
        return catDeaths;
    }
    public void setMouseDeaths()
    {
        mouseDeaths++;
    }
    public int getMouseDeaths()
    {
        return mouseDeaths;
    }
    [PunRPC]
    IEnumerator GameOver(string win)//function to change scene to the game over scene if a team wins
    {
        Debug.Log("GAMEOVER");
        GOText.text = win;
        anim.SetTrigger("GameOver");
        yield return new WaitForSeconds(5);
        LeaveRoom();
    }
}


