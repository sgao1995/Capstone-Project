using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
 
public class SkillScrollView : MonoBehaviour {
	public GameObject Button_Template;
	private List<string> NameList = new List<string>();
    GameObject SkillSlot1;
    GameObject SkillSlot2;
    GameObject SkillSlot3;
    GameObject SkillSlot4;
    public int skillsLeveled=0;

    /* Character Classes */
    public GameObject catCharObject;  // Game Object representing the 'Hunter' Class
    public GameObject mouseCharObject;  // Game Object representing the 'Explorer' Class
    public CatMovement catChar;  // Represents the 'Hunter' Class behaviour
    public MouseMovement mouseChar;  // Represents the 'Explorer' Class behaviour

	// initialize the GUI skills based on the character class
	void Start () {
        SkillSlot1 = GameObject.Find("skillSlotOne");
        SkillSlot2 = GameObject.Find("skillSlotTwo");
        SkillSlot3 = GameObject.Find("skillSlotThree");
        SkillSlot4 = GameObject.Find("skillSlotFour");
        GameObject content = transform.GetChild(0).gameObject;

        /* Initialises and retrieves the Character Classes and associated behaviours */
        if (GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype == 0)
        {
            catCharObject = GameObject.Find("Cat(Clone)");  // Retrieves the Hunter Class Object
            catChar = catCharObject.GetComponent<CatMovement>();  // Retrieves the Hunter Class behaviours
        }
        else
        {
            mouseCharObject = GameObject.Find("Mouse(Clone)");
            mouseChar = mouseCharObject.GetComponent<MouseMovement>();
        }

		// if hunter
		if (GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype == 0){
            content.transform.GetChild(1).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/HeightenedSensesIcon");
            content.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = "Heightened Senses - An alert will show when nearby an Explorer. Passive ability.";

            content.transform.GetChild(2).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/LieInWaitIcon");
            content.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = "Lie in Wait - After 3 seconds of not moving or using any abilities, become invisible until your next motion or ability use. Passive ability.";

            content.transform.GetChild(3).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/FocusIcon");
            content.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = "Focus - After 10 seconds of not attacking, your next attack deals 200% damage. Passive ability.";

            content.transform.GetChild(5).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/LeapIcon");
            content.transform.GetChild(5).transform.GetChild(1).GetComponent<Text>().text = "Leap - A powerful jump that carries you much farther than a normal jump. 5 second cooldown.";

            content.transform.GetChild(6).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/RecoupIcon");
            content.transform.GetChild(6).transform.GetChild(1).GetComponent<Text>().text = "Recoup - Restore 75 health over 10 seconds. 45 second cooldown.";

            content.transform.GetChild(7).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/StalkerIcon");
            content.transform.GetChild(7).transform.GetChild(1).GetComponent<Text>().text = "Stalker - Become invisible and lower the volume of your footsteps for 5 seconds. 20 second cooldown.";

            content.transform.GetChild(9).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/BashIcon");
            content.transform.GetChild(9).transform.GetChild(1).GetComponent<Text>().text = "Bash - Perform a heavy attack a short range in front of you, dealing damage and stunning the target for 1 second. 20 second cooldown.";

            content.transform.GetChild(10).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/LassoIcon");
            content.transform.GetChild(10).transform.GetChild(1).GetComponent<Text>().text = "Lasso - Throw out a rope that pulls the first enemy hit to you. 15 second cooldown.";

            content.transform.GetChild(11).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/TrapIcon");
            content.transform.GetChild(11).transform.GetChild(1).GetComponent<Text>().text = "Trap - Lay down a steel trap that lasts 300 seconds that will snare the first enemy that steps on it for 4 seconds. Maximum of 5 traps at once. 10 second cooldown.";

            content.transform.GetChild(13).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/TheHunterIcon");
            content.transform.GetChild(13).transform.GetChild(1).GetComponent<Text>().text = "The Hunter - Channel for 2 seconds, then teleport behind the closest Explorer. 100 second cooldown. ";

            content.transform.GetChild(14).transform.GetChild(0).GetComponent<SkillButton>().SetName("Hunter/RelaodIcon");
            content.transform.GetChild(14).transform.GetChild(1).GetComponent<Text>().text = "Reload - Instantly refreshes all of your other ability cooldowns. 60 second cooldown.";

        }
        // explorer
        else
        {
            content.transform.GetChild(1).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/TagTeamIcon");
            content.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = "Tag Team - Gain 50% bonus movement speed when near another Explorer. Passive ability.";

            content.transform.GetChild(2).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/TreasureHunterIcon");
            content.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = "Treasure Hunter - An alert will show when nearby chests or keys. Passive ability.";

            content.transform.GetChild(3).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/ProblemSolverIcon");
            content.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>().text = "Problem Solver - An alert will show when nearby puzzle rooms. Passive ability.";

            content.transform.GetChild(5).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/SmokescreenIcon");
            content.transform.GetChild(5).transform.GetChild(1).GetComponent<Text>().text = "Smokescreen - Throw down a cloud of smoke, causing you to become invisible for 5 seconds. 30 second cooldown.";

            content.transform.GetChild(6).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/BandageIcon");
            content.transform.GetChild(6).transform.GetChild(1).GetComponent<Text>().text = "Bandage - Restore 40 health over 10 seconds. 45 second cooldown.";

            content.transform.GetChild(7).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/Demolish");
            content.transform.GetChild(7).transform.GetChild(1).GetComponent<Text>().text = "Demolish - An attack in front of the Explorer. Can destroy walls. 30 second cooldown.";

            content.transform.GetChild(9).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/DisengageIcon");
            content.transform.GetChild(9).transform.GetChild(1).GetComponent<Text>().text = "Disengage - Instantly jump a large distance backwards. 20 second cooldown.";

            content.transform.GetChild(10).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/CrippleIcon");
            content.transform.GetChild(10).transform.GetChild(1).GetComponent<Text>().text = "Cripple - Perform an attack that cripples the first target hit, slowing their movement speed by 30% for 3 seconds. 20 second cooldown.";

            content.transform.GetChild(11).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/BrawlerIcon");
            content.transform.GetChild(11).transform.GetChild(1).GetComponent<Text>().text = "Brawler - Your attacks deal 50% extra damage, and you become immune to enemy abilities. Lasts 5 seconds. 45 second cooldown.";

            content.transform.GetChild(13).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/TheHuntedIcon");
            content.transform.GetChild(13).transform.GetChild(1).GetComponent<Text>().text = "The Hunted - Instantly become invisible, gain 50% bonus movement speed, and mute your footsteps completely for 6 seconds. 120 second cooldown.";

            content.transform.GetChild(14).transform.GetChild(0).GetComponent<SkillButton>().SetName("Explorer/SleepDartIcon");
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
    public Sprite skillIcon;
	public void ButtonClicked(string str)
	{
        skillIcon = Resources.Load<Sprite>("Images/"+str);
        Debug.Log(str + " button clicked.");
        
        if (skillsLeveled == 0)
        {
            SkillSlot1.GetComponent<Image>().sprite = skillIcon;
            skillsLeveled+=1;
        }
        else if (skillsLeveled == 1)
        {
            SkillSlot2.GetComponent<Image>().sprite = skillIcon;
            skillsLeveled += 1;

        }
        else if (skillsLeveled == 2)
        {
            SkillSlot3.GetComponent<Image>().sprite =skillIcon;
            skillsLeveled += 1;

        }
        else if (skillsLeveled == 3)
        {
            SkillSlot4.GetComponent<Image>().sprite = skillIcon;
            skillsLeveled += 1;

        }

    }

    /* Adds selected Skill to the Player's list of Learned Skills */
    public void addSkill(int skillID)
    {
        /* Checks the Character Class of the Player */

        /* If Hunter Class */
        if (GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype == 0)
        {

        }

        /* Else, Explorer Class */
        else
        {
            /* Select Skill to add from Skill ID */
            switch (skillID)
            {
                case 0:  // If Skill ID = 0
                    mouseChar.addLearnedSkill(0);  // Adds Skill with ID = 0
                    break;
                case 1:
                    mouseChar.addLearnedSkill(1);
                    break;
                case 2:
                    mouseChar.addLearnedSkill(2);
                    break;
                case 3:
                    mouseChar.addLearnedSkill(3);
                    break;
                case 4:
                    mouseChar.addLearnedSkill(4);
                    break;
                case 5:
                    mouseChar.addLearnedSkill(5);
                    break;
                case 6:
                    mouseChar.addLearnedSkill(6);
                    break;
                case 7:
                    mouseChar.addLearnedSkill(7);
                    break;
                case 8:
                    mouseChar.addLearnedSkill(8);
                    break;
                case 9:
                    mouseChar.addLearnedSkill(9);
                    break;
                case 10:
                    mouseChar.addLearnedSkill(10);
                    break;
                default:  // Invalid Skill ID, do nothing
                    break;
            }
        }
    }
} 