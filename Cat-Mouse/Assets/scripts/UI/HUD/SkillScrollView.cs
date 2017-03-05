﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
 
public class SkillScrollView : MonoBehaviour {
	public GameObject Button_Template;
	private List<string> NameList = new List<string>();
 
	// initialize the GUI skills based on the character class
	void Start () {
		GameObject content = transform.GetChild(0).gameObject;
		// if hunter
		if (GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype == 0){
			content.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = "Heightened Senses - An alert will show when nearby an Explorer. Passive ability.";
			content.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = "Lie in Wait - After 3 seconds of not moving or using any abilities, become invisible until your next motion or ability use. Passive ability.";
			content.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = "Focus - After 10 seconds of not attacking, your next attack deals 200% damage. Passive ability.";
			content.transform.GetChild(5).transform.GetChild(1).GetComponent<Text>().text = "Leap - A powerful jump that carries you much farther than a normal jump. 5 second cooldown.";
			content.transform.GetChild(6).transform.GetChild(1).GetComponent<Text>().text = "Recoup - Restore 75 health over 10 seconds. 45 second cooldown.";
			content.transform.GetChild(7).transform.GetChild(1).GetComponent<Text>().text = "Stalker - Become invisible and lower the volume of your footsteps for 5 seconds. 20 second cooldown.";
			content.transform.GetChild(9).transform.GetChild(1).GetComponent<Text>().text = "Bash - Perform a heavy attack a short range in front of you, dealing damage and stunning the target for 1 second. 20 second cooldown.";
			content.transform.GetChild(10).transform.GetChild(1).GetComponent<Text>().text = "Lasso - Throw out a rope that pulls the first enemy hit to you. 15 second cooldown.";
			content.transform.GetChild(11).transform.GetChild(1).GetComponent<Text>().text = "Trap - Lay down a steel trap that lasts 300 seconds that will snare the first enemy that steps on it for 4 seconds. Maximum of 5 traps at once. 10 second cooldown.";
			content.transform.GetChild(13).transform.GetChild(1).GetComponent<Text>().text = "The Hunter - Channel for 2 seconds, then teleport behind the closest Explorer. 100 second cooldown. ";
			content.transform.GetChild(14).transform.GetChild(1).GetComponent<Text>().text = "Reload - Instantly refreshes all of your other ability cooldowns. 60 second cooldown.";
		}
		// explorer
		else{
			content.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = "Tag Team - Gain 50% bonus movement speed when near another Explorer. Passive ability.";
			content.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = "Treasure Hunter - An alert will show when nearby chests or keys. Passive ability.";
			content.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = "Problem Solver - An alert will show when nearby puzzle rooms. Passive ability.";
			content.transform.GetChild(5).transform.GetChild(1).GetComponent<Text>().text = "Smokescreen - Throw down a cloud of smoke, causing you to become invisible for 5 seconds. 30 second cooldown.";
			content.transform.GetChild(6).transform.GetChild(1).GetComponent<Text>().text = "Bandage - Restore 40 health over 10 seconds. 45 second cooldown.";
			content.transform.GetChild(7).transform.GetChild(1).GetComponent<Text>().text = "Demolish - An attack in front of the Explorer. Can destroy walls. 30 second cooldown.";
			content.transform.GetChild(9).transform.GetChild(1).GetComponent<Text>().text = "Disengage - Instantly jump a large distance backwards. 20 second cooldown.";
			content.transform.GetChild(10).transform.GetChild(1).GetComponent<Text>().text = "Cripple - Perform an attack that cripples the first target hit, slowing their movement speed by 30% for 3 seconds. 20 second cooldown.";
			content.transform.GetChild(11).transform.GetChild(1).GetComponent<Text>().text = "Brawler - Your attacks deal 50% extra damage, and you become immune to enemy abilities. Lasts 5 seconds. 45 second cooldown.";
			content.transform.GetChild(13).transform.GetChild(1).GetComponent<Text>().text = "The Hunted - Instantly become invisible, gain 50% bonus movement speed, and mute your footsteps completely for 6 seconds. 120 second cooldown.";
			content.transform.GetChild(14).transform.GetChild(1).GetComponent<Text>().text = "Sleep Dart - Shoot a dart towards target location, putting the first enemy hit to sleep for 5 seconds, rendering them unable to take any action until the sleep wears off or they take damage. 100 second cooldown. ";
		}
		
	
  /*
  NameList.Add("Alan");
  NameList.Add("Amy");
  NameList.Add("Brian");
  NameList.Add("Carrie");
  NameList.Add("David");
  NameList.Add("Joe");
  NameList.Add("Jason");
  NameList.Add("Michelle");
  NameList.Add("Stephanie");
  NameList.Add("Zoe");
 
  foreach(string str in NameList)
  {
   GameObject go = Instantiate(Button_Template) as GameObject;
   go.SetActive(true);
   SkillButton TB = go.transform.GetChild(0).GetComponent<SkillButton>();	
   TB.SetName(str);
   go.transform.SetParent(Button_Template.transform.parent);
 
  }*/
 
 
	}
  
	public void ButtonClicked(string str)
	{
		Debug.Log(str + " button clicked.");
	}
} 