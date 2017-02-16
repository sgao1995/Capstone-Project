using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterAI : MonoBehaviour {
	// monster stats
	private float speed = 2.0f; //speed value
	public float HP;
	public float attackPower;
	private float attackCooldownDelay;
	private float attackCooldownTimer;
	private float expDrop;
	
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
	public void setMonsterType(string type){
		this.monsterType = type;
		if (type == "Monster"){
			HP = 40f;
			attackPower = 3f;
			attackCooldownDelay = 3f;
			attackCooldownTimer = 3f;
			expDrop = 50f;
		}
		else if (type == "MonsterElite"){
			HP = 80f;
			attackPower = 7f;
			attackCooldownDelay = 2f;
			attackCooldownTimer = 2f;
			expDrop = 100f;
		}
		else if (type == "Boss"){
			HP = 300f;
			attackPower = 20f;
			attackCooldownDelay = 4f;
			attackCooldownTimer = 4f;
			expDrop = 500f;
		}
		else if (type == "PuzzleRoomBoss"){
			HP = 200f;
			attackPower = 25f;
			attackCooldownDelay = 5f;
			attackCooldownTimer = 5f;
			expDrop = 250f;
		}
	}
	
    public float getHealth()
    {
        return HP;
    }
	public float getExpDrop(){
		return expDrop;
	}

    void takeDamage(float dmg)
    {
        transform.GetComponent<PhotonView>().RPC("changeHealth", PhotonTargets.AllBuffered, dmg);
		Debug.Log("take " + dmg + " damage");
		
		// change modes to attack
		if (currentMode != "Attack"){
			currentMode = "Attack";
			delayTimer = delayBetweenMovements;
			sprintTimer = 7f;
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
		animator.Play("Attack");
		DealDamage();
    }
	
	void DealDamage(){
		RaycastHit hitInfo;
		
        if (Physics.SphereCast(transform.position, 0.2f, transform.forward, out hitInfo, 1))
        {
            if (hitInfo.collider.name == "Cat(Clone)")
            {
                if (hitInfo.collider.transform.GetComponent<CatMovement>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<CatMovement>().getHealth() - attackPower <= 0)
                {
                    GameObject.Find("WinObj").GetComponent<WinScript>().setCatDeaths();
                }
                hitInfo.collider.transform.GetComponent<CatMovement>().SendMessage("TakeDamage", attackPower);
            }
            if (hitInfo.collider.name == "Mouse(Clone)")
            {
                if (hitInfo.collider.transform.GetComponent<MouseMovement>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MouseMovement>().getHealth() - attackPower <= 0)
                {
                    GameObject.Find("WinObj").GetComponent<WinScript>().setMouseDeaths();
                }
                hitInfo.collider.transform.GetComponent<MouseMovement>().SendMessage("TakeDamage", attackPower);
            }
        }
	}
	
    void FixedUpdate()
    {
		if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
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
