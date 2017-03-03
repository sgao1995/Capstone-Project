using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
	GameObject compass;
	private GameObject minimap;
	
	// Use this for initialization
	void Start () {
		Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 10;	
		compass = GameObject.Find("Compass").gameObject;
		minimap = GameObject.Find("Minimap");
	}
	
	
	
	// Update is called once per frame
	void Update () {
		Camera.main.transform.position = new Vector3 (this.transform.position.x, 15, this.transform.position.z);
		Vector3 angles = new Vector3(90f, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
		Camera.main.transform.eulerAngles = angles;
		// rotate the compass with the minimap
		Vector3 compassAngle = new Vector3(0f, 0f, this.transform.eulerAngles.y);
		compass.transform.eulerAngles = compassAngle;
		
		if (Input.GetKeyDown(KeyCode.Tab)){
			// calculate how large the minimap can be while still being circular
			float largestSize = (Screen.width < Screen.height) ? Screen.width - 100f : Screen.height - 200f;
			float mapScale = largestSize/200f;
			// a larger view of the minimap
			minimap.transform.localScale = new Vector3(mapScale, mapScale, 0f);
			minimap.transform.position = new Vector3(Screen.width/2f, Screen.height/2f, 0f);
			Camera.main.orthographicSize = 25;	
		}
		if (Input.GetKeyUp(KeyCode.Tab)){
			minimap.transform.localScale = new Vector3(1f, 1f, 1f);
			minimap.transform.position = new Vector3(Screen.width-115f, 115f, 0f);
			Camera.main.orthographicSize = 10;	
		}
	}
}
