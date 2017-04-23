using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {
    private float timeAlive = 2f;
	private float timer = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= timeAlive){
			PhotonNetwork.Destroy(this.gameObject);
		}
	}
}
