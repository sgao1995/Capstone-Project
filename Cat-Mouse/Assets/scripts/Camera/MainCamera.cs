using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPreCull () {
		 RenderSettings.ambientLight = Color.white;
	}

	void OnPreRender() {
		 RenderSettings.ambientLight = Color.white;
	}
	void OnPostRender() {
		 RenderSettings.ambientLight = new Color32 (0x49, 0x49, 0x49, 0xFF);
	}
}
