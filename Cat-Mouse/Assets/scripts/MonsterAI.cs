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
    private float speed = 2.0f; //speed value
    public float HP;
    public float attackPower;
    private float attackCooldownDelay;
    private float attackCooldownTimer;
    private float expDrop;
    private float attackChance;

    //animations:
    //  Animator animator;
    Vector3 animDirection = Vector3.zero;
    /*  static int hitState = Animator.StringToHash("Base Layer.GetHit");
      static int deathState = Animator.StringToHash("Base Layer.Death");
      static int attackState = Animator.StringToHash("Base Layer.Attack");
      static int bashState = Animator.StringToHash("Base Layer.Bash");
      static int stompState = Animator.StringToHash("Base Layer.Stomp");
      static int healState = Animator.StringToHash("Base Layer.Heal");
      static int idleState = Animator.StringToHash("Base Layer.Idle");
      static int walkState = Animator.StringToHash("Base Layer.WalkForward");
      private AnimatorStateInfo currentState;*/

    // list of the players in the game
    private List<GameObject> playersInGame = new List<GameObject>();
    public int targettedPlayer = 0;
    // patrol
    float patrolTimer = 5f;
    float turnTimer = 1.8f;
    // attack
    float retargetTimer = 5f;
    private float timeUntilRetarget = 1f;

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
    private float dist;

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

    // Use this for initialization
    void Start()
    {
        //        animator = this.transform.GetComponent<Animator>();
        //       animator.enabled = true;
        monsterrb = GetComponent<Rigidbody>();
        delayTimer = delayBetweenMovements;
        modes.Add("Patrol");
        modes.Add("Attack");
        modes.Add("Sleeping");

        agent = GetComponent<NavMeshAgent>();
        currentMode = "Sleeping";
        soundPlayer = GetComponent<AudioSource>();
    }
    // sets the player list with the players in the game
    private void setPlayerList()
    {
        playersInGame = new List<GameObject>();
        GameObject[] catArray = GameObject.FindGameObjectsWithTag("Cat");
        for (int i = 0; i < catArray.Length; i++)
        {
            playersInGame.Add(catArray[i]);
        }
        GameObject[] mouseArray = GameObject.FindGameObjectsWithTag("Mouse");
        for (int i = 0; i < mouseArray.Length; i++)
        {
            playersInGame.Add(mouseArray[i]);
        }
    }

    public void setMonsterType(string type)
    {
        this.monsterType = type;
        if (type == "Monster")
        {
            HP = 40f;
            attackPower = 3f;
            attackCooldownDelay = 3f;
            attackCooldownTimer = 3f;
            expDrop = 50f;
        }
        else if (type == "MonsterElite")
        {
            HP = 80f;
            attackPower = 7f;
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
    //when Monsters get stunned, this function will be called and will stop them from moving for 1 second
    void isStunned()
    {
        Debug.Log("STUNNED");
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Attack");
        WaitForAnimation(1f);
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
        retargetTimer = timeUntilRetarget;
        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 2, 1f);

        // change modes to attack
        if (currentMode != "Attack")
        {
            currentMode = "Attack";
            delayTimer = delayBetweenMovements;
            setPlayerList();
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
                StartCoroutine(Death());
                transform.GetComponent<MonsterAI>().enabled = false;
            }
        }
    }

    IEnumerator Death()
    {
        GetComponent<Animator>().SetBool("Death", true);
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
    // turn left 2 degrees
    void TurnLeft()
    {
        transform.Rotate(transform.up, -2f, Space.World);
    }

    // turn right 2 degrees
    void TurnRight()
    {
        transform.Rotate(transform.up, 2f, Space.World);
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
        stompCooldownTimer = 20f;
        //		animator.Play("Stomp");
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Stomp");
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
        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 6, 1f);
    }

    void DealDamage()
    {
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
            transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 3, 1f);
        }
        else
        {
            transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 4, 1f);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            currentPos = transform.position;
            if (currentPos == lastPos)
            {
                notmoving = true;
            }
            else
            {
                notmoving = false;
            }
            lastPos = currentPos;
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
                transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

            }
            else if (currentMode == "Attack")
            {

                GetComponent<Animator>().SetBool("WalkForward", true);

                if (!soundPlayer.isPlaying)
                {
                    transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                }
                // face forward unless distance is short, then look at player
                if (dist < 3f)
                {
                    transform.LookAt(playersInGame[targettedPlayer].transform);
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

        animDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (canMove)
        {
            if (currentMode == "Patrol")
            {
                // move back and forth
                patrolTimer -= Time.deltaTime;
                if (patrolTimer < 0)
                {
                    // turn
                    turnTimer -= Time.deltaTime;
                    TurnLeft();
                    if (turnTimer < 0)
                    {
                        turnTimer = 1.8f;
                        patrolTimer = 5f;
                    }
                }
            }
            else if (currentMode == "Attack")
            {
                dist = Vector3.Distance(this.transform.position, playersInGame[targettedPlayer].transform.position);
                // target a player upon mode attack
                if (retargetTimer < 0)
                {
                    retargetTimer = timeUntilRetarget;
                }
                // then retarget every 5 seconds
                if (retargetTimer == timeUntilRetarget)
                {
                    // find closest player
                    float closestDist = float.MaxValue;
                    for (int p = 0; p < playersInGame.Count; p++)
                    {
                        dist = Vector3.Distance(this.transform.position, playersInGame[p].transform.position);
                        if (dist < closestDist)
                        {
                            targettedPlayer = p;
                            agent.SetDestination(playersInGame[targettedPlayer].transform.position);
                        }
                    }
                }
                retargetTimer -= Time.deltaTime;

                // type check
                if (monsterType == "Monster")
                {
                    // chance to attack, 20%
                    if (attackCooldownTimer < 0)
                    {
                        // if close enough then attack
                        if (dist < 2f)
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
                        if (dist < 2f)
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
                        if (dist < 4f)
                        {
                            attackChance = Random.Range(0, 1f);
                            if (attackChance < 0.15f)
                            {
                                Attack();
                            }
                            // 10% chance to heal 
                            else if (healCooldownTimer < 0 && attackChance < 0.25f)
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
                        if (dist < 2f)
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
            currentMode = modes[Random.Range(0, modes.Count)];
            if (currentMode == "Sleeping")
            {
                delayTimer = delayBetweenMovements;
            }
            else if (currentMode == "Patrol")
            {
                // patrol takes twice as long
                delayTimer = delayBetweenMovements * 2;
            }
            else if (currentMode == "Attack")
            {
                delayTimer = delayBetweenMovements;
                setPlayerList();
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
    }
    //rpc for triggers
    [PunRPC]
    void SetTrigger(string a)
    {
        GetComponent<Animator>().SetTrigger(a);
    }

}
