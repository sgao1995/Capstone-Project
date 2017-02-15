using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour {
    string winner;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    public void setWinner(string win)
    {
        winner = win;
    }
    public string getWinner()
    {
        return winner;
    }
}
