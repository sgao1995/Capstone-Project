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

    /* Represents all Skills available to the Character */
    public CharSkill[] charSkills;

    /* Represents the Skill Data for all Skills */
    public string[] skillListName;  // Represents the Skill Name of all Skills
    public string[] skillListDescription;  // Represents the Skill Description of all Skills
    public int[] skillListType;  // Represents the Skill Type (0: Passive, 1: Active)
    public Sprite[] skillListIcon;  // Represents the Sprites of all Skills
    public Sprite skillLocked;  // Represents the sprite for a Locked Skill Slot
    public float[] skillListCooldown;   // Represents the cooldown times of all Skills

    /* Sets Character Skill Slot data */
    private int numSkillSlots;  // Represents number of Skill Slots enabled
    private int maxSkillSlots;  // Represents maximum number of Skill Slots

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
        private Sprite skillIcon;  // Represents the sprite of the Skill
        private float skillCooldownTotal;  // Represents the total (maximum) cooldown period of the Skill (in seconds)
        private float skillCooldownElapsed;  // Represents the elapsed cooldown period of the Skill (in seconds)

        /* Constructs a new Skill */
        public CharSkill(int skillID, string skillName, string skillDescription, int skillType, Sprite skillIcon, float skillCooldownTotal)
        {
            this.skillID = skillID;
            this.skillName = skillName;
            this.skillDescription = skillDescription;
            this.skillType = skillType;
            this.skillIcon = skillIcon;
            this.skillCooldownTotal = skillCooldownTotal;
            this.skillCooldownElapsed = this.skillCooldownTotal;
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

        /* Activates the Skill */
        public void useSkill()
        {
            /* Checks if cooldown period for slot is still active */
            if (getSkillCooldownElapsed() >= getSkillCooldownTotal())
            {
                /* Starts cooldown timer */
                setSkillCooldownElapsed(0);
            }  
        }
    }

    // Use this for initialization
    void Start ()
    {
        /* Initialises all Skill Slots of Character */
        skillSlots = new SkillSlot[4];  // Set to 4 Skill Slots

        for (int i = 0; i < this.skillSlots.Length; i++)
        {
            skillSlots[i] = new SkillSlot(this.skillSlotObjects[i], this.skillLocked, false);  // Initially, all Skill Slots are disabled
        }

        /* Initialises all Skills available to the Character */
        charSkills = new CharSkill[11];  // PLACEHOLDER: Set to 11 skills

        for (int i = 0; i < this.charSkills.Length; i++)
        {
            charSkills[i] = new CharSkill(i, this.skillListName[i], this.skillListDescription[i], this.skillListType[i], this.skillListIcon[i], this.skillListCooldown[i]);  // Loads Skill data
        }
    }
	
	// Updates the Skill System UI Elements every frame */
	void Update ()
    {
       /* Updates the cooldown status for each Skill */
       for (int i = 0; i < this.getNumSkillSlots(); i++)
        {
            /* Checks to see if Skill Slot currently has an Active skill */
            if (this.skillSlots[i].getSlotSkill().getSkillType() == 1)
            { 
                /* Checks to see if cooldown period has started */
                if (this.skillSlots[i].getSlotSkill().getSkillCooldownElapsed() < this.skillSlots[i].getSlotSkill().getSkillCooldownTotal())
                {
                    this.skillSlots[i].getSlotSkill().setSkillCooldownElapsed(this.skillSlots[i].getSlotSkill().getSkillCooldownElapsed() + Time.deltaTime);  // Increments the elapsed cooldown period
                    this.skillSlots[i].getSlotObject().fillAmount = this.skillSlots[i].getSlotSkill().getSkillCooldownElapsed() / this.skillSlots[i].getSlotSkill().getSkillCooldownTotal();  // Sets the countdown indicator for the Skill Slot
                }
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

    /* Sets the Skills to Skill Slot assignment */
    public void setSlotAssign(List<int> charSlotAssign)
    {
        this.charSlotAssign = charSlotAssign;

        /* Assigns the specified Skill to each available Skill Slot */
        for (int i = 0; i < this.getNumSkillSlots(); i++)
        {
             /* Assigns the Skill to the Skill Slot */
             skillSlots[i].setSlotSkill(charSkills[charSlotAssign[i]]);     
        }

        /* Updates the number of Skill Slots available */
        setNumSkillSlots(charSlotAssign.Count);
    }

    /* Gets the current Skills to Skill Slot assignment */
    public List<int> getSlotAssign()
    {
        return charSlotAssign;
    }

    /* Uses the Skill in the specified Skill Slot */
    public void useSkillSlot(int slotNum)
    {
        /* Checks if Skill Slot is currently enabled */
        if(skillSlots[slotNum - 1].getSlotEnabled() == true)
        {
            this.skillSlots[slotNum - 1].getSlotSkill().useSkill();
        }
       
    }
}
