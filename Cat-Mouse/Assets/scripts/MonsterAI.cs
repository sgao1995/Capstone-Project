using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterAI : MonoBehaviour {
	// monster stats
	private float speed = 2.0f; //speed value
	public float HP = 100f;
	public float attackPower = 2f;
	private float attackCooldownDelay = 3f;
	private float attackCooldownTimer = 3f;
	
	// list of the players in the game
	private GameObject[] playersInGame;
    private GameObject[] catsInGame;
    private GameObject[] miceInGame;
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
	private float delayBetweenMovements = 200f;
	private List<string> modes = new List<string>();
	private string currentMode = "Sleeping";
	private Animator animator;
	
	// monster type
	public string monsterType;
	
	// Use this for initialization
	void Start () {
		monsterrb = GetComponent<Rigidbody>();
		delayTimer = delayBetweenMovements;
		modes.Add("Patrol");
		modes.Add("Attack");
		modes.Add("Sleeping");
		playersInGame= GameObject.FindGameObjectsWithTag("Player");
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		currentMode = "Attack";
	}
    public float getHealth()
    {
        return HP;
    }

    void takeDamage(float dmg)
    {
        transform.GetComponent<PhotonView>().RPC("changeHealth", PhotonTargets.AllBuffered, dmg);
		Debug.Log("take " + dmg + " damage");
		
		// change modes to attack
		if (currentMode != "Attack"){
			currentMode = "Attack";
			delayTimer = delayBetweenMovements;
			sprintTimer = 10f;
		}
    }
    // take damage
    [PunRPC]
    void changeHealth(float dmg)
    {
        HP -= dmg;
        // if hp goes below 0, monster dies
        if (HP <= 0)
        {
            if (this.gameObject != null)
            {
               transform.GetComponent<MonsterAI>().enabled = false;
               StartCoroutine(Death());
            }
        }
    }
   
	IEnumerator Death()
    {
		animator.Play("Death");
		yield return new WaitForSeconds(5);
		// remove monster from the game
        PhotonNetwork.Destroy(this.gameObject);
        
    }
	// turn left 2 degrees
	void TurnLeft(){
		transform.Rotate (transform.up, -2f, Space.World);
	}
	
	// turn right 2 degrees
	void TurnRight(){
		transform.Rotate (transform.up, 2f, Space.World);
	}
	
	// attack in front of monster
    void Attack()
    {
		Debug.Log("Monster Attack");
		animator.Play("Punch");
		DealDamage();
    }
	
	void DealDamage(){
		Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 1))
        {
            if (hitInfo.collider.name == "Cat(Clone)")
            {
                hitInfo.collider.transform.GetComponent<CatMovement>().SendMessage("TakeDamage", attackPower);
            }
            if (hitInfo.collider.name == "Mouse(Clone)")
            {
                hitInfo.collider.transform.GetComponent<MouseMovement>().SendMessage("TakeDamage", attackPower);
            }
        }
	}
	
    void FixedUpdate()
    {
		if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Punch")){
			if (currentMode == "Sleeping"){
				animator.Play("Idle");
			}
			else if (currentMode == "Patrol"){
				animator.Play("WalkForward");
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
				if (sprintTimer > 2f){
					sprintTimer -= Time.deltaTime;
					animator.Play("Run");
				}
				else{
					animator.Play("WalkForward");
				}
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
				
				// chance to attack, 10%
				if (attackCooldownTimer < 0){
					float attack = Random.Range(0, 1f);
					if (attack < 0.1f){
						attackCooldownTimer = attackCooldownDelay;
						Attack();
					}
				}

				// always look at player
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playersInGame[targettedPlayer].transform.position - transform.position), 3f*Time.deltaTime);
				
				//Debug.Log("chasing player " + targettedPlayer);
				
				// follow the player, turn to face direction of movement
				transform.rotation = Quaternion.LookRotation(transform.forward);
				// moves faster for first couple seconds
				transform.Translate(transform.forward * speed * (1f + sprintTimer/8f) * Time.deltaTime, Space.World);
				agent.SetDestination(playersInGame[targettedPlayer].transform.position);
			}
		}
				
		// switch modes if delay is up
		delayTimer -= Time.deltaTime;
		if (delayTimer < 0){
			currentMode = modes[(int)Random.Range(0, modes.Count - 0.01f)];
			if (currentMode == "Sleeping")
				delayTimer = delayBetweenMovements;
			else if (currentMode == "Patrol")
				// patrol takes twice as long
				delayTimer = delayBetweenMovements*2;
			else if (currentMode == "Attack")
				delayTimer = delayBetweenMovements;
			Debug.Log(currentMode);
		}
		
		// attack cooldown
		if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }
    void OnCollisionStay()
    {			
		
    }
}
