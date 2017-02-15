using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* Class: Skill.cs
 * Description: Controls the behaviour of the Skill System of the HUD.
 */ 
public class Skill : MonoBehaviour {

    /* Sets Character Skill Slot data */
    private int numSkillSlots;

    /* Represents the objects within the Skill System */

    /* Represents each invidividual Skill Slot */
    public SkillSlot skillSlotOne;
    public SkillSlot skillSlotTwo;
    public SkillSlot skillSlotThree;
    public SkillSlot skillSlotFour;

    /* Represents the Skill Slot Objects in the HUD */
    public Image imgSkillSlotOne;
    public Image imgSkillSlotTwo;
    public Image imgSkillSlotThree;
    public Image imgSkillSlotFour;

    /* Represents the Sprites of all Skills */
    public Sprite[] skillListIcons;  /* PLACEHOLDER: Will be loaded from a seperate class */
    public Sprite skillLocked;  // Represents the sprite for a Locked Skill Slot

    /* Represents an individual Skill Slot */
    public class SkillSlot
    {
        /* Attributes of Skills in Skill Slot */
        private bool slotEnabled;  // Represents whether the Skill Slot is enabled
        private Sprite skillIcon;  // Represents the sprite of the Skill
        private float skillCooldownTotal;  // Represents the total (maximum) cooldown period of the Skill (in seconds)
        private float skillCooldownRemaining;  // Represents the remaining  cooldown period (in seconds)

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

        /* Sets the remaining cooldown period of the current Skill, in seconds */
        public void setSkillCooldownRemaining(float seconds)
        {
            this.skillCooldownRemaining = seconds;
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

        /* Gets the remaining cooldown period of the current Skill, in seconds */
        public float getSkillCooldownRemaining()
        {
            return this.skillCooldownRemaining;
        }
    }

    // Use this for initialization
    void Start () {

        /* Initialises each Skill Slot */
        skillSlotOne = new SkillSlot();
        skillSlotTwo = new SkillSlot();
        skillSlotThree = new SkillSlot();
        skillSlotFour = new SkillSlot();
    }
	
	// Updates the Skill System UI Elements every frame */
	void Update () {

        /* Updates the number of Skill Slots enabled */

        /* If one Skill Slot is enabled */
        if (this.numSkillSlots == 1)
        {
            /* Sets the Icon of the current Skill */
            skillSlotOne.setSkillIcon(this.skillListIcons[0]);
            skillSlotTwo.setSkillIcon(this.skillLocked);
            skillSlotThree.setSkillIcon(this.skillLocked);
            skillSlotFour.setSkillIcon(this.skillLocked);
        }
        else if (this.numSkillSlots == 2)
        {
            skillSlotOne.setSkillIcon(this.skillListIcons[0]);
            skillSlotTwo.setSkillIcon(this.skillListIcons[1]);
            skillSlotThree.setSkillIcon(this.skillLocked);
            skillSlotFour.setSkillIcon(this.skillLocked);
        }
        else if (this.numSkillSlots == 3)
        {
            skillSlotOne.setSkillIcon(this.skillListIcons[0]);
            skillSlotTwo.setSkillIcon(this.skillListIcons[1]);
            skillSlotThree.setSkillIcon(this.skillListIcons[2]);
            skillSlotFour.setSkillIcon(this.skillLocked);
        }
        else if (this.numSkillSlots == 4)
        {
            skillSlotOne.setSkillIcon(this.skillListIcons[0]);
            skillSlotTwo.setSkillIcon(this.skillListIcons[1]);
            skillSlotThree.setSkillIcon(this.skillListIcons[2]);
            skillSlotFour.setSkillIcon(this.skillListIcons[3]);
        }

        /* Updates the current Skill Icon of each Skill Object */
        imgSkillSlotOne.sprite = skillSlotOne.getSkillIcon();
        imgSkillSlotTwo.sprite = skillSlotTwo.getSkillIcon();
        imgSkillSlotThree.sprite = skillSlotThree.getSkillIcon();
        imgSkillSlotFour.sprite = skillSlotFour.getSkillIcon();
    }

    /* Sets the number of Skill Slots enabled for the Character */
    public void setNumSkillSlots(int numberSlots)
    {
        /* Checks if the number of Skill Slots set is valid */
        if (numberSlots > 0 || numberSlots <= 4)
        {
            this.numSkillSlots = numberSlots;
        }
    }
}
