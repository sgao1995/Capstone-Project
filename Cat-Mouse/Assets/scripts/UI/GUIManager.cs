using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
	public GameObject skillsMenu;
	
	void Start(){
		skillsMenu.SetActive(false);
	}
	
	
	/* Mini Menu button behaviours */
	public void SkillsMenuButtonClicked(){		Debug.Log("set active");
		skillsMenu.SetActive(true);

	}
	public void DangerSignalClicked(){
		
	}
	public void AssistSignalClicked(){
		
	}
	public void CloseSkillsMenu(){
		skillsMenu.SetActive(false);
	}
}
