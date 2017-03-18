using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Stores and processes Input and Ouput of Skill Data for all Character Classes */
public class SkillData : MonoBehaviour {
    private List<int> skillListID;  // Represents the Skill ID of all Skills
    private List<string> skillListName;  // Represents the Skill Name of all Skills
    private List<string> skillListDescription;  // Represents the Skill Description of all Skills
    private List<int> skillListType;  // Represents the Skill Type of all Skills  (0: Passive, 1: Active)
    private List<int> skillListTier;  // Represents the Skill Tier of all Skills  (0: Regular, 1: Ultimate)
    private List<Sprite> skillListIcon;  // Represents the Sprites for the Skill Icons of all Skills
    private List<float> skillListCooldown;  // Represents the Cooldown Times of all Skills

    private int numStoredSkills = 22;  // Sets and stores the number of Skills in Skill Data
    private bool skillDataLoaded;  // Stores whether all Skill Data has been loaded

	// Use this for initialization
	void Start () {

        /* Initially, no Skill Data is loaded */
        this.setSkillDataLoaded(false);

        /* Loads the Skill List ID Data of all Skills */
        this.skillListID = new List<int>();
        int[] skillListIDData = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 };
        this.skillListID.AddRange(skillListIDData);

        /* Loads the Skill List Name Data of all Skills */
        this.skillListName = new List<string>();

        /* Data for Explorer Class */
        this.skillListName.Add("Tag Team");
        this.skillListName.Add("Treasure Hunter");
        this.skillListName.Add("Problem Solver");
        this.skillListName.Add("Smokescreen");
        this.skillListName.Add("Bandage");
        this.skillListName.Add("Demolish");
        this.skillListName.Add("Disengage");
        this.skillListName.Add("Cripple");
        this.skillListName.Add("Brawler");
        this.skillListName.Add("The Hunted");
        this.skillListName.Add("Sleep Dart");

        /* Data for Hunter Class */
        this.skillListName.Add("Heightened Senses");
        this.skillListName.Add("Lie In Wait");
        this.skillListName.Add("Focus");
        this.skillListName.Add("Leap");
        this.skillListName.Add("Recoup");
        this.skillListName.Add("Stalker");
        this.skillListName.Add("Bash");
        this.skillListName.Add("Lasso");
        this.skillListName.Add("Trap");
        this.skillListName.Add("The Hunter");
        this.skillListName.Add("Reload");


        /* Loads the Skill List Description Data of all Skills */
        this.skillListDescription = new List<string>();

        /* Data for Explorer Class */
        this.skillListDescription.Add("Gain 50% bonus movement speed when near another Explorer. Passive ability.");
        this.skillListDescription.Add("An alert will show when nearby chests or keys. Passive ability.");
        this.skillListDescription.Add("An alert will show when nearby puzzle rooms. Passive ability.");
        this.skillListDescription.Add("Throw down a cloud of smoke, causing you to become invisible for 5 seconds. 30 second cooldown.");
        this.skillListDescription.Add("Restore 40 health over 10 seconds. 45 second cooldown.");
        this.skillListDescription.Add("An attack in front of the Explorer. Can destroy walls. 30 second cooldown.");
        this.skillListDescription.Add("Instantly jump a large distance backwards. 20 second cooldown.");
        this.skillListDescription.Add("Perform an attack that cripples the first target hit, slowing their movement speed by 30% for 3 seconds. 20 second cooldown.");
        this.skillListDescription.Add("Your attacks deal 50% extra damage, and you become immune to enemy abilities. Lasts 5 seconds. 45 second cooldown.");
        this.skillListDescription.Add("Instantly become invisible, gain 50% bonus movement speed, and mute your footsteps completely for 6 seconds. 120 second cooldown.");
        this.skillListDescription.Add("Shoot a dart towards target location, putting the first enemy hit to sleep for 5 seconds, rendering them unable to take any action until the sleep wears off or they take damage. 100 second cooldown.");

        /* Data for Hunter Class */
        this.skillListDescription.Add("An alert will show when nearby an Explorer. Passive ability.");
        this.skillListDescription.Add("After 3 seconds of not moving or using any abilities, become invisible until your next motion or ability use. Passive ability.");
        this.skillListDescription.Add("After 10 seconds of not attacking, your next attack deals 200% damage. Passive ability.");
        this.skillListDescription.Add("A powerful jump that carries you much farther than a normal jump. 5 second cooldown.");
        this.skillListDescription.Add("Restore 75 health over 10 seconds. 45 second cooldown.");
        this.skillListDescription.Add("Become invisible and lower the volume of your footsteps for 5 seconds. 20 second cooldown.");
        this.skillListDescription.Add("Perform a heavy attack a short range in front of you, dealing damage and stunning the target for 1 second. 20 second cooldown.");
        this.skillListDescription.Add("Throw out a rope that pulls the first enemy hit to you. 15 second cooldown.");
        this.skillListDescription.Add("Lay down a steel trap that lasts 300 seconds that will snare the first enemy that steps on it for 4 seconds. Maximum of 5 traps at once. 10 second cooldown.");
        this.skillListDescription.Add("Channel for 2 seconds, then teleport behind the closest Explorer. 100 second cooldown.");
        this.skillListDescription.Add("Reload - Instantly refreshes all of your other ability cooldowns. 60 second cooldown.");

        /* Loads the Skill List Type Data of all Skills */
        this.skillListType = new List<int>();
        int[] skillListTypeData = { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
        this.skillListType.AddRange(skillListTypeData);

        /* Loads the Skill List Tier Data of all Skills */
        this.skillListTier = new List<int>();
        int[] skillListTierData = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 };
        this.skillListTier.AddRange(skillListTierData);

        /* Loads the Skill List Cooldown Time Data for all Skills */
        this.skillListCooldown = new List<float>();

        /* Data for Explorer Class */
        this.skillListCooldown.Add(0f);
        this.skillListCooldown.Add(0f);
        this.skillListCooldown.Add(0f);
        this.skillListCooldown.Add(30f);
        this.skillListCooldown.Add(45f);
        this.skillListCooldown.Add(30f);
        this.skillListCooldown.Add(20f);
        this.skillListCooldown.Add(20f);
        this.skillListCooldown.Add(45f);
        this.skillListCooldown.Add(120f);
        this.skillListCooldown.Add(100f);

        /* Data for Hunter Class */
        this.skillListCooldown.Add(0f);
        this.skillListCooldown.Add(0f);
        this.skillListCooldown.Add(0f);
        this.skillListCooldown.Add(5f);
        this.skillListCooldown.Add(45f);
        this.skillListCooldown.Add(20f);
        this.skillListCooldown.Add(20f);
        this.skillListCooldown.Add(1f);
        this.skillListCooldown.Add(1f);
        this.skillListCooldown.Add(100f);
        this.skillListCooldown.Add(60f);

        /* Indicates that all Skill Data has been loaded */
        this.setSkillDataLoaded(true);
    }

    // Update is called once per frame
    void Update () {
	
	}

    /* Sets the number of Skills stored in the Skill Data */
    public void setNumStoredSkills(int numSkills)
    {
        this.numStoredSkills = numSkills;
    }

    /* Gets the number of Skills stored in the Skill Data */
    public int getNumStoredSkills()
    {
        return this.numStoredSkills;
    }

    /* Sets whether all Skill Data has been loaded */
    public void setSkillDataLoaded(bool loaded)
    {
        this.skillDataLoaded = loaded;
    }

    /* Gets whether all Skill Data has been loaded */
    public bool getSkillDataLoaded()
    {
        return this.skillDataLoaded;
    }

    /* Gets a list containing the Skill ID of all Skills */
    public List<int> getSkillListID()
    {
        return this.skillListID;
    }

    /* Gets a list containing the Skill Name of all Skills */
    public List<string> getSkillListName()
    {
        return this.skillListName;
    }

    /* Gets a list containing the Skill Description of all Skills */
    public List<string> getSkillListDescription()
    {
        return this.skillListDescription;
    }

    /* Gets a list containing the Skill Type of all Skills */
    public List<int> getSkillListType()
    {
        return this.skillListType;
    }

    /* Gets a list containing the Skill Tier of all Skills */
    public List<int> getSkillListTier()
    {
        return this.skillListTier;
    }

    /* Gets an array containing the Skill Cooldown Time of all Skills */
    public List<float> getSkillListCooldown()
    {
        return this.skillListCooldown;
    }
}
