using UnityEngine;
using System.Collections;

public class SteelTrap : MonoBehaviour {
	public float timeActive;
	
	// Use this for initialization
	void Start () {
		// lasts 300 seconds
		timeActive = 300f;
	}
	
	// Update is called once per frame
	void Update () {
		timeActive -= Time.deltaTime;
		if (timeActive <= 0f){
			PhotonNetwork.Destroy(this.gameObject);
		}
	}
}
