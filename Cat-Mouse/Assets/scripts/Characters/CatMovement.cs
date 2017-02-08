using UnityEngine;
using System.Collections;

public class CatMovement : MonoBehaviour
{
	private const int expToLevel2 = 100;
	private const int expToLevel3 = 200;
	private const int expToLevel4 = 400;
	
   // stats
	private float level = 0;
	private float currentEXP = 0;
    private float maxEXP;
	public float power;
	private float speed = 3.0f; //speed value
	private float jumpForce;//amount of jump force
	public float currentHealth;
	private float maxHealth;
	private int skillPoints;
	private int ultimateSkillPoints;
	private int[] learnedSkills = {3, 4, 5, 6};
	
	// movement speed
	private int movementModifier = 1;
	private float movementModifierTimer = 10f;
	
	// attack
	private float attackPower;
	private float attackCooldownDelay;
	private float attackCooldownTimer = 1f;
	
	private Vector3 moveV; //vector to store movement
    public Rigidbody catrb;
	
	// jump variables
	private bool isGrounded = false;
	
	// skills
	private float[] skillCooldownTimers = new float[4]; // the cooldown timer
	private float[] skillCooldowns = new float[4]; // the max cooldown

    /* HUD state */
    public Vitality catVitality;  // Vitality System component

    void Start()
    {
        catrb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; //cursor is gone from screen

        LevelUp();  // Starts at the first level

        /*  Finds and initialises the Vitality System component */
       GameObject catVitalityGameObject = GameObject.Find("Vitality");
       catVitality = catVitalityGameObject.GetComponent<Vitality>();
    }

	// level up
	public void LevelUp(){
		level += 1;
		if (level == 4)
			ultimateSkillPoints += 1;
		else
			skillPoints += 1;
		power = 5 + level * 5;
		attackPower = power;
		maxHealth = 80 + level * 20;
		jumpForce = 2f + power / 25f;
		attackCooldownDelay = 1.1f - power * 0.1f;
		currentHealth = maxHealth;
	}
	
	// execute a skill (not jump or attack)
	public void useSkill(int skillCode){
		switch (skillCode){
			// skills 1 and 2 are passive
			case 3: 
				Debug.Log("use 3");
				break;
			case 4:
				Debug.Log("use 4");
				break;
			case 5: 
				Debug.Log("use 5");
				break;
			case 6:
				Debug.Log("use 6");
				break;
			case 7: 
				break;
			case 8:
				break;
			case 9: 
				break;
		}
	}
	
    void FixedUpdate()
    {
        
        /* Updates the HUD state for the current player */
        if (GetComponent<PhotonView>().isMine)
        {
            /* Updates the Health Points attributes of the Cat */
            catVitality.setMaxHealthPoints(maxHealth); // Updates the Maximum Health Points
            catVitality.setCurrentHealthPoints(currentHealth);  // Updates the Current Health Points

            /* Updates the Experience Points attributes */
            catVitality.setMaximumExperiencePoints(maxEXP);
            catVitality.setCurrentExperiencePoints(currentEXP);
        }

        // keyboard commands
        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None; //if we press esc, cursor appears on screen
        }
		// jump control
		if (isGrounded){
			moveV = new Vector3(0, 0, 0);
			if (Input.GetKey(KeyCode.A)){
				moveV = new Vector3(-1, 0, moveV.z);
			}
			if (Input.GetKey(KeyCode.D)){
				moveV = new Vector3(1, 0, moveV.z);
			}
			if (Input.GetKey(KeyCode.W)){
				moveV = new Vector3(moveV.x, 0, 1);				
			}		
			if (Input.GetKey(KeyCode.S)){
				moveV = new Vector3(moveV.x, 0, -1);				
			}
			if (Input.GetKeyDown(KeyCode.Space)){
				isGrounded = false;
				Debug.Log(moveV.x +  " " + moveV.y + " " + moveV.z);
				catrb.AddForce(new Vector3(0, jumpForce, 0));
			}
		}
		moveV = moveV.normalized * speed * movementModifier * Time.deltaTime;
		transform.Translate(moveV);

		// left click
		if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0){
			attackCooldownTimer = attackCooldownDelay;
			Attack();
		}
		// skills
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			useSkill(learnedSkills[0]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)){
			useSkill(learnedSkills[1]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)){
			useSkill(learnedSkills[2]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)){
			useSkill(learnedSkills[3]);
		}
		
		// interactions
		if (Input.GetKeyDown(KeyCode.E)){
			InteractWithObject();
		}
		if (Input.GetKeyDown(KeyCode.K)){
			TakeDamage(5);
		}
		
		// timer actions
		if (movementModifierTimer > 0f)
			movementModifierTimer -= Time.deltaTime;
		else{
			movementModifier = 1;
		}
		if (attackCooldownTimer > 0){
			attackCooldownTimer -= Time.deltaTime;
		}

    }
	
	// 
	void OnCollisionEnter(Collision collisionInfo){
		if (collisionInfo.gameObject.tag == "MazeGround");{
			isGrounded = true;
		}
	}
	
	// take a certain amount of damage
	public void TakeDamage(int amt){
		currentHealth -= amt;
		if (currentHealth <= 0){
			currentHealth = 0;
			Death();
		}
	}
	
	void Death(){
		Debug.Log("player died");
	}
	
	// attack in front of player
	void Attack(){
		Vector3 attackCenter = transform.position + transform.forward * 0.8f;
		Collider[] hitColliders = Physics.OverlapSphere(attackCenter,0.5f);
        
		// just to test where the attack hitbox is
		/*
		GameObject newGO = (GameObject)PhotonNetwork.Instantiate("Powerup", attackCenter, transform.rotation, 0);
		Powerup newPowerup = newGO.GetComponent<Powerup>();
		newPowerup.setType(0);*/
		
		// check the hitbox area
        int i = 0;
        while (i < hitColliders.Length) {
			if (hitColliders[i].tag == "Monster"){
				hitColliders[i].GetComponent<MonsterAI>().takeDamage(50);
			}
            i++;
        }
	}
	
	// allow the player to open and close doors
	void InteractWithObject(){
		Vector3 pushCenter = transform.position + transform.forward * 0.6f;
		Collider[] hitColliders = Physics.OverlapSphere(pushCenter,0.8f);
		// check the hitbox area
        int i = 0;
        while (i < hitColliders.Length) {
			if (hitColliders[i].tag == "Door"){
				hitColliders[i].transform.parent.GetComponent<MazeDoor>().Interact();
			}
            i++;
        }
	}
	
	// when player collides with powerup
	void OnTriggerEnter(Collider obj){
		if(obj.tag == "Powerup"){
			Powerup pup = obj.GetComponent<Powerup>();
			// movement speed boost
			if (pup.powerupType == 0){
				movementModifier = 2;
				movementModifierTimer = 10f;
			}
			// damage boost
			else if (pup.powerupType == 1){
				
			}
			// invis
			else if (pup.powerupType == 2){
				
			}
			// exp boost
			else if (pup.powerupType == 3){
				
			}
			Debug.Log("destroy " + obj);
			Destroy(obj.gameObject);
		}
	}
	
}