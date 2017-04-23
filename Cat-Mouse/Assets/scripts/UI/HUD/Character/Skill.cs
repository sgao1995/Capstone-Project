using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/* Class: Skill.cs
 * Description: Controls the behaviour of the Skill System of the HUD.
 */ 
public class Skill : MonoBehaviour {

    /* Represents the objects within the Skill System */

    /* Represents each invidividual Skill Slot */
    public SkillSlot[] skillSlots;

    /* Represents the Skill Slot Objects in the HUD */
    public Image[] skillSlotObjects;

    /* Represents the Skill Info Box in the HUD */
    public GameObject skillInfoBoxObject;

    /* Represents the Skill Info Objects in the HUD */
    public Text[] skillInfoObjects;

    /* Represents all Skills available to the Character */
    public CharSkill[] charSkills;
    public int charSkillsNum;  // Represents number of Skills

    /* Represents the Skill Data for all Skills */
    public SkillData skillDataSource; // Represents the Skill Data source
    public Sprite[] skillListIcon;  // Represents the Sprites of all Skills
    public Sprite skillLocked;  // Represents the sprite for a Locked Skill Slot

    /* Sets Character Skill Slot data */
    private int numSkillSlots;  // Represents number of Skill Slots enabled
    private int maxSkillSlots;  // Represents maximum number of Skill Slots

    private bool skillDataSourceLoaded = false;  // Represents whether all Skill Data has been successfully loaded from the Data Source

    /* Represents the assignment of Skills to Skill Slots */
    private List<int> charSlotAssign;  // Each INDEX represents a Skill Slot and the respective VALUE represents the Skill index (from the list of Skills)

    /* Represents a Skill Slot of a Character */
    public class SkillSlot
    {
        /* Attributes of a Skill Slot */
        private Image slotObject;  // Represents the corresponding scene object of the Skill Slot
        private Sprite slotLockedIcon;  // Represents the sprite used to indicate a locked Skill Slot
        private bool slotEnabled;  // Represents whether the Skill Slot is enabled
        private CharSkill currentSkill;  // Represents the current Skill within the Skill Slot

        /* Constructs a new Skill Slot */
        public SkillSlot(Image slotObject, Sprite slotLockedIcon, bool slotEnabled)
        {
            /* Sets each attribute to the specified value */
            this.slotObject = slotObject;
            this.slotLockedIcon = slotLockedIcon;
            this.slotEnabled = slotEnabled;
        }

        /* Sets the corresponding scene object of the Skill Slot */
        public void setSlotObject(Image slotObject)
        {
            this.slotObject = slotObject;
        }

        /* Gets the corresponding scene object of the Skill Slot */
        public Image getSlotObject()
        {
            return this.slotObject;
        }

        /* Sets the sprite to indicate a locked Skill Slot */
        public void setSlotLockedIcon(Sprite icon)
        {
            this.slotLockedIcon = icon;
        }

        /* Gets the sprite to indicate a locked Skill Slot */
        public Sprite getSlotLockedIcon()
        {
            return this.slotLockedIcon;
        }

        /* Sets whether the Skill Slot is enabled */
        public void setSlotEnabled(bool enabled)
        {
            /* updates the sprite displayed for the Skill Slot */
            if (enabled == true)
            {
                this.getSlotObject().sprite = this.currentSkill.getSkillIcon();
            }

            else
            {
                this.getSlotObject().sprite = this.getSlotLockedIcon();
            }
            this.slotEnabled = enabled;
        }

        /* Gets whether the Skill Slot is enabled */
        public bool getSlotEnabled()
        {
            return this.slotEnabled;
        }

        /* Sets the Skill currently assigned to the Skill Slot */
        public void setSlotSkill(CharSkill currentSkill)
        {
            this.currentSkill = currentSkill;
        }

        /* Gets the Skill currently assigned to the Skill Slot */
        public CharSkill getSlotSkill()
        {
            return this.currentSkill;
        }
    }

    /* Represents a Skill of a Character */
    public class CharSkill
    {
        /* Attributes of a Skill */
        private int skillID;  // Represents the Skill ID of the Skill
        private string skillName;  // Represents the Name of the Skill
        private string skillDescription;  // Represents the Description of the Skill
        private int skillType;  // Represents the Type of the Skill  (0: Passive, 1: Active)
        private int skillTier;  // Represents the Tier of the Skill  (0: Regular, 1: Ultimate) 
        private Sprite skillIcon;  // Represents the sprite of the Skill
        private float skillCooldownTotal;  // Represents the total (maximum) cooldown period of the Skill (in seconds)
        private float skillCooldownElapsed;  // Represents the elapsed cooldown period of the Skill (in seconds)
        private bool skillCooldownActive;  // Represents whether the cooldown period of the Skill is active

        /* Constructs a new Skill */
        public CharSkill(int skillID, string skillName, string skillDescription, int skillType, int skillTier, Sprite skillIcon, float skillCooldownTotal)
        {
            this.skillID = skillID;
            this.skillName = skillName;
            this.skillDescription = skillDescription;
            this.skillType = skillType;
            this.skillTier = skillTier;
            this.skillIcon = skillIcon;
            this.skillCooldownTotal = skillCooldownTotal;
            this.skillCooldownElapsed = this.skillCooldownTotal;
            this.skillCooldownActive = false;
        }

        /* Sets the Skill ID of the Skill */
        public void setSkillID(int id)
        {
            skillID = id;
        }

        /* Gets the Skill ID of the Skill */
        public int getSkillID()
        {
            return skillID;
        }

        /* Sets the Name of the Skill */
        public void setSkillName(string name)
        {
            skillName = name;
        }

        /* Gets the Name of the Skill */
        public string getSkillName()
        {
            return skillName;
        }

        /* Sets the Description of the Skill */
        public void setSkillDescription(string description)
        {
            skillDescription = description;
        }

        /* Gets the Description of the Skill */
        public string getSkillDescription()
        {
            return skillDescription;
        }

        /* Sets the Type of the Skill */
        public void setSkillType(int type)
        {
            skillType = type;
        }

        /* Gets the Type of the Skill */
        public int getSkillType()
        {
            return skillType;
        }

        /* Sets the Tier of the Skill */
        public void setSkillTier(int tier)
        {
            this.skillTier = tier;
        }

        /* Gets the Tier of the Skill */
        public int getSkillTier()
        {
            return this.skillTier;
        }

        /* Sets the sprite representing the Skill */
        public void setSkillIcon(Sprite icon)
        {
            skillIcon = icon;
        }

        /* Gets the sprite representing the Skill */
        public Sprite getSkillIcon()
        {
            return skillIcon;
        }

        /* Sets the total cooldown period of the Skill, in seconds */
        public void setSkillCooldownTotal(float seconds)
        {
            skillCooldownTotal = seconds;
        }

        /* Gets the total cooldown period of the Skill, in seconds */
        public float getSkillCooldownTotal()
        {
            return skillCooldownTotal;
        }

        /* Sets the elapsed cooldown period of the Skill, in seconds */
        public void setSkillCooldownElapsed(float seconds)
        {
            skillCooldownElapsed = seconds;
        }

        /* Gets the elapsed cooldown period of the Skill, in seconds */
        public float getSkillCooldownElapsed()
        {
            return skillCooldownElapsed;
        }

        /* Activates the Skill and returns the Skill ID */
        public int useSkill()
        {
            /* Checks if cooldown period for slot is still active */
            if (this.getCooldownPeriodActive() == false)
            {
                /* Starts cooldown timer */
                this.setSkillCooldownElapsed(0);

                /* Returns the Skill ID of this Skill */
                return this.getSkillID();
            }

            /* Otherwise, do nothing */
            else
            { 
                /* Returns flag indicating skill not used */
                return -1;
            }
        }
		
		/* Sets whether the Cooldown Period of the Skill is active */
        public void setCooldownPeriodActive(bool isActive)
        {
            this.skillCooldownActive = isActive;
        }

        /* Gets whether the Cooldown Period of the Skill is active */
        public bool getCooldownPeriodActive()
        {
            return this.skillCooldownActive;
        }
    }

    // Use this for initialization
    void Start ()
    {
        /* Initialises all Skill Slots of Character */
        skillSlots = new SkillSlot[4];  // Set to 4 Skill Slots

        /* Retrieves and initialises the Skill Data source */
        this.skillDataSource = this.gameObject.GetComponent<SkillData>();

        /* Retrieves and initialises the Skill Info Box */
        this.skillInfoBoxObject = GameObject.Find("SkillInfo");

        for (int i = 0; i < this.skillSlots.Length; i++)
        {
            skillSlots[i] = new SkillSlot(this.skillSlotObjects[i], this.skillLocked, false);  // Initially, all Skill Slots are disabled
            this.skillInfoObjects[i].text = "";  // Empty Skill Info for disabled Skill Slots
        }

        /* Retrieves the number of Skills available */
        this.setCharSkillsNum(this.skillDataSource.getNumStoredSkills());

        /* Initialises all Skills available to the Character */
        charSkills = new CharSkill[this.getCharSkillsNum()];

        /* Hides the Skill Info Box initially */
        this.skillInfoBoxObject.SetActive(false);
    }
	
	// Updates the Skill System UI Elements every frame */
	void Update ()
    {
       /* If Skill Data has not been loaded, load data  */
       if (this.getSkillDataSourceLoaded() == false)
        {
            /* Checks to see if Skill Data is ready to be loaded */
            if (this.skillDataSource.getSkillDataLoaded() == true)
            {
                /* Loads Skill data */
                for (int i = 0; i < this.charSkills.Length; i++)
                {
                    charSkills[i] = new CharSkill(this.skillDataSource.getSkillListID()[i],
                        this.skillDataSource.getSkillListName()[i],
                        this.skillDataSource.getSkillListDescription()[i],
                        this.skillDataSource.getSkillListType()[i],
                        this.skillDataSource.getSkillListTier()[i],
                        this.skillListIcon[i],
                        this.skillDataSource.getSkillListCooldown()[i]);
                }

                /* Indicate that Skill Data has been loaded */
                this.setSkillDataSourceLoaded(true);
            }
        }
        
       /* Otherwise, Skill System is active */
       else
        {
            /* Updates the cooldown status for each Skill */
            for (int i = 0; i < this.getNumSkillSlots(); i++)
            {
                /* Checks to see if Skill Slot currently has an Active skill */
                if (this.skillSlots[i].getSlotSkill().getSkillType() == 1)
                { 
                    /* Checks to see if cooldown period of Skill is active */
                    if (this.skillSlots[i].getSlotSkill().getSkillCooldownElapsed() < this.skillSlots[i].getSlotSkill().getSkillCooldownTotal())
                    {
                        this.skillSlots[i].getSlotSkill().setCooldownPeriodActive(true);  // Sets the Cooldown Period of the Skill as active
                        this.skillSlots[i].getSlotSkill().setSkillCooldownElapsed(this.skillSlots[i].getSlotSkill().getSkillCooldownElapsed() + Time.deltaTime);  // Increments the elapsed cooldown period
                        this.skillSlots[i].getSlotObject().fillAmount = this.skillSlots[i].getSlotSkill().getSkillCooldownElapsed() / this.skillSlots[i].getSlotSkill().getSkillCooldownTotal();  // Sets the countdown indicator for the Skill Slot
                    }

                    /* If cooldown period is not active */
                    else
                    {
                        this.skillSlots[i].getSlotSkill().setCooldownPeriodActive(false);  // Sets the Cooldown Period of the Skill as inactive
                    }
               }

                /* Updates the Skill Info Box to display the Player's current Skills */

                /* Retrieves and populates text objects with Skill attributes */
               this.skillInfoObjects[i].text = ("SLOT " + (i + 1) + ":   " + this.skillSlots[i].getSlotSkill().getSkillName() + " - " + this.skillSlots[i].getSlotSkill().getSkillDescription());
            }
        }

       /* Detects whether user has pressed the Skill Info Box key */
       if (Input.GetKeyDown(KeyCode.I))
        {
            /* Checks to see if the Skill Info Box is currently visible */
            if (this.skillInfoBoxObject.GetActive() == false)  // If not visible
            {
                this.skillInfoBoxObject.SetActive(true);
            }
            else  // If visible 
            {
                this.skillInfoBoxObject.SetActive(false);
            }
        }
    }

    /* Sets the number of Skill Slots enabled for the Character */
    public void setNumSkillSlots(int numberSlots)
    {
        /* Checks if the number of Skill Slots set is valid */
        if (numberSlots >= 0 || numberSlots <= this.getMaxSkillSlots())
        {
            /* Disables all Skill Slots initially */
            for (int i = 0; i < this.getMaxSkillSlots(); i++)
            {
                skillSlots[i].setSlotEnabled(false);  // Sets the Skill Slot as disabled
            }

            /* Updates the number of Skill Slots enabled */
            for (int i = 0; i < this.getNumSkillSlots(); i++)
            {
                skillSlots[i].setSlotEnabled(true);
            }
            this.numSkillSlots = numberSlots;
        }
    }

    /* Gets the number of Skill Slots enabled for the Character */
    public int getNumSkillSlots()
    {
        return numSkillSlots;
    }

    /* Sets the maximum number of Skill Slots */
    public void setMaxSkillSlots(int numberSlots)
    {
        this.maxSkillSlots = numberSlots;
    }

    /* Gets the maximum number of Skill Slots */
    public int getMaxSkillSlots()
    {
        return this.maxSkillSlots;
    }

    /* Sets whether the Skill Data has been loaded from the data source */
    public void setSkillDataSourceLoaded(bool loaded)
    {
        this.skillDataSourceLoaded = loaded;
    }

    /* Gets whether the Skill Data has been loaded from the data source */
    public bool getSkillDataSourceLoaded()
    {
        return this.skillDataSourceLoaded;
    }

    /* Sets the Skills to Skill Slot assignment */
    public void setSlotAssign(List<int> charSlotAssign)
    {
        this.charSlotAssign = charSlotAssign;

        /* Checks if the number of Skill Slots assigned is less than or equal to the maximum number */
        if (this.getSlotAssign().Count <= this.getMaxSkillSlots())
        {
            /* Updates the number of Skill Slots available */
            setNumSkillSlots(charSlotAssign.Count);
        }
        
        /* Updates the number of Skill Slots available to the maximum number */
        else
        {
            setNumSkillSlots(this.getMaxSkillSlots());
        }

        /* Assigns the specified Skill to each available Skill Slot */
        for (int i = 0; i < this.getNumSkillSlots(); i++)
        {
            /* Assigns the Skill to the Skill Slot */
            skillSlots[i].setSlotSkill(charSkills[charSlotAssign[i]]);
        }
    }

    /* Gets the current Skills to Skill Slot assignment */
    public List<int> getSlotAssign()
    {
        return charSlotAssign;
    }

    /* Uses the Skill in the specified Skill Slot */
    public int useSkillSlot(int slotNum)
    {
        /* Checks if Skill Slot is currently enabled */
        if(skillSlots[slotNum - 1].getSlotEnabled() == true)
        {
            return this.skillSlots[slotNum - 1].getSlotSkill().useSkill();  // Activates the Skill in the Skill Slot
        }

        /* Otherwise, do nothing */
        else
        {
            /* Returns flag indicating skill not used */
            return -1;
        }
    }

    /* Gets the list of all Skills */
    public CharSkill[] getCharSkills()
    {
        return this.charSkills;
    }

    /* Sets the total number of Skills available */
    public void setCharSkillsNum(int numSkills)
    {
        this.charSkillsNum = numSkills;
    }

    /* Gets the total number of Skills available */
    public int getCharSkillsNum()
    {
        return this.charSkillsNum;
    }
	
	/* Gets the specified Skill Slot Object */
    public SkillSlot getSkillSlot(int slotNum)
    {
        return skillSlots[slotNum - 1];
    }
}
