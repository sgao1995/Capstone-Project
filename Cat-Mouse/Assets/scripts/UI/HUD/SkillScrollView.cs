using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class SkillScrollView : MonoBehaviour {
 
 public GameObject Button_Template;
 private List<string> NameList = new List<string>();
 
 
 // Use this for initialization
 void Start () {
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