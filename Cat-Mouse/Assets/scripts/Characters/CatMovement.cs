using UnityEngine;
using System.Collections;

public class CatMovement : MonoBehaviour
{
    private float speed = 3.0f; //speed value
    private Vector3 moveV; //vector to store movement
    public Rigidbody catrb;
    private int jumpForce = 5;//amount of jump force
    private bool isJumping; //flag to check if user is already jumping or not
	
	// movement speed
	private int movementModifier = 1;
	private float movementModifierTimer = 10f;
	
	// attack
	public float attackPower = 1;
	private float attackCooldownDelay = 1f;
	private float attackCooldownTimer = 1f;
	
	// skills
	
    void Start()
    {
        catrb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; //cursor is gone from screen
    }

    void FixedUpdate()
    {
		// keyboard commands
		if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None; //if we press esc, cursor appears on screen
        }
        //jump control
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false)//if user press space and is not already jumping
        {	
			// RIGHT NOW THE CURRENT JUMP BEHAVIOUR ALLOWS WALL CLIMBING
            catrb.velocity = new Vector3(0, jumpForce, 0);       
			isJumping = true;
        }
		// left click
		if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0){
			attackCooldownTimer = attackCooldownDelay;
			Attack();
		}
		if (Input.GetKeyDown(KeyCode.E)){
			InteractWithObject();
		}
		
		// move and rotate the player
        moveV.Set(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); //set vector3 with wasd
        moveV = moveV.normalized * speed * movementModifier * Time.deltaTime; //multiply the normalized vector with speed and delta time
        transform.Translate(moveV); //translate cat obj based on moveV vector
		
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
	
    void OnCollisionStay()
    {
        isJumping = false;   
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