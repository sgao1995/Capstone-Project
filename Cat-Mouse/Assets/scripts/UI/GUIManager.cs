using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
	private GameObject fullMap;
	private GameObject skillsMenu;
	
	
	void Start(){
		fullMap = GameObject.Find("FullMap");
		skillsMenu = GameObject.Find("SkillMenu");
		skillsMenu.SetActive(false);
	}
	/*
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
	
		*/
	/* Mini Menu button behaviours */
	public void SkillsMenuButtonClicked(){
		skillsMenu.SetActive(true);
		Debug.Log("set active");
	}
	public void DangerSignalClicked(){
		
	}
	public void AssistSignalClicked(){
		
	}
	public void CloseSkillsMenu(){
		skillsMenu.SetActive(false);
	}
}
