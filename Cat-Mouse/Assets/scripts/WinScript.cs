using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour {
    string winner;
    int catDeaths;
    int mouseDeaths;
	bool exitOpen = false;
	int numPuzzlePieces = 0;
    // Use this for initialization
    void Start () {
        catDeaths = 0;
        mouseDeaths = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (catDeaths >= 1 || exitOpen)
        {
            GameOver("The Mice Win!");
        }
        if (mouseDeaths >= 3)
        {
            GameOver("The Cat Wins!");
        }
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
    void GameOver(string win)//function to change scene to the game over scene if a team wins
    {
       // if (PhotonNetwork.isMasterClient)
        //{
            winner = win;
            PhotonNetwork.LoadLevel("GameOver");
       // }
      
    }
}


