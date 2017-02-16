using UnityEngine;
using System.Collections;

public class teamselectiondata : MonoBehaviour {
    public int playertype;
    public int playercount = 0;
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
}
