using UnityEngine;
using System.Collections;

public class teamselectiondata : MonoBehaviour {
    public int playertype;
    public int playername;
    // Use this for initialization
    void Start () {
        playername = PhotonNetwork.player.ID;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
