using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
	private GameObject skillsMenu;
	
	void Start(){
		skillsMenu = GameObject.Find("SkillMenu");
		skillsMenu.SetActive(false);
	}
	
	
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
