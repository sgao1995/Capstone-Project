using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MouseMovement : MonoBehaviour {
    private const int expToLevel2 = 100;
    private const int expToLevel3 = 200;
    private const int expToLevel4 = 400;

    // stats
    private int level = 0;
    private float currentEXP = 0;
    private float maxEXP;
    public float power;
    private float speed = 3.0f; //speed value
    private float jumpForce;//amount of jump force
    public float currentHealth;
    private float maxHealth;
    private int skillPoints;
    private int ultimateSkillPoints;
    private int[] learnedSkills = { 3, 4, 5, 6 };

    // movement speed
    private int movementModifier = 1;
    private float movementModifierTimer = 10f;

    // attack
    private Animator animator;
    private float attackPower;
    private float attackCooldownDelay;
    private float attackCooldownTimer = 1f;

    private Vector3 moveV; //vector to store movement
    public Rigidbody mouserb;

    // jump variables
    private bool isGrounded = false;
    // status effects
    private bool onLava = false;
	private bool onIce = false;
    private bool onSpikes = false;
    private bool alive = true;
	private bool canToggleDoor = false;
	private bool canTakeKey = false;
	private bool canOpenChest = false;
	private bool canTakePuzzlePiece = false;
	
	// keys and puzzle pieces on hand
	public int numKeysHeld = 0;
	List<int> puzzlePiecesHeld = new List<int>();

    // skills
    private float[] skillCooldownTimers = new float[4]; // the cooldown timer
    private float[] skillCooldowns = new float[4]; // the max cooldown

    /* Vitality System attribute parameters */
    private float[] vitalLevelHP = {50, 65, 80, 110};  // Health Points of Mouse per Level
    private float[] vitalLevelEXP = {320, 640, 1280, 2500};  // Experience Points Mouse per Level

    /* HUD state */
    public Vitality mouseVitality;  // Vitality System component
	public Text interactText;

    void Start()
    {
        mouserb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; //cursor is gone from screen

        LevelUp();  // Starts at the first level
        animator = GetComponent<Animator>();

        /*  Finds and initialises the Vitality System component */
        GameObject mouseVitalityGameObject = GameObject.Find("Vitality");
        mouseVitality = mouseVitalityGameObject.GetComponent<Vitality>();
		
		GameObject interactiveText = GameObject.Find("Text");
		interactText = interactiveText.GetComponent<Text>();
		interactText.text = "";

    }

    // level up
    public void LevelUp()
    {
        level += 1;
        if (level == 4)
            ultimateSkillPoints += 1;
        else
            skillPoints += 1;
        power = 5 + level * 5;
        attackPower = power;
        maxHealth = 80 + level * 20;
        jumpForce = 250f;
        attackCooldownDelay = 1.1f - power * 0.1f;
        currentHealth = maxHealth;

        /* Sets Character Maximum Health for new Level */
        this.maxHealth = this.vitalLevelHP[this.level - 1];
        this.currentHealth = this.maxHealth;

        /* Sets Character Maximum Experience for new level */
        this.maxEXP = this.vitalLevelEXP[this.level - 1];
        this.currentEXP = 0;
    }

    // execute a skill (not jump or attack)
    public void useSkill(int skillCode)
    {
        switch (skillCode)
        {
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
            /* Updates the Level attributes */
            mouseVitality.setCurrentLevel(this.level);

            /* Updates the Health Points attributes of the Mouse */
            mouseVitality.setMaxHealthPoints(this.maxHealth); // Updates the Maximum Health Points
            mouseVitality.setCurrentHealthPoints(this.currentHealth);  // Updates the Current Health Points

            /* Updates the Experience Points attributes */
            mouseVitality.setMaximumExperiencePoints(this.maxEXP);
            mouseVitality.setCurrentExperiencePoints(this.currentEXP);

            //tell that it is a mouse
            mouseVitality.setCharacterType("mouse");

        }

        // status effects
        if (onLava)
        {
            TakeDamage(0.5f);
        }
        if (onSpikes)
        {
            TakeDamage(0.2f);
        }
        // dont let player move if they are on spikes
        else
        {	
			if (onIce){
				moveV = moveV.normalized * speed * movementModifier * Time.deltaTime * 0.1f;
			}
			else{
				moveV = moveV.normalized * speed * movementModifier * Time.deltaTime;
			}

            transform.Translate(moveV);
        }

        // keyboard commands
        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None; //if we press esc, cursor appears on screen
        }
        // movement control
        if (isGrounded && !onSpikes)
        {
            moveV = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.A))
            {
                animator.Play("Unarmed-Strafe-Left");

				if (onIce)
					mouserb.AddRelativeForce(Vector3.left*0.2f, ForceMode.Impulse);
				else{
					moveV = new Vector3(-1, 0, moveV.z);
				}
            }
            if (Input.GetKey(KeyCode.D))
            {
                animator.Play("Unarmed-Strafe-Right");
                
				if (onIce)
					mouserb.AddRelativeForce(Vector3.right*0.2f, ForceMode.Impulse);
				else{
					moveV = new Vector3(1, 0, moveV.z);
				}
            }
            if (Input.GetKey(KeyCode.W))
            {
                animator.Play("Unarmed-Strafe-Forward");
                
				if (onIce)
					mouserb.AddRelativeForce(Vector3.forward*0.2f, ForceMode.Impulse);
				else{
					moveV = new Vector3(moveV.x, 0, 1);
				}
            }
            if (Input.GetKey(KeyCode.S))
            {
                animator.Play("Unarmed-Strafe-Backward");
                
				if (onIce)
					mouserb.AddRelativeForce(Vector3.back*0.2f, ForceMode.Impulse);
				else{
					moveV = new Vector3(moveV.x, 0, -1);
				}
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                animator.Play("Unarmed-Jump");
                mouserb.AddForce(new Vector3(0, jumpForce, 0));
            }
        }


        // left click
        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0 && !Input.GetKey(KeyCode.Escape))
        {
            attackCooldownTimer = attackCooldownDelay;
            Attack();
        }
        // skills
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            useSkill(learnedSkills[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            useSkill(learnedSkills[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            useSkill(learnedSkills[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            useSkill(learnedSkills[3]);
        }

        // interactions
		if (canToggleDoor || canTakeKey || canOpenChest || canTakePuzzlePiece){
			if (Input.GetKeyDown(KeyCode.E)){
				InteractWithObject();
			}
		}
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(5f);
        }
		if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = new Vector3(22, 0, 25);
        }

        // timer actions
        if (movementModifierTimer > 0f)
            movementModifierTimer -= Time.deltaTime;
        else
        {
            movementModifier = 1;
        }
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

    }

    // take a certain amount of damage
    public void TakeDamage(float amt)
    {
        transform.GetComponent<PhotonView>().RPC("changeHealth", PhotonTargets.AllBuffered, amt);
    }
    [PunRPC]
    void changeHealth(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    void Death()
    {
        Debug.Log("player died");
        alive = false;
        //animator.Play("Unarmed-Death1");
    }

    // attack in front of player
    void Attack()
    {
        animator.Play("Unarmed-Attack-L3");

    }
    void DealDamage()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        Debug.Log("hitting");

        if (Physics.Raycast(ray, out hitInfo, 1))
        {
            Debug.Log("We hit: " + hitInfo.collider.name);
            if (hitInfo.collider.name == "Character" || hitInfo.collider.name == "MonsterClone" || hitInfo.collider.name == "Monster")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.parent.name + " by calling script " + hitInfo.collider.transform.parent.GetComponent<MonsterAI>().name);
                hitInfo.collider.transform.parent.GetComponent<MonsterAI>().SendMessage("takeDamage", 25f);

                if (hitInfo.collider.transform.parent.GetComponent<MonsterAI>().getHealth() <= 0)
                {
                    Debug.Log("got exp");

                    currentEXP += 50;
                    //mouseVitality.setCurrentExperiencePoints(currentEXP);
                    maxEXP += 50;
                    Debug.Log("current EXP is" + mouseVitality.getEXP());
                }
            }
            if (hitInfo.collider.name == "Cat(Clone)" || hitInfo.collider.name == "Cat")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.parent.name + " by calling script " + hitInfo.collider.transform.parent.GetComponent<CatMovement>().name);
                hitInfo.collider.transform.parent.GetComponent<CatMovement>().SendMessage("TakeDamage", 10f);
                if (hitInfo.collider.transform.parent.GetComponent<CatMovement>().getHealth() <= 0)
                {
                    currentEXP += 100;
                    maxEXP += 100;
                }
            }
          /*  if (hitInfo.collider.name == "Mouse(Clone)")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.parent.name + " by calling script " + hitInfo.collider.transform.parent.GetComponent<CatMovement>().name);
                hitInfo.collider.transform.parent.GetComponent<CatMovement>().SendMessage("TakeDamage", 10f);
                if (hitInfo.collider.transform.parent.GetComponent<MouseMovement>().getHealth() <= 0)
                {
                    currentEXP += 100;
                    maxEXP += 100;
                }
            }*/
        }
    }

    // allow the player to open and close doors
    void InteractWithObject()
    {
        Vector3 pushCenter = transform.position + transform.forward * 0.6f;
		Collider[] hitColliders = Physics.OverlapSphere(pushCenter,0.8f);
		// check the hitbox area
        int i = 0;
        while (i < hitColliders.Length) {
			if (hitColliders[i].tag == "Door"){
				hitColliders[i].transform.GetComponent<MazeDoor>().Interact();
			}
			if (hitColliders[i].tag == "Chest"){
				if (numKeysHeld > 0){
					numKeysHeld -= hitColliders[i].transform.GetComponent<Chest>().Interact();
					break;
				}
			}
			if (hitColliders[i].tag == "Key"){
				hitColliders[i].transform.GetComponent<Key>().Interact();
				numKeysHeld++;
				interactText.text = "";
				canTakeKey = false;
				break;
			}
			if (hitColliders[i].tag == "PuzzlePiece"){
				puzzlePiecesHeld.Add(hitColliders[i].transform.GetComponent<PuzzlePiece>().Interact());
				interactText.text = "";
				canTakeKey = false;
				break;
			}
            i++;
        }
    }
	
    // collision with objects
    void OnCollisionEnter(Collision collisionInfo)
    {
        isGrounded = true;
        if (collisionInfo.gameObject.tag == "Ground")
        {
            onLava = false;
			onIce = false;
        }
        // if enters lava
        if (collisionInfo.gameObject.tag == "Lava")
        {
            onLava = true;
        }
		if (collisionInfo.gameObject.tag == "Ice")
        {
            onIce = true;
        }
        // if steps on a mine
        if (collisionInfo.gameObject.tag == "Mine")
        {
            Mine mine = collisionInfo.gameObject.GetComponent<Mine>();
            TakeDamage(mine.mineSize * 50);
            transform.GetComponent<PhotonView>().RPC("destroyMine", PhotonTargets.MasterClient, collisionInfo);
        }
    }
    [PunRPC]
    void destroyMine(Collision collisionInfo)
    {

        PhotonNetwork.Destroy(collisionInfo.gameObject);
    }
	void OnTriggerStay(Collider obj){
		// enters range to use an object. Certain objects take priority over others
		if (obj.tag == "Door" && !canTakeKey && !canOpenChest){
			MazeDoor door = obj.gameObject.GetComponent<MazeDoor>();
			if (door.doorOpen){
				interactText.text = "Press E to close Door";
			}
			else{
				interactText.text = "Press E to open Door";
			}
			canToggleDoor = true;
		}
	}

    // when player leaves the trigger
    void OnTriggerExit(Collider obj)
    {
		if (obj.tag == "Spike"){
			onSpikes = false;
		}
		if (obj.tag == "Door"){
			interactText.text = "";
			canToggleDoor = false;
		}
		if (obj.tag == "Key"){
			interactText.text = "";
			canTakeKey = false;
		}
		if (obj.tag == "Chest"){
			interactText.text = "";
			canOpenChest = false;
		}
		if (obj.tag == "PuzzlePiece"){
			interactText.text = "";
			canTakePuzzlePiece = false;
		}
    }
    [PunRPC]
    void destroyPU(Collider obj)
    {

        PhotonNetwork.Destroy(obj.gameObject);
    }
    // when player collides with powerup
    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Powerup")
        {
            Powerup pup = obj.GetComponent<Powerup>();
            // movement speed boost
            if (pup.powerupType == 0)
            {
                movementModifier = 2;
                movementModifierTimer = 10f;
            }
            // damage boost
            else if (pup.powerupType == 1)
            {

            }
            // invis
            else if (pup.powerupType == 2)
            {

            }
            // exp boost
            else if (pup.powerupType == 3)
            {

            }
            //Debug.Log("destroy " + obj);
            transform.GetComponent<PhotonView>().RPC("destroyPU", PhotonTargets.MasterClient, obj);
        }

        // put spikes here because we dont want spikes displacing the player
        if (obj.tag == "Spike")
        {
            onSpikes = true;
        }
	
		if (obj.tag == "Key"){
			interactText.text = "Press E to take Key";
			canTakeKey = true;
		}
		if (obj.tag == "Chest" && !canTakeKey){
			Chest chest = obj.gameObject.GetComponent<Chest>();
			if (!chest.chestOpen){
				if (numKeysHeld > 0){
					interactText.text = "Press E to open Chest";
					canOpenChest = true;
				}
				else if (numKeysHeld == 0){
					interactText.text = "You require a Key";
				}
			}
		}
		if (obj.tag == "PuzzlePiece"){
			interactText.text = "Press E to take Puzzle Piece";
			canTakePuzzlePiece = true;
		}
    }
    public float getHealth()
    {
        return currentHealth;
    }
}
