using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* Class: Skill.cs
 * Description: Controls the behaviour of the Skill System of the HUD.
 */ 
public class Skill : MonoBehaviour {

    /* Sets Character Skill Slot data */
    private int numSkillSlots;  // Represents number of Skill Slots enabled
    private int maxSkillSlots;  // Represents maximum number of Skill Slots

    /* Represents the objects within the Skill System */

    /* Represents each invidividual Skill Slot */
    public SkillSlot[] charSkills;

    /* Represents the Skill Slot Objects in the HUD */
    public Image[] skillSlotObjects;

    /* Represents the Sprites of all Skills */
    public Sprite[] skillListIcons;  /* PLACEHOLDER: Will be loaded from a seperate class */
    public Sprite skillLocked;  // Represents the sprite for a Locked Skill Slot

    /* Represents the cooldown times of all Skills */
    public float[] skillListCooldown;  /* PLACEHOLDER: Will be loaded from a seperate class */

    /* Represents an individual Skill Slot */
    public class SkillSlot
    {
        /* Attributes of Skills in Skill Slot */
        private Image slotObject;  // Represents the corresponding scene object of the Skill Slot
        private bool slotEnabled;  // Represents whether the Skill Slot is enabled
        private Sprite slotDisabledIcon;  // Represents the sprite of a disabled Skill Slot
        private Sprite skillIcon;  // Represents the sprite of the current Skill
        private float skillCooldownTotal;  // Represents the total (maximum) cooldown period of the current Skill (in seconds)
        private float skillCooldownElapsed;  // Represents the elapsed cooldown period of the current Skill (in seconds)

        /* Constructs a new Skill Slot */
        public SkillSlot(Image slotObject, Sprite slotDisabledIcon, Sprite skillIcon,  float skillCooldownTotal)
        {
            /* Sets each attribute to the specified value */
            this.slotObject = slotObject;
            this.slotDisabledIcon = slotDisabledIcon;
            this.skillIcon = skillIcon;
            this.skillCooldownTotal = skillCooldownTotal;
            this.skillCooldownElapsed = this.skillCooldownTotal;
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

        /* Sets whether the Skill Slot is enabled */
        public void setSlotEnabled(bool enabled)
        {
            if (enabled == true)
            {
                this.getSlotObject().sprite = this.getSkillIcon();
            }

            else
            {
                this.getSlotObject().sprite = this.getSlotDisabledIcon();
            }
            this.slotEnabled = enabled;
        }

        /* Gets whether the Skill Slot is enabled */
        public bool getSlotEnabled()
        {
            return this.slotEnabled;
        }

        /* Sets the sprite of a disabled Skill Slot */
        public void setSlotDisabledIcon(Sprite icon)
        {
            this.slotDisabledIcon = icon;
        }

        /* Gets the sprite of a disabled Skill Slot */
        public Sprite getSlotDisabledIcon()
        {
            return this.slotDisabledIcon;
        }

        /* Sets the sprite representing the current Skill */
        public void setSkillIcon(Sprite icon)
        {
            this.skillIcon = icon;
        }

        /* Sets the total cooldown period of the current Skill, in seconds */
        public void setSkillCooldownTotal(float seconds)
        {
            this.skillCooldownTotal = seconds;
        }

        /* Sets the elapsed cooldown period of the current Skill, in seconds */
        public void setSkillCooldownElapsed(float seconds)
        {
            this.skillCooldownElapsed = seconds;
        }

        /* Gets the sprite representing the current Skill */
        public Sprite getSkillIcon()
        {
            return this.skillIcon;
        }

        /* Gets the total cooldown period of the current Skill, in seconds */
        public float getSkillCooldownTotal()
        {
            return this.skillCooldownTotal;
        }

        /* Gets the elapsed cooldown period of the current Skill, in seconds */
        public float getSkillCooldownElapsed()
        {
            return this.skillCooldownElapsed;
        }

        /* Activates the current Skill in the Skill Slot */
        public void useSkill()
        {
            /* Checks if cooldown period for slot is still active */
            if (this.getSkillCooldownElapsed() >= this.getSkillCooldownTotal())
            {
                /* Starts cooldown timer */
                this.setSkillCooldownElapsed(0);
            }  
        }
    }

    // Use this for initialization
    void Start ()
    {
        /* Initialises all Skill Slots of Character */
        charSkills = new SkillSlot[4];  // Set to 4 Skill Slots

        for (int i = 0; i < charSkills.Length; i++)
        {
            charSkills[i] = new SkillSlot(this.skillSlotObjects[i], this.skillLocked, this.skillListIcons[i], this.skillListCooldown[i]);
        }
    }
	
	// Updates the Skill System UI Elements every frame */
	void Update ()
    {
       /* Updates the cooldown indicator for each Skill Slot */
       for (int i = 0; i < this.getNumSkillSlots(); i++)
        {
            /* Check to see if cooldown period has started */
            if (this.charSkills[i].getSkillCooldownElapsed() < this.charSkills[i].getSkillCooldownTotal())
            {
                this.charSkills[i].setSkillCooldownElapsed(this.charSkills[i].getSkillCooldownElapsed() + Time.deltaTime);  // Increments the elapsed cooldown period
                this.charSkills[i].getSlotObject().fillAmount = this.charSkills[i].getSkillCooldownElapsed() / this.charSkills[i].getSkillCooldownTotal();
            }
        }
    }

    /* Sets the number of Skill Slots enabled for the Character */
    public void setNumSkillSlots(int numberSlots)
    {
        /* Checks if the number of Skill Slots set is valid */
        if (numberSlots > 0 || numberSlots <= this.getMaxSkillSlots())
        {
            /* Disables all Skill Slots initially */
            for (int i = 0; i < this.getMaxSkillSlots(); i++)
            {
                charSkills[i].setSlotEnabled(false);  // Sets the Skill Slot as disabled
            }

            /* Updates the number of Skill Slots enabled */
            for (int i = 0; i < this.getNumSkillSlots(); i++)
            {
                charSkills[i].setSlotEnabled(true);
            }
            this.numSkillSlots = numberSlots;
        }
    }

    /* Gets the number of Skill Slots enabled for the Character */
    public int getNumSkillSlots()
    {
        return this.numSkillSlots;
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

    /* Uses the Skill in the specified Skill Slot */
    public void useSkillSlot(int slotNum)
    {
        this.charSkills[slotNum - 1].useSkill();
    }
}
