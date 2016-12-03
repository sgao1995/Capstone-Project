using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
	GameObject cat;

	// Use this for initialization
	void Start () {
		Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 8;	
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.transform.position = new Vector3 (this.transform.position.x, 8, this.transform.position.z);
	}
}
