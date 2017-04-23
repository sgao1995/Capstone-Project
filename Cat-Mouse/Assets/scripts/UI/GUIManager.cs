using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
	public GameObject skillsMenu;
	
	void Start(){
		//skillsMenu.SetActive(false);
	}
	
	/* Mini Menu button behaviours */
	public void SkillsMenuButtonClicked(){	
		skillsMenu.SetActive(true);
	}
	public void CloseSkillsMenu(){
		skillsMenu.SetActive(false);
	}
}
