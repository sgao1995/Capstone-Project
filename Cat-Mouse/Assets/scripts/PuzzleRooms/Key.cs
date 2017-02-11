using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void Interact(){
		// need to figure out which mouse took the key
		// then destroy the key
		Debug.Log("Picked up Key");
	}
}
