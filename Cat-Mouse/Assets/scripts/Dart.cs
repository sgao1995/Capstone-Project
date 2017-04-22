using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {
    private float damage = 10f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void OnCollisionEnter(Collision collision)
    {
        PhotonNetwork.Destroy(gameObject);
        Debug.Log("hit");
    }
}
