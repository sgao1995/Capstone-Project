using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
	GameObject cat;

	// Use this for initialization
	void Start () {
		Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 10;	
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.transform.position = new Vector3 (this.transform.position.x, 15, this.transform.position.z);
		Vector3 angles = new Vector3(90f, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
		Camera.main.transform.eulerAngles = angles;
		
		//Camera.main.transform.rotation = new Quaternion(0f,this.transform.rotation.y,0, 0);
	}
}
