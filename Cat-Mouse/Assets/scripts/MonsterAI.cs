using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class MonsterAI : MonoBehaviour
{
    //flag to check if monster is moving
    private bool notmoving = true;
    private Vector3 currentPos;
    private Vector3 lastPos;
    // monster stats
    public float HP;
    public float attackPower;
    private float attackCooldownDelay;
    private float attackCooldownTimer;
    private float expDrop;
    private float attackChance;

    //animations:
    //  Animator animator;
    //Vector3 animDirection = Vector3.zero;
    /*  static int hitState = Animator.StringToHash("Base Layer.GetHit");
      static int deathState = Animator.StringToHash("Base Layer.Death");
      static int attackState = Animator.StringToHash("Base Layer.Attack");
      static int bashState = Animator.StringToHash("Base Layer.Bash");
      static int stompState = Animator.StringToHash("Base Layer.Stomp");
      static int healState = Animator.StringToHash("Base Layer.Heal");
      static int idleState = Animator.StringToHash("Base Layer.Idle");
      static int walkState = Animator.StringToHash("Base Layer.WalkForward");
      private AnimatorStateInfo currentState;*/

    // utility variables
    NavMeshAgent agent;
    public Rigidbody monsterrb;
    private float delayTimer = 0f;
    private float delayBetweenMovements = 20f;
    private List<string> modes = new List<string>();
    private string currentMode;
    private bool canMove = true;

    // monster type and skills
    public string monsterType;
    private float bashCooldownTimer = 30f;
    private float stompCooldownTimer = 20f;
    private float healCooldownTimer = 30f;

    public AudioClip footstepSound;
    public AudioClip attackMissSound;
    public AudioClip[] dealDamageSound;
    public AudioClip[] takeDamageSound;
    public AudioClip bashSound;
    public AudioClip healSound;
    public AudioClip stompSound;
    public AudioSource soundPlayer;

    private GameObject trappedBy;
    private bool trapped = false;
	
	private float lastTime;
	
	// keep track of the last player to hit this monster
	private GameObject targettedPlayer;

    // Use this for initialization
    void Start()
    {
        //        animator = this.transform.GetComponent<Animator>();
        //       animator.enabled = true;
        monsterrb = GetComponent<Rigidbody>();
        delayTimer = delayBetweenMovements;
		modes.Add("Sleeping");
        modes.Add("Patrol");
		modes.Add("Attack");

        agent = GetComponent<NavMeshAgent>();
        currentMode = "Sleeping";
        soundPlayer = GetComponent<AudioSource>();
    }


    public void setMonsterType(string type)
    {
        this.monsterType = type;
        if (type == "Monster")
        {
            HP = 40f;
            attackPower = 6f;
            attackCooldownDelay = 3f;
            attackCooldownTimer = 3f;
            expDrop = 50f;
        }
        else if (type == "MonsterElite")
        {
            HP = 80f;
            attackPower = 12f;
            attackCooldownDelay = 2f;
            attackCooldownTimer = 2f;
            expDrop = 100f;
        }
        else if (type == "Boss")
        {
            HP = 300f;
            attackPower = 20f;
            attackCooldownDelay = 4f;
            attackCooldownTimer = 4f;
            expDrop = 500f;
        }
        else if (type == "PuzzleRoomBoss")
        {
            HP = 200f;
            attackPower = 25f;
            attackCooldownDelay = 5f;
            attackCooldownTimer = 5f;
            expDrop = 250f;
        }
    }

    IEnumerator Asleep()//if is hit by a sleeping dart, is put to sleep for 5 seconds
    {
        canMove = false;
		transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Death");
		agent.SetDestination(transform.position);
        Debug.Log("can't move");
        yield return new WaitForSeconds(5);
        canMove = true;
		transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Idle");
        Debug.Log("can move");
    }
    [PunRPC]
    void PlayAnim(string a)
    {
        GetComponent<Animator>().Play(a);
    }
    [PunRPC]
    void playSound(int type, float t)
    {
        switch (type)
        {
            // monster footsteps
            case 0:
                soundPlayer.PlayOneShot(footstepSound, t);
                break;
            // take damage
            case 2:
                soundPlayer.PlayOneShot(takeDamageSound[Random.Range(0, takeDamageSound.Length)], t);
                break;
            // deal damage
            case 3:
                soundPlayer.PlayOneShot(dealDamageSound[Random.Range(0, dealDamageSound.Length)], t);
                break;
            // attack miss
            case 4:
                soundPlayer.PlayOneShot(attackMissSound, t);
                break;
            // bash
            case 5:
                soundPlayer.PlayOneShot(bashSound, t);
                break;
            // heal
            case 6:
                soundPlayer.PlayOneShot(healSound, t);
                break;
            // stomp
            case 7:
                soundPlayer.PlayOneShot(stompSound, t);
                break;
        }
    }


    public float getHealth()
    {
        return HP;
    }
    public float getExpDrop()
    {
        return expDrop;
    }

    // wait function
    public void WaitForAnimation(float seconds)
    {
        StartCoroutine(_wait(seconds));
    }
    IEnumerator _wait(float time)
    {
        canMove = false;
        transform.GetComponent<NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(time);
        transform.GetComponent<NavMeshAgent>().enabled = true;
        canMove = true;
    }
    //when Monsters get crippled, this function will be called
    [PunRPC]
    IEnumerator Crippled()
    {
        agent.speed = 1.4f;
        Debug.Log("CRIPPLED");
        yield return new WaitForSeconds(5);
        agent.speed = 2f;
    }

	[PunRPC]
	void TargetMe(int thisPlayerHitMe){
		targettedPlayer = PhotonView.Find(thisPlayerHitMe).gameObject;
		// change modes to attack
        if (currentMode != "Attack")
        {
            currentMode = "Attack";
            delayTimer = delayBetweenMovements;
        }
	}
	
    void takeDamage(float dmg)
    {
        if (HP - dmg > 0)
        {
            transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "GetHit");

            //  GetComponent<Animator>().SetTrigger("GetHit");

            WaitForAnimation(1f);
            //  if (animator.GetNextAnimatorStateInfo(0).IsName("GetHit"))
            //  {
            //    animator.SetBool("GetHit", false);
            //  }
        }
        transform.GetComponent<PhotonView>().RPC("changeHealth", PhotonTargets.AllBuffered, dmg);
        Debug.Log("take " + dmg + " damage");
        Debug.Log("hp is:" + HP);
        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 2, 1f);
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
                StartCoroutine(Death());
                transform.GetComponent<MonsterAI>().enabled = false;
            }
        }
    }

    IEnumerator Death()
    {
        GetComponent<Animator>().SetBool("Death", true);
		canMove = false;
        transform.GetComponent<NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(5);
        // notify game manager of a monster death
        if (this.monsterType == "PuzzleRoomBoss")
        {
            // notify game manager to spawn the key
            GameManager gm = GameObject.Find("SCRIPTS").GetComponent<GameManager>();
            gm.ClearedRoomSoSpawnKey(5);
        }
        // remove monster from the game
        PhotonNetwork.Destroy(this.gameObject);

    }

    // attack in front of monster
    void Attack()
    {
        attackCooldownTimer = attackCooldownDelay;
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Attack");
        //GetComponent<Animator>().SetTrigger("Attack");


        WaitForAnimation(1f);
        // if (animator.GetNextAnimatorStateInfo(0).IsName("Attack"))
        //  {
        //       NetworkAnimator.SetTrigger("Attack");
        //  }
        DealDamage();
    }
    // special ranged attack for the boss
    void RangedAttack()
    {
        // doesnt cause a hitbox, just has a particle effect that does damage on its own
        attackCooldownTimer = attackCooldownDelay;
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Attack");
        WaitForAnimation(1f);
        // so we dont call deal damage, just spawn a particle effect
        Quaternion bossAttackRot = Quaternion.LookRotation(transform.forward);
        Vector3 bossAttackPos = new Vector3(transform.position.x, 1.2f, transform.position.z) + (transform.forward * 0.5f);
        PhotonNetwork.InstantiateSceneObject("BossAttack", bossAttackPos, bossAttackRot, 0);
    }

    // cast a bash skill in front of monster
    void Bash()
    {
        bashCooldownTimer = 5f;
        //		animator.Play("Bash");
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Bash");

        // GetComponent<Animator>().SetTrigger("Bash");
        WaitForAnimation(1f);
        DealDamage();
        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 5, 1f);

    }
    // cast stomp in aoe around monster
    void Stomp()
    {
        Debug.Log("stomp");
        stompCooldownTimer = 20f;
        //		animator.Play("Stomp");
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Stomp");
        // display cracks on ground
        Quaternion crackRot = Quaternion.Euler(-90, 0, 0);
        Vector3 crackPos = new Vector3(transform.position.x, 0.05f, transform.position.z);
        PhotonNetwork.InstantiateSceneObject("SplitEarth", crackPos, crackRot, 0);
        WaitForAnimation(1.5f);
        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 7, 1f);
    }
    // cast heal on self
    void Heal()
    {
        healCooldownTimer = 30f;
        //		animator.Play("Heal");
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Heal");

        //  GetComponent<Animator>().SetTrigger("Heal");
        WaitForAnimation(3f);
		HP += 40f;
        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 6, 1f);
    }

    void DealDamage()
    {
        RaycastHit hitInfo;
        // default hit box size, change it for bigger monsters
        float attackHitboxSize = 0.2f;
        if (monsterType == "Monster")
        {
            attackHitboxSize = 0.2f;
        }
        else if (monsterType == "MonsterElite")
        {
            attackHitboxSize = 0.25f;
        }
        else if (monsterType == "PuzzleRoomBoss")
        {
            attackHitboxSize = 0.4f;
        }

        if (Physics.SphereCast(transform.position, attackHitboxSize, transform.forward, out hitInfo, 1))
        {
            if (hitInfo.collider.name == "Cat(Clone)")
            {
                hitInfo.collider.transform.GetComponent<CatMovement>().SendMessage("TakeDamage", attackPower);
            }
            if (hitInfo.collider.name == "Mouse(Clone)")
            {
                if (hitInfo.collider.transform.GetComponent<MouseMovement>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MouseMovement>().getHealth() - attackPower <= 0)
                {
                    transform.GetComponent<PhotonView>().RPC("setMouseDeath", PhotonTargets.AllBuffered);
                }
                hitInfo.collider.transform.GetComponent<MouseMovement>().SendMessage("TakeDamage", attackPower);
            }
            transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 3, 1f);
        }
        else
        {
            transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 4, 1f);
        }
    }
    [PunRPC]
    void setMouseDeath()
    {
        GameObject.Find("GUI").GetComponent<WinScript>().setMouseDeaths();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (trapped)
            {
                transform.position = trappedBy.transform.position;
            }

            if (currentMode == "Sleeping")
            {

                GetComponent<Animator>().SetBool("WalkForward", false);

            }
            else if (currentMode == "Patrol")
            {

                if (!notmoving)
                {
                    GetComponent<Animator>().SetBool("WalkForward", true);
                }
                else
                {
                    GetComponent<Animator>().SetBool("WalkForward", false);
                }

                if (!soundPlayer.isPlaying)
                {
                    transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                }
            }
            else if (currentMode == "Attack")
            {
                if (!notmoving)
                {
                    GetComponent<Animator>().SetBool("WalkForward", true);
                }
                else
                {
                    GetComponent<Animator>().SetBool("WalkForward", false);
                }

                if (!soundPlayer.isPlaying)
                {
                    transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                }
                // face forward unless distance is short, then look at player
                if (Vector3.Distance(this.transform.position, targettedPlayer.transform.position) < 3f)
                {
                    transform.LookAt(targettedPlayer.transform);
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(transform.forward);
                }
            }
        }
    }

    // non motion based 
    void Update()
    {
		// check if monster is moving based on position
		currentPos = transform.position;
		if (Time.time - lastTime > 0.01f && Vector3.Distance(currentPos, lastPos) <= 0.01f)
		{
			notmoving = true;
		}
		else
		{
			notmoving = false;
		}
		lastPos = currentPos;
		lastTime = Time.time;
		
       // animDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (canMove)
        {
            if (currentMode == "Attack")
            {
				Vector3 offset;
				if (monsterType == "Boss")
				{
					offset = 1.5f * Vector3.Normalize(this.transform.position - targettedPlayer.transform.position);
				}
				else
				{
					offset = Vector3.Normalize(this.transform.position - targettedPlayer.transform.position) / 3f;
				}
				agent.SetDestination(targettedPlayer.transform.position + offset);
                    
				Debug.Log("targetting player " + targettedPlayer + ", distance of " + Vector3.Distance(this.transform.position, targettedPlayer.transform.position));
					
                // type check
                if (monsterType == "Monster")
                {
                    // chance to attack, 20%
                    if (attackCooldownTimer < 0)
                    {
                        // if close enough then attack
                        if (Vector3.Distance(this.transform.position, targettedPlayer.transform.position) < 1f)
                        {
                            attackChance = Random.Range(0, 1f);
                            if (attackChance < 0.2f)
                            {
                                Attack();
                            }
                        }
                    }
                }
                else if (monsterType == "MonsterElite")
                {
                    // chance to attack, 15%
                    if (attackCooldownTimer < 0)
                    {
                        // if close enough then attack
                        if (Vector3.Distance(this.transform.position, targettedPlayer.transform.position) < 1f)
                        {
                            attackChance = Random.Range(0, 1f);
                            if (attackChance < 0.15f)
                            {
                                Attack();
                            }
                            // 5% chance to bash
                            else if (bashCooldownTimer < 0 && attackChance < 0.2f)
                            {
                                Bash();
                            }
                        }
                    }
                }
                else if (monsterType == "Boss")
                {
                    // chance to attack, 15%
                    if (attackCooldownTimer < 0)
                    {
                        // if close enough then attack
                        if (Vector3.Distance(this.transform.position, targettedPlayer.transform.position) < 4f)
                        {
                            attackChance = Random.Range(0, 1f);
                            if (attackChance < 0.15f)
                            {
                                RangedAttack();
                            }
                            // 10% chance to heal 
                            else if (healCooldownTimer < 0 && attackChance < 0.25f && HP <= 200f)
                            {
                                Heal();
                            }
                        }
                    }
                }
                else if (monsterType == "PuzzleRoomBoss")
                {
                    // chance to attack, 10%
                    if (attackCooldownTimer < 0)
                    {
                        // if close enough then attack
                        if (Vector3.Distance(this.transform.position, targettedPlayer.transform.position) < 1.5f)
                        {
                            attackChance = Random.Range(0, 1f);
                            if (attackChance < 0.1f)
                            {
                                Attack();
                            }
                            // 10% chance to stomp
                            else if (stompCooldownTimer < 0 && attackChance < 0.2f)
                            {
                                Stomp();
                            }
                        }
                    }
                }
            }
        }
        // switch modes if delay is up
        delayTimer -= Time.deltaTime;
        if (delayTimer < 0)
        {
			// monsters can only switch between sleep and patrol
            currentMode = modes[Random.Range(0,2)];
			Debug.Log(currentMode);
            if (currentMode == "Sleeping")
            {
                delayTimer = delayBetweenMovements;
            }
            else if (currentMode == "Patrol")
            {
                // patrol takes twice as long
                delayTimer = delayBetweenMovements * 2;
                // upon change to patrol, pick a random location in the maze as target
                int mazeSize = GameObject.Find("Maze(Clone)").GetComponent<Maze>().size.x;
                // need to add 0.5 or else it will be on an edge
                Vector3 targetLocation = new Vector3(0.5f + Random.Range(-mazeSize, mazeSize), 0.5f, 0.5f + Random.Range(-mazeSize, mazeSize));
                agent.SetDestination(targetLocation);
            }
        }

        // cooldowns
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        if (bashCooldownTimer > 0)
        {
            bashCooldownTimer -= Time.deltaTime;
        }
        if (stompCooldownTimer > 0)
        {
            stompCooldownTimer -= Time.deltaTime;
        }
        if (healCooldownTimer > 0)
        {
            healCooldownTimer -= Time.deltaTime;
        }
    }

    /* Stun the monster */
    public void denyMonsterMovement()
    {
        canMove = false;
    }

    public void allowMonsterMovement()
    {
        canMove = true;
    }

    public void releaseFromTrap()
    {
        trapped = false;
        trappedBy = null;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "SteelTrap")
        {
            trappedBy = obj.gameObject;
            trapped = true;
            // trap just stops movement for 5 seconds, can use the wait for animation function for this
            WaitForAnimation(5f);
            takeDamage(5f);
            obj.GetComponent<SteelTrap>().activate(this.gameObject);
        }
		// if gets hit by a dart
        if (obj.tag == "Dart")
        {
            Debug.Log("hit by dart");
			PhotonNetwork.Destroy(obj.gameObject);
            StartCoroutine(Asleep());
        }
    }
    //rpc for triggers
    [PunRPC]
    void SetTrigger(string a)
    {
        GetComponent<Animator>().SetTrigger(a);
    }

}
