using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterAI : MonoBehaviour {
	// monster stats
	private float speed = 2.0f; //speed value
	public float HP = 100f;
	public float attackPower = 1;
	private float attackCooldownDelay = 3f;
	private float attackCooldownTimer = 3f;
	
	// list of the players in the game
	private GameObject[] playersInGame; 
	
	// patrol
	float patrolTimer = 5f;
	float turnTimer = 1.8f;
	// attack
	float sprintTimer = 10f;
	float retargetTimer = 5f;
	private float timeUntilRetarget = 5f;
	
	// utility variables
	NavMeshAgent agent;
    public Rigidbody monsterrb;
	private float delayTimer = 0f;
	private float delayBetweenMovements = 20f;
	private List<string> modes = new List<string>();
	private string currentMode = "Attack";
	
	// Use this for initialization
	void Start () {
		monsterrb = GetComponent<Rigidbody>();
		delayTimer = delayBetweenMovements;
		modes.Add("Patrol");
		modes.Add("Attack");
		modes.Add("Sleeping");
		playersInGame= GameObject.FindGameObjectsWithTag("Player");
		agent = GetComponent<NavMeshAgent>();
	}
	
	// take damage
	public void takeDamage(float dmg){
		HP -= dmg;
		// if hp goes below 0, monster dies
		if (HP <= 0){
            Death();
			
		}
	}
	public void Death()
    {
        Destroy(this.gameObject);
        
    }
	// turn left 2 degrees
	void TurnLeft(){
		transform.Rotate (transform.up, -2f, Space.World);
	}
	
	// turn right 2 degrees
	void TurnRight(){
		transform.Rotate (transform.up, 2f, Space.World);
	}
	
    void FixedUpdate()
    {
		if (currentMode == "Sleeping"){
			// do nothing
		}
		else if (currentMode == "Patrol"){
			transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
			// move back and forth
			patrolTimer -= Time.deltaTime;
			if (patrolTimer < 0){
				// turn
				turnTimer -= Time.deltaTime;
				TurnLeft();
				if (turnTimer < 0){
					turnTimer = 1.8f;
					patrolTimer = 5f;
				}
			}
		}
		else if (currentMode == "Attack"){
			if (sprintTimer > 2f)
				sprintTimer -= Time.deltaTime;
			// target a player upon mode attack
			if (retargetTimer < 0){
				retargetTimer = timeUntilRetarget;
			}
			int targettedPlayer = 0;
			// then retarget every 5 seconds
			if (retargetTimer == timeUntilRetarget)
			{
				// find closest player
				float closestDist = float.MaxValue;
				for (int p = 0; p < playersInGame.Length; p++){
					float dist = Vector3.Distance(this.transform.position, playersInGame[p].transform.position);
					if (dist < closestDist){
						targettedPlayer = p;
					}
				}
			}		
			retargetTimer -= Time.deltaTime;
			

			// always look at player
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playersInGame[targettedPlayer].transform.position - transform.position), 3f*Time.deltaTime);
			
			//Debug.Log("chasing player " + targettedPlayer);
			
			// follow the player, turn to face direction of movement
			transform.rotation = Quaternion.LookRotation(transform.forward);
			// moves faster for first couple seconds
			transform.Translate(transform.forward * speed * (1f + sprintTimer/8f) * Time.deltaTime, Space.World);
			agent.SetDestination(playersInGame[targettedPlayer].transform.position);
		}
				
		// switch modes if delay is up
		delayTimer -= Time.deltaTime;
		if (delayTimer < 0){
			currentMode = "Attack";//modes[Random.Range(0, modes.Count - 1)];
			if (currentMode == "Sleeping")
				delayTimer = delayBetweenMovements;
			else if (currentMode == "Patrol")
				// patrol takes twice as long
				delayTimer = delayBetweenMovements*2;
			else if (currentMode == "Attack")
				delayTimer = delayBetweenMovements;
			Debug.Log(currentMode);
		}
    }
    void OnCollisionStay()
    {			
		
    }
}
