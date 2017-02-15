using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour {
    string winner;
    int catDeaths;
    int mouseDeaths;
    // Use this for initialization
    void Start () {
        catDeaths = 0;
        mouseDeaths = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (catDeaths >= 1)
        {
            GameOver("The Mice");
        }
        if (mouseDeaths >= 3)
        {
            GameOver("The Cat");
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


