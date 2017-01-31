using UnityEngine;
using System.Collections;

/* Class: Vitality.cs
 * Description: Controls the behaviour of the Vitality System of the HUD. 
 */
public class Vitality : MonoBehaviour {
    
    /* Character Health Bar data */
    private float currentHealthPoints;
    private float maxHealthPoints;
    private float healthPointsRatio;

    /* Character Leveling System data */
    private float currentLevel;

    /* Character Experience Bar data */
    private float currentExperiencePoints;
    private float maxExperiencePoints;

    /* Represents the objects within the Vitality System */
    public RectTransform rectHealthCurrent;

    /* Constructs a new instance of the Vitality System and sets initial character values */
    void Start()
    {
        this.currentLevel = 1;
        this.maxExperiencePoints = 0;
    }

    /* Updates Vitality System UI Elements every frame */
    void Update()
    {
        rectHealthCurrent.sizeDelta = new Vector2((this.healthPointsRatio * 150), rectHealthCurrent.sizeDelta.y);
        Debug.Log(rectHealthCurrent.sizeDelta.x);
    }

    /* Sets the current Health Points of the Character */
    public void setCurrentHealthPoints(float healthPoints)
    {
        /* Checks if the maximum Health Point has been correctly set */
        if (this.maxHealthPoints > 0)
        {
             /* Checks if specified health point value is valid */
            if (healthPoints <= this.maxHealthPoints)
            {
                this.currentHealthPoints = healthPoints;  // Sets the current Health Points
                this.healthPointsRatio = this.currentHealthPoints /  this.maxHealthPoints;  // Calculates the current Health Point ratio
                Debug.Log("The current HP is: " + currentHealthPoints);
            }
        }
    }

    /* Sets the maximum Health Points of the Character */
    public void setMaxHealthPoints(float healthPoints)
    {
        /* Checks if the specified health point value is valid */
        if (healthPoints > 0)
        {
            this.maxHealthPoints = healthPoints;

            /* Checks if the specified health point value is less than the current health point value of the Character */
            if (healthPoints > this.maxHealthPoints)
            {
                /* If so, make the current health point the value the same as the maximum health point value */
                this.currentHealthPoints = this.maxHealthPoints;
            }
            this.healthPointsRatio = this.currentHealthPoints / this.maxHealthPoints;  // Calculates ratio
            this.healthPointsRatio = 0.5f;
        }
    }

    /* Sets the current Level of the Character */
    public void setCurrentLevel(float level)
    {
        this.currentLevel = level;
    }

    /* Sets the current Experience Points of the Character */
    public void setCurrentExperiencePoints(float experiencePoints)
    {
        this.currentExperiencePoints = experiencePoints;
    }

    /* Sets the maximum Experience Points of the Character */
    public void setMaximumExperiencePoints(float experiencePoints)
    {
        this.maxExperiencePoints = experiencePoints;
    }
}
