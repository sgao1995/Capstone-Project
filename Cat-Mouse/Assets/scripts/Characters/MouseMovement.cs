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
    public float currentHealth=100;
    private float maxHealth;
    private int skillPoints;
    private int ultimateSkillPoints;
    private List<int> learnedSkills;
    private float damage = 10f;
    // movement speed
    private float movementModifier = 1f;
    private float movementModifier2 = 1f; //this is for the Hunted skill, so it will be additional movement speed if active.
    private float movementModifierTimer = 10f;
	// invis duration
	private float invisDuration = 0f;

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
	private bool canOpenExit = false;
	private bool canMove = true;
	private float flareCooldownTimer = 0f; // cooldown of flare usage
	
	// keys and puzzle pieces on hand
	public int numKeysHeld = 0;

    // skills
    private int numSkillSlotsMaximum = 4;  // Sets the maximum number of Skill Slots for the Mouse
    private float[] skillCooldownTimers = new float[4]; // the cooldown timer
    private float[] skillCooldowns = new float[4]; // the max cooldown
    private float healingAmt = 4f; //Needed for Bandage skill, if skill is active, explorer will heal 4 hp every second
    private bool isHuntedActive = false; //Needed for Hunted skill, if true, will muffle footstep sound
    private bool isCrippled = false; //Needed for Cripple skill

    /* Vitality System attribute parameters */
    private float[] vitalLevelHP = {50, 65, 80, 110};  // Health Points of Mouse per Level
    private float[] vitalLevelEXP = {320, 640, 1280, 2500};  // Experience Points Mouse per Level

    /* HUD state */
    public Vitality mouseVitality;  // Vitality System component
    public Skill mouseSkill;  // Skill System component
    public Text interactText;
	public bool miniMenuShowing = false;
	private GameObject miniMenu;
    private GameObject Alert;
    private Text ObjectiveMsg;
    /* Sound effects */
    public AudioClip footstepSound;
	public AudioClip jumpSound;
	public AudioClip attackMissSound;
	public AudioClip[] dealDamageSound;
	public AudioClip[] takeDamageSound;
	public AudioClip smokescreenSound;
	public AudioSource soundPlayer;

    //hit collider for attacking
    private Collider[] hitCollider;

    //list of spawns
    SpawnM[] sm;
    private float spawnDelay=10;

    void Start()
    {
        //for spawns:
        sm = GameObject.FindObjectsOfType<SpawnM>();

        mouserb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; //cursor is gone from screen

        LevelUp();  // Starts at the first level
        animator = GetComponent<Animator>();

        /* Initialises list of Learned Skills for the Cat */
        learnedSkills = new List<int>();

        /*  Finds and initialises the Vitality System component */
        GameObject mouseVitalityGameObject = GameObject.Find("Vitality");
        mouseVitality = mouseVitalityGameObject.GetComponent<Vitality>();

        /*  Finds and initialises the Skill System component */
        GameObject mouseSkillGameObject = GameObject.Find("Skill");
        mouseSkill = mouseSkillGameObject.GetComponent<Skill>();

        GameObject interactiveText = GameObject.Find("Text");
		interactText = interactiveText.GetComponent<Text>();
		interactText.text = "";
		miniMenu = GameObject.Find("MiniMenu");
		// need to disable the minimenu to begin with
		miniMenu.SetActive(false);
        Alert = GameObject.Find("Alert");
        Alert.SetActive(false);
        ObjectiveMsg = GameObject.Find("Objective").GetComponent<Text>();
        StartCoroutine(ObjectiveMessage());

        soundPlayer = GetComponent<AudioSource>();
    }
    IEnumerator ObjectiveMessage()
    {
        if (GameObject.Find("Objective").GetComponent<Text>())
        {
            Debug.Log("messagedisplayed");
        }
        ObjectiveMsg.text = "Gather Puzzle Pieces From Chests By Solving Puzzles And Find The Exit";
        yield return new WaitForSeconds(5);
        ObjectiveMsg.text = "";
    }
    
    void tagTeam() //Explorer Skill (passive): Increase movement speed by 50% when near another Explorer
    {
        hitCollider = Physics.OverlapSphere(this.transform.position, 5);
        foreach (Collider C in hitCollider)
        {
            if (C.GetComponent<Collider>().transform.root != this.transform && C.GetComponent<Collider>().tag == "Mouse")
            {
                Debug.Log("hit");
                movementModifier = 1.5f;

            }
        }
    }
    void TreasureHunter()//Explorer Skill (passive): Alert shows up when player is near chest or keys
    {
        hitCollider = Physics.OverlapSphere(this.transform.position, 10);
        foreach (Collider C in hitCollider)
        {
            if (C.GetComponent<Collider>().transform.root != this.transform && (C.GetComponent<Collider>().tag == "Key" || C.GetComponent<Collider>().tag == "Key(Clone)"))
            {
                Debug.Log("Key Detected");
                interactText.text = "A key is nearby...";
            }else if (C.GetComponent<Collider>().transform.root != this.transform && (C.GetComponent<Collider>().tag == "Chest" || C.GetComponent<Collider>().tag == "Chest(Clone)"))
            {
                Debug.Log("Chest Detected");
                interactText.text = "A treasure chest is nearby...";
            }
        }
    }
    void ProblemSolver()//Explorer Skill (passive): Alert shows up when player is near puzzle rooms
    {
        hitCollider = Physics.OverlapSphere(this.transform.position, 10);
        foreach (Collider C in hitCollider)
        {
            if (C.GetComponent<Collider>().transform.root != this.transform && (C.GetComponent<Collider>().tag == "PuzzleRoomBoss" || C.GetComponent<Collider>().tag == "Spike(Clone)" || C.GetComponent<Collider>().tag == "IcyMist(Clone)" || C.GetComponent<Collider>().tag == "Mine(Clone)" || C.GetComponent<Collider>().tag == "FireyMist(Clone)"))
            {
                Debug.Log("Puzzle Detected");
                interactText.text = "A puzzle room is nearby...";
            }
        }
    }
    IEnumerator Bandage()//Explorer Skill (active): Heals player 40 hp over 10 seconds
    {
        float time = 10;
        if (currentHealth < maxHealth)
        {
            while (time > 0)
            {
                currentHealth += healingAmt;
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
                time = time - 1;
                yield return new WaitForSeconds(1);
            }
        }
    }
    void HiddenPassage()//Explorer Skill (active): If the user hits a wall, the wall will slide down and a new path can be taken
    {
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Attack3Trigger");
        RaycastHit hitInfo;

        if (Physics.SphereCast(transform.position, 0.2f, transform.forward, out hitInfo, 1))
        {
            Debug.Log("We hit: " + hitInfo.collider.name);
            if (hitInfo.collider.tag == "Wall")
            {
                PhotonNetwork.Destroy(hitInfo.collider.gameObject);
            }
        }
        else
        {
            transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 4, 1f);
        }
    }
    [PunRPC]
    void DestroyWall(GameObject obj)
    {
        PhotonNetwork.Destroy(obj);
    }
    void Disengage()//Explorer Skill (active): Player will jump backwards a certain amount
    {
        canMove = false;
        mouserb.velocity = new Vector3(0,0,0);
        mouserb.angularVelocity = new Vector3(0, 0, 0);
        isGrounded = false;
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "JumpTrigger");
        transform.GetComponent<PhotonView>().RPC("SetInteger", PhotonTargets.All, "Jumping", 1);
        mouserb.AddForce(new Vector3(0, 120f, 0) + (-transform.forward * 300f));
        canMove = true;
    }
    void Cripple()//Explorer Skill (active): Attack which cripples an enemy (slows them down by 30%)
    {
        transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);
        attackCooldownTimer = attackCooldownDelay;
        WaitForAnimation(0.7f);
        isCrippled = true;
        StartCoroutine(Attack());
        
    }
    IEnumerator Hunted()//Explorer Skill (active): For 6 seconds become invisible instantly, increase movement speed by 50%, and footsteps are silent 
    {
        transform.GetComponent<PhotonView>().RPC("Invis", PhotonTargets.AllBuffered, 0.2f, 1f);
        isHuntedActive = true;
        movementModifier2 = 1.5f;
        yield return new WaitForSeconds(6);
        transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);
        isHuntedActive = false;
        movementModifier2 = 1f;
    }
    [PunRPC]
    IEnumerator SleepDart()//Explorer Skill (active): shoot a dart which stuns for 5 seconds (enemies will be able to move again if damaged)
    {
        yield return new WaitForSeconds(0.3f);
        Camera mouseCam = transform.Find("MouseCam").GetComponent<Camera>();
        Quaternion dartRot = Quaternion.Euler(0, 0, 0);
        Vector3 dartPos = transform.localPosition;
        Vector3 PosMod = new Vector3(0.5f, 1, 0);
        Ray ray = mouseCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Quaternion rayRot = Quaternion.LookRotation(ray.direction, Vector3.up);
        GameObject dart = PhotonNetwork.Instantiate("dart", dartPos+PosMod, rayRot, 0);
        dart.GetComponent<Rigidbody>().AddForce(transform.forward * 25f);
    }
    void LateUpdate()
    {
        tagTeam();

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
        attackCooldownDelay = 1.1f;
        currentHealth = maxHealth;

        /* Sets Character Maximum Health for new Level */
        this.maxHealth = this.vitalLevelHP[this.level - 1];
        this.currentHealth = this.maxHealth;

        /* Sets Character Maximum Experience for new level */
        this.maxEXP = this.vitalLevelEXP[this.level - 1];
        this.currentEXP = 0;
    }
    //when Explorers get stunned, this function will be called and will stop them from moving for 1 second
    void isStunned()
    {
        Debug.Log("STUNNED");
    }
    // wait function
    public void WaitForAnimation(float seconds){
		StartCoroutine(_wait(seconds));
	}
	IEnumerator _wait(float time){
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}
	
    // execute a skill (not jump or attack)
    public void useSkill(int skillCode)
    {
        switch (skillCode)
        {
            // skills 1 and 2 are passive
            case 3:
                Debug.Log("use smokescreen"); //SmokeScreen, Explorer Skill (active): Throw down smokescreen, and become invisible for 5 seconds
                // placeholder skill for a smoke screen
                //	animator.Play("Throw");
                transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Throw");
                WaitForAnimation(0.7f);
                transform.GetComponent<PhotonView>().RPC("cloak", PhotonTargets.AllBuffered);
				transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 5, 1f);

                break;
            case 4:
                Debug.Log("use 4");
                transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);

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
				transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 6, 1f);
                break;
        }
    }
    [PunRPC]
    void playSound(int type, float t)
    {
		switch(type){
			case 0:
                if (isHuntedActive)
                {
                    soundPlayer.PlayOneShot(footstepSound, 0.0f);
                }
                else
                {
                    soundPlayer.PlayOneShot(footstepSound, t);
                }
                break;
            case 1:
				soundPlayer.PlayOneShot(jumpSound, t);
				break;
			// take damage
			case 2:
				soundPlayer.PlayOneShot(takeDamageSound[Random.Range(0, takeDamageSound.Length)], t);
				break;
			// deal damage
			case 3:
				soundPlayer.PlayOneShot(dealDamageSound[Random.Range(0, dealDamageSound.Length)], t);
				break;
			case 4:
				soundPlayer.PlayOneShot(attackMissSound, t);
				break;
			// smokescreen
			case 5:
				soundPlayer.PlayOneShot(smokescreenSound, t);
				break;
			case 6:
				break;
		}
    }
    //used https://docs.unity3d.com/Manual/Coroutines.html as resource for coroutines and invisiblity
    [PunRPC]
    IEnumerator cloak()
    {	                
		yield return new WaitForSeconds(0.3f);
		Quaternion smokeRot = Quaternion.Euler(-90, 0, 0);
		Vector3 smokePos = new Vector3(transform.position.x, 0.5f, transform.position.z) + transform.forward;
		GameObject smokeScreen = (GameObject)PhotonNetwork.Instantiate("Smoke", smokePos, smokeRot, 0);
        transform.GetComponent<PhotonView>().RPC("Invis", PhotonTargets.AllBuffered, 0.2f, 1f);
        yield return new WaitForSeconds(5f);
        transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);
    }
    [PunRPC]
    void uncloak()
    {
        StartCoroutine(Invis(1.0f, 1f));
    }
	[PunRPC]
    IEnumerator Invis(float val, float time)
    {
        for (float f = 1f; f >= 0; f -= Time.deltaTime/time)
        {
            Color mouseColor = transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().material.color;
            mouseColor.a = Mathf.Lerp(transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().material.color.a, val, f); //lerp to make it look smoother
            transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().material.color = mouseColor;
            yield return null;
        }
    }
	void Update(){
        //jump function
        if (isGrounded && !onSpikes)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump();
            }
        }

        /* Updates the Character attributes and HUD state for the current player */
        hitCollider = Physics.OverlapSphere(this.transform.position, 10);
        //temporary for SleepDart
        if (Input.GetKeyDown(KeyCode.V))
        {
            transform.GetComponent<PhotonView>().RPC("SleepDart", PhotonTargets.AllBuffered);
        }
        //temporary for problemsolver
        foreach (Collider C in hitCollider)
        {
            if (C.GetComponent<Collider>().transform.root != this.transform && (C.GetComponent<Collider>().tag == "PuzzleRoomBoss" || C.GetComponent<Collider>().tag == "Spike(Clone)" || C.GetComponent<Collider>().tag == "PuzzleRoom" || C.GetComponent<Collider>().tag == "Mine(Clone)" ))
            {
                Debug.Log("Puzzle Detected");
                interactText.text = "A puzzle room is nearby...";
            }
        }
        //temporary for cripple skill
        if (Input.GetKeyDown(KeyCode.R))
        {
            Cripple();
        }
        //temporary for hunted skill
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Hunted());
        }
        //temporary for hidden passage skill
        if (Input.GetKeyDown(KeyCode.X))
        {
            HiddenPassage();
        }
        //temporary for recoup skill
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth = currentHealth - 80;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Bandage());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Disengage();
        }
        if (GetComponent<PhotonView>().isMine)
        {
            /* If the Maximum Experience Points for the current Level is reached, Level Up the Character */
            if (this.currentEXP >= this.maxEXP)
            {
                this.LevelUp();
            }

            /* Updates the Vitality System states */

            /* Updates the Level attributes */
            mouseVitality.setCurrentLevel(this.level);

            /* Updates the Health Points attributes of the Mouse */
            mouseVitality.setMaxHealthPoints(this.maxHealth); // Updates the Maximum Health Points
            mouseVitality.setCurrentHealthPoints(this.currentHealth);  // Updates the Current Health Points

            /* Updates the Experience Points attributes */
            mouseVitality.setMaximumExperiencePoints(this.maxEXP);
            mouseVitality.setCurrentExperiencePoints(this.currentEXP);

            /* Updates the Character Type attribute as a Mouse */
            mouseVitality.setCharacterType("mouse");

            /* Updates the number of Skill System states */

            /* Updates the total number of Skill Slots */
            mouseSkill.setMaxSkillSlots(numSkillSlotsMaximum);

            /* Updates the Skill assigned to each Skill Slot */
            mouseSkill.setSlotAssign(learnedSkills);
        }

        // status effects
        if (onLava)
        {
            TakeDamage(0.2f);
        }
        if (onSpikes)
        {
            TakeDamage(0.2f);
        }
		
		// left click
        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0 && !Input.GetKey(KeyCode.Escape) && !miniMenuShowing)
        {
			transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);
			attackCooldownTimer = attackCooldownDelay;
			WaitForAnimation(0.7f);
			StartCoroutine(Attack());
        }
		// right click brings up mini menu
		if (Input.GetMouseButtonDown(1)){
			if (miniMenuShowing == false){
				// show the cursor
				Cursor.lockState = CursorLockMode.None;
				miniMenu.SetActive(true);
				miniMenuShowing = true;	
			}
			else{
				Cursor.lockState = CursorLockMode.Locked;
				miniMenu.SetActive(false);
				miniMenuShowing = false;
			}
		}

        // skills
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.useSkill(this.mouseSkill.useSkillSlot(1));  // Uses the 1st Skill Slot of the Explorer Character
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.useSkill(this.mouseSkill.useSkillSlot(2));  // Uses the 2nd Skill Slot
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.useSkill(this.mouseSkill.useSkillSlot(3));  // Uses the 3rd Skill Slot
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.useSkill(this.mouseSkill.useSkillSlot(4));  // Uses the 4th Skill Slot
        }

        // interactions
		if (canToggleDoor || canTakeKey || canOpenChest || canTakePuzzlePiece || canOpenChest || canOpenExit){
			if (Input.GetKeyDown(KeyCode.E)){
				InteractWithObject();
			}
		}
		if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = new Vector3(22, 0, 25);
        }
        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None; //if we press esc, cursor appears on screen
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
		if (flareCooldownTimer > 0f){
			flareCooldownTimer -= Time.deltaTime;
		}
		if (invisDuration > 0f){
			invisDuration -= Time.deltaTime;
			if (invisDuration <= 0){
				transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);
			}
		}
	}

    void FixedUpdate()
    {
		if (canMove)
        {	
			if (onIce){
				moveV = moveV.normalized * speed * movementModifier *movementModifier2* Time.deltaTime * 0.1f;
			}
			else{
				moveV = moveV.normalized * speed * movementModifier *movementModifier2* Time.deltaTime;
			}
            transform.Translate(moveV);

			// movement control
			if (isGrounded && !onSpikes)
			{
				moveV = new Vector3(0, 0, 0);
				soundPlayer.pitch = Random.Range(0.9f, 1.1f);
				
				if (Input.GetKey(KeyCode.A))
				{
				//	animator.Play("MoveLeft");
                    transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "MoveLeft");
                    // play sound effect
                    if (!soundPlayer.isPlaying){
                        //soundPlayer.PlayOneShot(footstepSound, 1f);
                        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                    }

					if (onIce)
						mouserb.AddRelativeForce(Vector3.left*0.2f, ForceMode.Impulse);
					else{
						moveV = new Vector3(-1, 0, moveV.z);
					}
				}
				if (Input.GetKey(KeyCode.D))
				{
			//		animator.Play("MoveRight");
                    transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "MoveRight");
                    // play sound effect
                    if (!soundPlayer.isPlaying){
                        //soundPlayer.PlayOneShot(footstepSound, 1f);
                        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                    }
					
					if (onIce)
						mouserb.AddRelativeForce(Vector3.right*0.2f, ForceMode.Impulse);
					else{
						moveV = new Vector3(1, 0, moveV.z);
					}
				}
				if (Input.GetKey(KeyCode.W))
				{
					// play animation
					if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
					//	animator.Play("MoveForward");
                        transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "MoveForward");

                    // play sound effect
                    if (!soundPlayer.isPlaying){
						//soundPlayer.PlayOneShot(footstepSound, 1f);
                        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                    }
					
					if (onIce)
						mouserb.AddRelativeForce(Vector3.forward*0.2f, ForceMode.Impulse);
					else{
						moveV = new Vector3(moveV.x, 0, 1);
					}
				}
				if (Input.GetKey(KeyCode.S))
				{
					if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
						//animator.Play("MoveBackward");
                        transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "MoveBackward");
                    // play sound effect
                    if (!soundPlayer.isPlaying){
                        //soundPlayer.PlayOneShot(footstepSound, 1f);
                        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                    }
					
					if (onIce)
						mouserb.AddRelativeForce(Vector3.back*0.2f, ForceMode.Impulse);
					else{
						moveV = new Vector3(moveV.x, 0, -1);
					}
				}
				
			}
		}
    }
    void jump()
    {
        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 1, 1f);
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "JumpTrigger");
        transform.GetComponent<PhotonView>().RPC("SetInteger", PhotonTargets.All, "Jumping", 1);
        mouserb.AddForce(new Vector3(0, jumpForce, 0));
        isGrounded = false;
        Debug.Log("Jumped");
    }

    // take a certain amount of damage
    public void TakeDamage(float amt)
    {
		transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);
        //animator.Play("GetHit");
       // transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "GetHit");
        //WaitForAnimation(0.5f);
        transform.GetComponent<PhotonView>().RPC("changeHealth", PhotonTargets.AllBuffered, amt);
		transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 2, 1f);
        Debug.Log("Took Damage");
    }
    [PunRPC]
    void changeHealth(float dmg)
    {
		if (alive){
			currentHealth -= dmg;
			if (currentHealth <= 0)
			{
                mouseVitality.setCurrentHealthPoints(0);
                Death();
			}
		}
    }

    void Death()
    {
        StopCoroutine(Bandage());
        Debug.Log("player died");
        alive = false;
        GetComponent<MouseMovement>().enabled = false;
        CamMovement cam = gameObject.GetComponentInChildren<CamMovement>();
        cam.enabled = false;
        transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Unarmed-Death1");
        WaitForAnimation(5f);
        transform.GetComponent<PhotonView>().RPC("respawn", PhotonTargets.AllBuffered);

    }

    [PunRPC]
	void HideMiniMenu(){
		Cursor.lockState = CursorLockMode.Locked;
		miniMenu.SetActive(false);
		miniMenuShowing = false;
	}
	
	[PunRPC]
	void CastFlare(int color){
		if (flareCooldownTimer <= 0){
			WaitForAnimation(1.5f);
			StartCoroutine(Flare(color));
		}
		else{
			interactText.text = (int)flareCooldownTimer + " seconds left until recharged.";
		}
	}
	
	// cast a flare at the location of the player
	IEnumerator Flare(int color){
		flareCooldownTimer = 10f;
		//animator.Play("Flare");
        transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Flare");
        yield return new WaitForSeconds(0.5f);
		Quaternion flareRot = Quaternion.Euler(90, 0, 0);
		Vector3 flarePos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) + transform.right/2f;
		if (color == 1){
			GameObject newGO = (GameObject)PhotonNetwork.Instantiate("SignalFlareRed", flarePos, flareRot, 0);
		}
		else if (color == 2){
			GameObject newGO = (GameObject)PhotonNetwork.Instantiate("SignalFlareBlue", flarePos, flareRot, 0);	
		}
	}

    // attack in front of player
    IEnumerator Attack()
    {
		float attackType = Random.Range(0f, 1f);
		if (attackType <= 0.5f)
		//	animator.SetTrigger("Attack3Trigger");
            transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Attack3Trigger");
		else if (attackType > 0.5f)
            //animator.SetTrigger("Attack6Trigger");
            transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Attack6Trigger");
        yield return new WaitForSeconds(0.3f);
		DealDamage();
    }
	
    void DealDamage()
    {
		RaycastHit hitInfo;
		
        if (Physics.SphereCast(transform.position, 0.2f, transform.forward, out hitInfo, 1))
        {
            Debug.Log("We hit: " + hitInfo.collider.name);
            if (hitInfo.collider.tag == "Monster")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<MonsterAI>().name);
				
				if (hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() - damage <= 0){
                    //currentEXP += hitInfo.collider.transform.GetComponent<MonsterAI>().getExpDrop();
                    //mouseVitality.setCurrentExperiencePoints(currentEXP);
                    Debug.Log("got EXP");
                    currentEXP += 20;
					
				}

				hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", damage);
                if (isCrippled)
                {
                    hitInfo.collider.transform.GetComponent<PhotonView>().RPC("Crippled", PhotonTargets.AllBuffered);
                    isCrippled = false;
                }

            }
            if (hitInfo.collider.tag == "MonsterElite")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<MonsterAI>().name);

                if (hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() - damage <= 0)
                {
                    //currentEXP += hitInfo.collider.transform.GetComponent<MonsterAI>().getExpDrop();
                    //mouseVitality.setCurrentExperiencePoints(currentEXP);
                    currentEXP += 50;
					
                }

                hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", damage);
                if (isCrippled)
                {
                    hitInfo.collider.transform.GetComponent<PhotonView>().RPC("Crippled", PhotonTargets.AllBuffered);
                    isCrippled = false;
                }

            }
            if (hitInfo.collider.tag == "Boss")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<MonsterAI>().name);

                if (hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() - damage <= 0)
                {
                    //currentEXP += hitInfo.collider.transform.GetComponent<MonsterAI>().getExpDrop();
                    //mouseVitality.setCurrentExperiencePoints(currentEXP);
                    currentEXP += 200;
                }

                hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", damage);
                if (isCrippled)
                {
                    hitInfo.collider.transform.GetComponent<PhotonView>().RPC("Crippled", PhotonTargets.AllBuffered);
                    isCrippled = false;
                }

            }
            if (hitInfo.collider.tag == "PuzzleRoomBoss")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<MonsterAI>().name);

                if (hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() - damage <= 0)
                {
                    //currentEXP += hitInfo.collider.transform.GetComponent<MonsterAI>().getExpDrop();
                    //mouseVitality.setCurrentExperiencePoints(currentEXP);
                    currentEXP += 200;
                }

                hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", damage);
                if (isCrippled)
                {
                    hitInfo.collider.transform.GetComponent<PhotonView>().RPC("Crippled", PhotonTargets.AllBuffered);
                    isCrippled = false;
                }

            }
            if (hitInfo.collider.tag == "Cat")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<CatMovement>().name);
                
                if (hitInfo.collider.transform.GetComponent<CatMovement>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<CatMovement>().getHealth() - damage <= 0)
                {
                    GameObject.Find("WinObj").GetComponent<WinScript>().setCatDeaths();
                    currentEXP += 100;
                }
				
				hitInfo.collider.transform.GetComponent<CatMovement>().SendMessage("TakeDamage", damage);
                if (isCrippled)
                {
                    hitInfo.collider.transform.GetComponent<PhotonView>().RPC("Crippled", PhotonTargets.AllBuffered);
                    isCrippled = false;
                }
            }
			transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 3, 1f);
        }
		else{
			transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 4, 1f);
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
			if (hitColliders[i].tag == "Key"){
				hitColliders[i].transform.GetComponent<Key>().Interact();
				numKeysHeld++;
				interactText.text = "";
				canTakeKey = false;
			}
			if (hitColliders[i].tag == "PuzzlePiece"){
				hitColliders[i].transform.GetComponent<PuzzlePiece>().Interact();
				Debug.Log("take puzzle piece");
				interactText.text = "";
				canTakeKey = false;
			}
			if (hitColliders[i].tag == "Exit"){
				hitColliders[i].transform.GetComponent<Exit>().Interact();
				interactText.text = "You can't seem to activate it";
			}
			if (hitColliders[i].tag == "Chest"){
				if (numKeysHeld > 0){
					numKeysHeld -= hitColliders[i].transform.GetComponent<Chest>().Interact();
				}
			}
            i++;
        }
    }
    [PunRPC]
    void PlayAnim(string a)
    {
        GetComponent<Animator>().Play(a);
    }
    [PunRPC]
    void SetTrigger(string a)
    {
        GetComponent<Animator>().SetTrigger(a);
    }
    [PunRPC]
    void SetInteger(string a, int i)
    {
        GetComponent<Animator>().SetInteger(a, i);
    }
    // collision with objects
    void OnCollisionEnter(Collision collisionInfo)
    {
        
        transform.GetComponent<PhotonView>().RPC("SetInteger", PhotonTargets.All, "Jumping", 0);
        if (collisionInfo.gameObject.tag == "Ground")
        {
            isGrounded = true;
            onLava = false;
			onIce = false;
        }
        // if enters lava
        if (collisionInfo.gameObject.tag == "Lava")
        {
            isGrounded = true;
            onLava = true;
        }
		if (collisionInfo.gameObject.tag == "Ice")
        {
            isGrounded = true;
            onIce = true;
        }
        // if steps on a mine
        if (collisionInfo.gameObject.tag == "Mine")
        {
            isGrounded = true;
			
            Mine mine = collisionInfo.gameObject.GetComponent<Mine>();
			mine.transform.GetComponent<PhotonView>().RPC("explode", PhotonTargets.MasterClient, 2f);
			// if mine hasnt been exploded already then take damage
			if (mine.exploded == false){
				TakeDamage(mine.mineSize * 50);
			}
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
		if (obj.tag == "Chest"){
			Chest chest = obj.gameObject.GetComponent<Chest>();
			if (chest.chestOpen || chest.chestOpening)
				canOpenChest = false;
		}
	}

    // when player leaves the trigger
    void OnTriggerExit(Collider obj)
    {
		if (obj.tag == "Spike"){
			onSpikes = false;
			canMove = true;
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
		if (obj.tag == "Exit"){
			interactText.text = "";
			canOpenExit = false;
		}
    }
    [PunRPC]
    void destroyPU(Collider obj)
    {

        PhotonNetwork.Destroy(obj.gameObject);
    }
    // when player collides
    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Powerup")
        {
            Powerup pup = obj.GetComponent<Powerup>();
            // movement speed boost
            if (pup.powerupType == 0)
            {
                movementModifier = 1.25f;
                movementModifierTimer = 10f;
            }
            // hp restore
            else if (pup.powerupType == 1)
            {
				if (currentHealth < maxHealth){
					// do nothing
				}
				else{
					// heal for 20% of missing health
					float missingHP = maxHealth - currentHealth;
					currentHealth += missingHP*0.2f;
				}
            }
            // invis
            else if (pup.powerupType == 2)
            {
				transform.GetComponent<PhotonView>().RPC("Invis", PhotonTargets.AllBuffered, 0.2f, 1f);
				invisDuration = 15f;
            }
            // exp boost
            else if (pup.powerupType == 3)
            {
				currentEXP += 100f;
            }
            //Debug.Log("destroy " + obj);
            transform.GetComponent<PhotonView>().RPC("destroyPU", PhotonTargets.MasterClient, obj);
        }

        // put spikes here because we dont want spikes displacing the player
        if (obj.tag == "Spike")
        {
            onSpikes = true;
			canMove = false;
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
		if (obj.tag == "Exit"){
			interactText.text = "Press E to insert Puzzle Pieces";
			canOpenExit = true;
		}
		if (obj.tag == "SteelTrap"){
			// trap just stops movement for 5 seconds, can use the wait for animation function for this
			transform.position = new Vector3(obj.transform.position.x, transform.position.y, obj.transform.position.z);
			WaitForAnimation(5f);
			TakeDamage(5f);
			obj.GetComponent<SteelTrap>().activate(this.gameObject);
		}
		if (obj.tag == "Stomp"){
			// player gets knocked down for 1.5 seconds
			TakeDamage(10f);
			transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Unarmed-Death1");
			WaitForAnimation(1.5f);
		}
    }
    public float getHealth()
    {
        return currentHealth;
    }

    /* Adds a new Skill to the Skills Learned by the Explorer Character */
    public void addLearnedSkill(int skillID)
    {
        /* Checks if Skill has already been Learned */
        if (!(this.getLearnedSkills().Contains(skillID)))
        {
            /* Checks if Explorer Character has enough Skill Points to add the Skill */

            /* Checks Skill Tier for type of Skill Point required */
            if (mouseSkill.getCharSkills()[skillID].getSkillTier() == 0)
            {
                /* Checks to see if character has enough Regular Skill Points */
                if (this.skillPoints > 0)
                {
                    Debug.Log("The number of Skill Points is: " + this.skillPoints);
                    learnedSkills.Add(skillID);  // Adds specified skill
                    this.skillPoints--;  // Decrements the number of Regular Skill Points
                    Debug.Log("The number of Skill Points is: " + this.skillPoints);
                }
            }

            else if (mouseSkill.getCharSkills()[skillID].getSkillTier() == 1)
            {
                /* Checks to see if character has enough Ultimate Skill Points */
                if (this.ultimateSkillPoints > 0)
                {
                    Debug.Log("The number of Skill Points is: " + this.skillPoints);
                    learnedSkills.Add(skillID);
                    this.ultimateSkillPoints--;
                    Debug.Log("The number of Skill Points is: " + this.skillPoints);
                }
            }
        }
    }

    /* Sets the Skills currently learned by the Explorer Character */
    public void setLearnedSkills(List<int> learnedSkills)
    {
        this.learnedSkills = learnedSkills;
    }

    /* Gets the Skills currently learned by the Explorer Character */
    public List<int> getLearnedSkills()
    {
        return learnedSkills;
    }
	
	/* Stun the player */
	public void denyPlayerMovement(){
		canMove = false;
	}
	
	public void allowPlayerMovement(){
		canMove = true;
	}

    /* Gets the number of Regular Skill Points available to the Character */
    public int getRegularSP()
    {
        return this.skillPoints;
    }

    /* Gets the number of Ultimate Skill Points available to the Character */
    public int getUltimateSP()
    {
        return this.ultimateSkillPoints;
    }
    [PunRPC]
    IEnumerator respawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        //spawns at random mouse spawn location
        SpawnM mys = sm[Random.Range(0, 2)];
        transform.position = mys.transform.position;
        transform.rotation = mys.transform.rotation;
        //enable movement and cam movement again
        alive = true;
        currentHealth = maxHealth;
        GetComponent<MouseMovement>().enabled = true;
        CamMovement cam = gameObject.GetComponentInChildren<CamMovement>();
        cam.enabled = true;
        transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Unarmed-Idle");
    }

}
