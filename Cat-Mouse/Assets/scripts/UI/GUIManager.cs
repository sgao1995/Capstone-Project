using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
	// minimap 
	public RenderTexture MiniMapTexture;
	public Material MiniMapMaterial;
	private float offset;
	private GameObject fullMap;
	
	void Start(){
		fullMap = GameObject.Find("FullMap");
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Tab)){
			fullMap.transform.GetChild(0).GetComponent<Camera>().enabled = true;
			fullMap.GetComponent<Image>().enabled = true;
		}
		if (Input.GetKeyUp(KeyCode.Tab)){
			fullMap.transform.GetChild(0).GetComponent<Camera>().enabled = false;
			fullMap.GetComponent<Image>().enabled = false;
		}
	}
}
