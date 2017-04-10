using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CatMovement : MonoBehaviour
{
    // stats
    private int level = 0;
    private float currentEXP = 0;
    private float maxEXP;
    public float power;
    private float speed = 3.0f; //speed value
    private float jumpForce;//amount of jump force
    public float currentHealth = 100;
    private float maxHealth;
    private int skillPoints;
    private int ultimateSkillPoints;
    private List<int> learnedSkills;
    // movement speed
    private float movementModifier = 1;
    private float movementModifierTimer = 10f;
    private float crippledSpeedMod = 1f; //speed mod that depends on if the cat gets crippled by a mouse
    // invis duration
    private float invisDuration = 0f;

    // attack
    private Animator animator;
    private float attackPower;
    private float attackPowerM; //this is the attackpower after damage modifiers are applied
    private float attackCooldownDelay;
    private float attackCooldownTimer = 1f;
    private int damageModifier = 1;
    private int damageModifierF = 1; //for the focus skill
    private float damageModifierTimer = 20f;
    private bool focusTimerPause = false;

    private Vector3 moveV; //vector to store movement
    public Rigidbody catrb;

	
	// jump variables
	private bool isGrounded = false;
	// status effects
	private bool onLava = false;
	private bool onIce = false;
	private bool onSpikes = false;
	private bool alive = true;
	private bool canToggleDoor = false;
	private bool canMove = true;

    // skills
    private int numSkillSlotsMaximum = 4;  // Sets the maximum number of Skill Slots for the Cat
	private float[] skillCooldownTimers = new float[4]; // the cooldown timer
	private float[] skillCooldowns = new float[4]; // the max cooldown
    private float healingAmt = 7.5f;
    private GameObject targetMouse;
    private float beginT = 0f;
    private float beginT2 = 0f;
    private float beginT3 = 0f;
    private float holdT = 2.0f;
    private float Invis = 3.0f;
    private bool pauseTimer = false;
    private float stalkerTime = 5f;
    private bool isStalkerActive = false;
    private bool bashActive = false;
    private bool isBrawlerActive = false; //this one tells the cat is the brawler skill for the mouse is active or not
    private bool HSActive = false; //Needed for heightened sense skill
    private bool LIWActive = false; //Needed for lieinwait skill
    private bool FocusActive = false; //Needed for focus skill
    private bool canUseSkills = true;

    /* Vitality System attribute parameters */
    private float[] vitalLevelHP = {100, 125, 160, 200};  // Health Points of Cat per Level
    private float[] vitalLevelEXP = {200, 400, 800, 2000};  // Experience Points Cat per Level

    /* HUD state */
    public Vitality catVitality;  // Vitality System component
    public Skill catSkill;  // Skill System component
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
	public AudioSource soundPlayer;

    private Collider[] hitCollider;
	
	// list of traps the player has set
	private List<GameObject> steelTrapsList = new List<GameObject>();

    void Start()
    {
        catrb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; //cursor is gone from screen

        LevelUp();  // Starts at the first level
        animator = GetComponent<Animator>();

        /* Initialises list of Learned Skills for the Cat */
        learnedSkills = new List<int>();

        /*  Finds and initialises the Vitality System component */
        GameObject catVitalityGameObject = GameObject.Find("Vitality");
		catVitality = catVitalityGameObject.GetComponent<Vitality>();

        /*  Finds and initialises the Skill System component */
        GameObject catSkillGameObject = GameObject.Find("Skill");
        catSkill = catSkillGameObject.GetComponent<Skill>();

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
        // find and initialize sound effects
        soundPlayer = GetComponent<AudioSource>();
        soundPlayer.clip = footstepSound;
		// steel traps
    }
    IEnumerator ObjectiveMessage()
    {
        if (GameObject.Find("Objective").GetComponent<Text>())
        {
            Debug.Log("messagedisplayed");
        }
        ObjectiveMsg.text = "Find and Eliminate 3 Explorers";
        yield return new WaitForSeconds(5);
        ObjectiveMsg.text = "";
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
		jumpForce = 300f;
		attackCooldownDelay = 1.1f;
		
        /* Sets Character Maximum Health for new Level */
        this.maxHealth = this.vitalLevelHP[this.level - 1];
        this.currentHealth = this.maxHealth;

        /* Sets Character Maximum Experience for new level */
        this.maxEXP = this.vitalLevelEXP[this.level - 1];
        this.currentEXP = 0;
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
	public void useSkill(int skillCode){
        if (canUseSkills)
        {
            switch (skillCode)
            {
                case 11:
                    Debug.Log("use 11");
                    //heightened Senses
                    HSActive = true;
                    break;
                case 12:
                    //lie in wait
                    Debug.Log("use 12");
                    LIWActive = true;
                    break;
                case 13:
                    Debug.Log("use 13");
                    //Focus skill
                    //temporary for focus skill
                    if (!focusTimerPause)
                    {
                        beginT3 += Time.deltaTime;
                    }
                    if (focusTimerPause)
                    {
                        beginT3 = 0f;
                    }
                    if (beginT3 >= 10.0f)
                    {
                        if (!isBrawlerActive)
                        {
                            damageModifierF = 2;
                        }
                        else
                        {
                            damageModifierF = 1;
                        }
                    }
                    else
                    {
                        damageModifierF = 1;
                    }
                    focusTimerPause = false;
                    break;
                case 14:
                    Debug.Log("use 14");
                    //Leap skill
                    isGrounded = false;
                    transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "JumpTrigger");
                    transform.GetComponent<PhotonView>().RPC("SetInteger", PhotonTargets.All, "Jumping", 1);
                    catrb.AddForce(new Vector3(0, 350f, 0) + (transform.forward * 150f));
                    break;
                case 15:
                    Debug.Log("use 15");
                    //Recoup skill
                    //temporary for recoup skill
                    StartCoroutine(recoup());
                    break;
                case 16:
                    //Stalker skill
                    Debug.Log("use 16");
                    StartCoroutine(Stalker());
                    break;
                case 17:
                    //Bash skill 
                    Debug.Log("use 17");
                    if (attackCooldownTimer <= 0 && !Input.GetKey(KeyCode.Escape) && !miniMenuShowing)
                    {
                        WaitForAnimation(0.7f);
                        StartCoroutine(Bash());
                    }
                    break;
                case 18:
                    /* "Lasso - Throw out a rope that pulls the first enemy hit to you. 15 second cooldown." */
                    Debug.Log("use 18");
                    transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Throw");
                    WaitForAnimation(0.7f);
                    transform.GetComponent<PhotonView>().RPC("Lasso", PhotonTargets.AllBuffered);
                    break;
                case 19:
                    Debug.Log("use 19");
                    /* "Trap - Lay down a steel trap that lasts 300 seconds that will snare the first enemy that steps on it for 4 seconds. Maximum of 5 traps at once. 10 second cooldown. */
                    transform.GetComponent<PhotonView>().RPC("PlaceTrap", PhotonTargets.AllBuffered);
                    break;
                case 20:
                    Debug.Log("use 20");
                    //The Hunter skill
                    theHunter();
                    break;
                case 21:
                    Debug.Log("use 21");
                    //reload skill
                    break;
            }
        }
	}
	
	/* Function that placed a trap on the ground in front of the player */
	[PunRPC]
	IEnumerator PlaceTrap(){
		yield return new WaitForSeconds(0.3f);
		Quaternion trapRot = Quaternion.Euler(0, 0, 0);
		Vector3 trapPos = new Vector3(transform.position.x, -0.051f, transform.position.z) + transform.forward;
		GameObject trapGO = (GameObject)PhotonNetwork.InstantiateSceneObject("SteelTrap", trapPos, trapRot, 0);
		steelTrapsList.Add(trapGO);
		// if there are more than 5 traps, remove the earliest placed one
		if (steelTrapsList.Count > 5){
			transform.GetComponent<PhotonView>().RPC("destroyObj", PhotonTargets.MasterClient, steelTrapsList[0]);
			// and cleanse the trap list
			steelTrapsList.RemoveAt(0);
		}
	}
    //if mouse has brawler skill active, skills that affect the mouse are disabled
    [PunRPC]
    void BrawlerActive()
    {
        isBrawlerActive = true;
        Debug.Log("BRAWLER IS ACTIVE");
    }
    [PunRPC]
    void BrawlerNotActive()
    {
        isBrawlerActive = false;
        Debug.Log("BRAWLER IS NOT ACTIVE");
    }
	
	/* Throws a lasso in front of the player, pulling monsters and players towards the cat if connected*/
	[PunRPC]
    IEnumerator Lasso()
    {	                
		yield return new WaitForSeconds(0.3f);
		Quaternion lassoRot = Quaternion.Euler(0, 0, 0);
		Vector3 lassoPos = transform.position;
		Lasso newLasso = ((GameObject)PhotonNetwork.InstantiateSceneObject("Lasso", lassoPos, lassoRot, 0)).GetComponent<Lasso>();
		// range of 10 units
		Camera catCam = transform.Find("CatCam").GetComponent<Camera>();
		Ray ray = catCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		Vector3 endPos = ray.origin + ray.direction * 10f;
		newLasso.Initialize(transform.position + (transform.up*0.7f) + (transform.right*0.5f) + (transform.forward*0.5f), endPos, ray, transform.gameObject);
    }
    IEnumerator lieInWait() //lieinwait skill, wait 3 seconds and become invisible
    {
        //become invisible
        transform.GetComponent<PhotonView>().RPC("InvisC", PhotonTargets.AllBuffered, 0.2f, 3f);
        //Debug.Log("INVIS");
        yield return null;
    }

    [PunRPC]
    void uncloak()
    {
        if (!isStalkerActive)
        {
            pauseTimer = true;
            StartCoroutine(InvisC(1f, 1f));
        }   
    }
    [PunRPC]
    IEnumerator InvisC(float val, float time)
    {
        for (float f = 1f; f >= 0; f -= Time.deltaTime / time)
        {
            Color catColor = transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().material.color;
            catColor.a = Mathf.Lerp(transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().material.color.a, val, f); //lerp to make it look smoother
            transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().material.color = catColor;
            yield return null;
        }
    }
    IEnumerator recoup() //Recoup skill, heals 75 hp over 10 seconds
    {
        float time = 10;
        if(currentHealth < maxHealth)
        {
            while(time > 0)
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
    IEnumerator Bash() //Bash Skill, stun effect will only work on enemy players
    {
        focusTimerPause = true;//so the Focus buff will be reset to damagemultiplierF of 1
        if (!isBrawlerActive)
        {
            bashActive = true;
        }
        transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Attack3Trigger");
        attackCooldownTimer = attackCooldownDelay;
        yield return new WaitForSeconds(0.3f);
        DealDamage();
        bashActive = false;
    }
    IEnumerator Stalker() //activate to become invisible for 5 seconds with muffled footsteps
    {
        transform.GetComponent<PhotonView>().RPC("InvisC", PhotonTargets.AllBuffered, 0.2f, 1f);
        isStalkerActive = true;
        yield return new WaitForSeconds(5);
        Debug.Log("Stalker inactive");
        isStalkerActive = false;
        transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);
    }
    void theHunter() //Ultimate skill, teleports you to a random enemy player
    {
        GameObject[] target;
        target = GameObject.FindGameObjectsWithTag("Mouse");
        float dist = Mathf.Infinity;
        Vector3 position = transform.position;
        for (int i = 0; i < target.Length; i++)
        {
            Vector3 d = target[i].transform.position - position;
            float dt = d.sqrMagnitude;
            if (dt < dist)
            {
                targetMouse = target[i];
                dist = dt;
            }
        }
        Vector3 temp = new Vector3(2f, 0, 0);
        transform.position = targetMouse.transform.position + temp;
    }

	[PunRPC]
    void playSound(int type, float t)
    {
		switch(type){
			case 0:
                if (isStalkerActive)
                {
                    soundPlayer.PlayOneShot(footstepSound, 0.1f);
                }else
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
				//soundPlayer.PlayOneShot(smokescreenSound, t);
				break;
			case 6:
				break;
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
        /* Updates the HUD state for the current player */
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = new Vector3(22, 0, 25);
        }
        //focus skill passive
        if (FocusActive)
        {
            if (!focusTimerPause)
            {
                beginT3 += Time.deltaTime;
            }
            if (focusTimerPause)
            {
                beginT3 = 0f;
            }
            if (beginT3 >= 10.0f)
            {
                damageModifierF = 2;
            }
            else
            {
                damageModifierF = 1;
            }
            focusTimerPause = false;
        }
        //lieInWait passive
        if (LIWActive)
        {
            if (!pauseTimer)
            {
                beginT2 += Time.deltaTime;
            }
            if (pauseTimer)
            {
                beginT2 = 0f;
            }
            if (beginT2 >= 3.0f)
            {
                StartCoroutine(lieInWait());
            }
            pauseTimer = false;
        }     
        /* Updates the Character attributes and HUD state for the current player */
        if (GetComponent<PhotonView>().isMine)
        {
            /* If the Maximum Experience Points for the current Level is reached, Level Up the Character */
            if (this.currentEXP >= this.maxEXP)
            {
                this.LevelUp();
            }

            /* Updates the Vitality System states */

            /* Updates the Level attributes */
            catVitality.setCurrentLevel(this.level);

            /* Updates the Health Points attributes of the Cat */
            catVitality.setMaxHealthPoints(this.maxHealth); // Updates the Maximum Health Points
            catVitality.setCurrentHealthPoints(this.currentHealth);  // Updates the Current Health Points

            /* Updates the Experience Points attributes */
            catVitality.setMaximumExperiencePoints(this.maxEXP);
            catVitality.setCurrentExperiencePoints(this.currentEXP);

            /* Updates the Character Type attribute as a Cat */
            catVitality.setCharacterType("cat");

            /* Updates the number of Skill System states */

            /* Updates the maximum number of Skill Slots */
            catSkill.setMaxSkillSlots(numSkillSlotsMaximum);

           /* Updates the Skill assigned to each Skill Slot */
           catSkill.setSlotAssign(learnedSkills);
        }

        // status effects
        if (onLava){
			TakeDamage(0.2f);
		}
		if (onSpikes){
			TakeDamage(0.2f);
		}
		
		// left click
		if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0 && !Input.GetKey(KeyCode.Escape) && !miniMenuShowing)
        {
            transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);
            focusTimerPause = true;
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
            this.useSkill(this.catSkill.useSkillSlot(1));  // Uses the 1st Skill Slot of the Hunter Class
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.useSkill(this.catSkill.useSkillSlot(2));  // Uses the 2nd Skill Slot
        }
		if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.useSkill(this.catSkill.useSkillSlot(3));  // Uses the 3rd Skill Slot
        }
		if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.useSkill(this.catSkill.useSkillSlot(4));  // Uses the 4th Skill Slot
        }
		
		if (canToggleDoor){
			if (Input.GetKeyDown(KeyCode.E)){
				InteractWithObject();
			}
		}
		if (Input.GetKeyDown(KeyCode.K)){
			TakeDamage(5f);
		}
		if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None; //if we press esc, cursor appears on screen
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
        if (damageModifierTimer > 0f)
        {
            damageModifierTimer -= Time.deltaTime;
        }else
        {
            damageModifier = 1;
        }
		if (invisDuration > 0f){
			invisDuration -= Time.deltaTime;
			if (invisDuration <= 0){
				transform.GetComponent<PhotonView>().RPC("InvisC", PhotonTargets.AllBuffered, 1f, 1f);
			}
		}
        //Debug.Log("DEAL THIS MUCH DAMAGE:" + attackPowerM);
	}
    //heightenedSenses
    void heightenedSenses()
    {
        hitCollider = Physics.OverlapSphere(this.transform.position, 10);
        foreach (Collider C in hitCollider)
        {
            if (C.GetComponent<Collider>().transform.root != this.transform && (C.GetComponent<Collider>().tag == "Mouse" || C.GetComponent<Collider>().tag == "Mouse(Clone)"))
            {
                Debug.Log("Mice Detected");
                if (!isBrawlerActive)
                {
                    Alert.SetActive(true);
                }else
                {
                    Alert.SetActive(false);
                }

            }else
            {
                Alert.SetActive(false);
            }
        }
    }
    void LateUpdate()
    {
        if (HSActive)
        {
            heightenedSenses();
        }
    }
    void FixedUpdate()
    {
		if (canMove)
        {	
			if (onIce){
				moveV = moveV.normalized * speed * movementModifier * Time.deltaTime * 0.1f*crippledSpeedMod;
			}
			else{
				moveV = moveV.normalized * speed * movementModifier * Time.deltaTime*crippledSpeedMod;
			}
            transform.Translate(moveV);
            // attack power modifier
            attackPowerM = attackPower * damageModifier * damageModifierF;
			// skill testing
			if (Input.GetKey(KeyCode.Q))
				useSkill(18);
			// movement control
			if (isGrounded && !onSpikes)
			{
				moveV = new Vector3(0, 0, 0);
				soundPlayer.pitch = Random.Range(0.9f, 1.1f);
				if (Input.GetKey(KeyCode.A))
				{
                    //animator.Play("MoveLeft");
                    transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "MoveLeft");

				   	// play sound effect
					if (!soundPlayer.isPlaying){
                        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                    }
					if (onIce)
						catrb.AddRelativeForce(Vector3.left*0.2f, ForceMode.Impulse);
					else{
						moveV = new Vector3(-1, 0, moveV.z);
					}
				}
				if (Input.GetKey(KeyCode.D))
				{
                    //animator.Play("MoveRight");
                    transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "MoveRight");
                    // play sound effect
					if (!soundPlayer.isPlaying){
                        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                    }
					if (onIce)
						catrb.AddRelativeForce(Vector3.right*0.2f, ForceMode.Impulse);
					else{
						moveV = new Vector3(1, 0, moveV.z);
					}
				}
				if (Input.GetKey(KeyCode.W))
				{
                    if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                        //animator.Play("MoveForward");
                        transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "MoveForward");
				   	// play sound effect
					if (!soundPlayer.isPlaying){
                        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                    }
					if (onIce)
						catrb.AddRelativeForce(Vector3.forward*0.2f, ForceMode.Impulse);
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
                        transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 0, 1f);
                    }
					if (onIce)
						catrb.AddRelativeForce(Vector3.back*0.2f, ForceMode.Impulse);
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
            isGrounded = false;
            transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "JumpTrigger");
            transform.GetComponent<PhotonView>().RPC("SetInteger", PhotonTargets.All, "Jumping", 1);
            catrb.AddForce(new Vector3(0, jumpForce, 0));
        
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

    // take a certain amount of damage
    public void TakeDamage(float amt)
    {
        transform.GetComponent<PhotonView>().RPC("uncloak", PhotonTargets.AllBuffered);
        //   transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "GetHit");
        //   WaitForAnimation(0.5f);
        transform.GetComponent<PhotonView>().RPC("changeHealth", PhotonTargets.AllBuffered, amt);
		transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 2, 1f);
    }
    //if crippled by explorer, speed is modified to be 30% slower
    [PunRPC]
    IEnumerator Crippled()
    {
        crippledSpeedMod = 0.7f;
        Debug.Log("CRIPPLED");
        yield return new WaitForSeconds(5);
        crippledSpeedMod = 1f;
    }
    [PunRPC]
    void changeHealth(float dmg)
    {
		if (alive){
			currentHealth -= dmg;
			if (currentHealth <= 0)
			{
				currentHealth = 0;
				Death();
			}
		}
    }
    
    void Death(){
		Debug.Log("player died");
        GameObject.Find("GUI").GetComponent<WinScript>().setCatDeaths();
        alive = false;
        StopCoroutine(recoup());
        Debug.Log("player died");
		alive = false;
        //animator.Play("Unarmed-Death1");
        transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Unarmed-Death1");
        WaitForAnimation(5f);
	}

    // attack in front of player
    IEnumerator Attack()
    {
		float attackType = Random.Range(0f, 1f);
		if (attackType <= 0.5f)
            transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Attack3Trigger");
			//animator.SetTrigger("Attack3Trigger");
		else if (attackType > 0.5f)
            //animator.SetTrigger("Attack6Trigger");
            transform.GetComponent<PhotonView>().RPC("SetTrigger", PhotonTargets.All, "Attack6Trigger");
        yield return new WaitForSeconds(0.3f);
		DealDamage();
    }
	
    public void DealDamage()
    {
		RaycastHit hitInfo;
		
        if (Physics.SphereCast(transform.position, 0.2f, transform.forward, out hitInfo, 1))
        {
            Debug.Log("We hit: " + hitInfo.collider.name);
            if (hitInfo.collider.tag == "Monster")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<MonsterAI>().name);
				
				if (hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() - attackPowerM <= 0){
                    //currentEXP += hitInfo.collider.transform.GetComponent<MonsterAI>().getExpDrop();
                    //mouseVitality.setCurrentExperiencePoints(currentEXP);
                    Debug.Log("got exp Cat");
                    currentEXP += 20;
					
				}
                if (bashActive)
                {
                    hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", attackPowerM+15f);
                }
                else
                {
                    hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", attackPowerM);
                }	
            }
            if (hitInfo.collider.tag == "MonsterElite")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<MonsterAI>().name);

                if (hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() - attackPowerM <= 0)
                {
                    //currentEXP += hitInfo.collider.transform.GetComponent<MonsterAI>().getExpDrop();
                    //mouseVitality.setCurrentExperiencePoints(currentEXP);
                    currentEXP += 50;
					
                }

                if (bashActive)
                {
                    hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", attackPowerM + 15f);
                }
                else
                {
                    hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", attackPowerM);
                }

            }
            if (hitInfo.collider.tag == "Boss")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<MonsterAI>().name);

                if (hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() - attackPowerM <= 0)
                {
                    //currentEXP += hitInfo.collider.transform.GetComponent<MonsterAI>().getExpDrop();
                    //mouseVitality.setCurrentExperiencePoints(currentEXP);
                    currentEXP += 200;
                }

                if (bashActive)
                {
                    hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", attackPowerM + 15f);
                }
                else
                {
                    hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", attackPowerM);
                }

            }
            if (hitInfo.collider.tag == "PuzzleRoomBoss")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<MonsterAI>().name);

                if (hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MonsterAI>().getHealth() - attackPowerM <= 0)
                {
                    //currentEXP += hitInfo.collider.transform.GetComponent<MonsterAI>().getExpDrop();
                    //mouseVitality.setCurrentExperiencePoints(currentEXP);
                    currentEXP += 200;
                }

                if (bashActive)
                {
                    hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", attackPowerM + 15f);
                }
                else
                {
                    hitInfo.collider.transform.GetComponent<MonsterAI>().SendMessage("takeDamage", attackPowerM);
                }

            }
            if (hitInfo.collider.tag == "Mouse")
            {
                Debug.Log("Trying to hurt " + hitInfo.collider.transform.name + " by calling script " + hitInfo.collider.transform.GetComponent<MouseMovement>().name);

                if (hitInfo.collider.transform.GetComponent<MouseMovement>().getHealth() > 0 && hitInfo.collider.transform.GetComponent<MouseMovement>().getHealth() - attackPowerM <= 0)
                {
                    currentEXP += 100;
                }
				
				if (bashActive)
                {
                    Debug.Log("TRYING TO STUN!");
                    hitInfo.collider.transform.GetComponent<MouseMovement>().SendMessage("Stunned");
                    hitInfo.collider.transform.GetComponent<MouseMovement>().SendMessage("TakeDamage", attackPowerM+15f);     
                }
                else
                {
                    hitInfo.collider.transform.GetComponent<MouseMovement>().SendMessage("TakeDamage", attackPowerM);
                }	
            }
			transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 3, 1f);
        }
		else{
			transform.GetComponent<PhotonView>().RPC("playSound", PhotonTargets.AllBuffered, 4, 1f);
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
                hitColliders[i].transform.GetComponent<MazeDoor>().SendMessage("Interact");
			}
            i++;
        }
	}
	
	// collision with objects
	void OnCollisionEnter(Collision collisionInfo){

        //animator.SetInteger("Jumping", 0);
        transform.GetComponent<PhotonView>().RPC("SetInteger", PhotonTargets.All, "Jumping", 0);
        if (collisionInfo.gameObject.tag == "Ground"){
            isGrounded = true;

            onLava = false;
			onIce = false;
		}
		// if enters lava
		if (collisionInfo.gameObject.tag == "Lava"){
            isGrounded = true;

            onLava = true;
		}
		if (collisionInfo.gameObject.tag == "Ice")
        {
            isGrounded = true;

            onIce = true;
        }
		// if steps on a mine
		if (collisionInfo.gameObject.tag == "Mine"){
            isGrounded = true;

            Mine mine = collisionInfo.gameObject.GetComponent<Mine>();
			mine.transform.GetComponent<PhotonView>().RPC("explode", PhotonTargets.MasterClient, 2f);
			// if mine hasnt been exploded already then take damage
			if (mine.exploded == false){
				TakeDamage(mine.mineSize * 50f);
			}
        }
        // if gets hit by a dart
        if (collisionInfo.gameObject.tag == "dart")
        {
            transform.GetComponent<PhotonView>().RPC("Asleep", PhotonTargets.AllBuffered);
        }
	}
    [PunRPC]
    IEnumerator Asleep()//if is hit by a sleeping dart, is put to sleep for 5 seconds
    {
        Debug.Log("STUNNED");
        canMove = false;
        canUseSkills = false;
        Debug.Log("is Stunned");
        transform.GetComponent<PhotonView>().RPC("PlayAnim", PhotonTargets.All, "Unarmed-Death1");
        yield return new WaitForSeconds(5);
        Debug.Log("is not Stunned");
        canMove = true;
        canUseSkills = true;
    }
    [PunRPC]
    void destroyPU(Collider obj)
    {

        PhotonNetwork.Destroy(obj.gameObject);
    }
	[PunRPC]
	void destroyObj(GameObject go){
		PhotonNetwork.Destroy(go);
	}
	
	
	void OnTriggerStay(Collider obj){
		// enters range to use an object. Certain objects take priority over others
		if (obj.tag == "Door"){
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
	void OnTriggerExit(Collider obj){

		if (obj.tag == "Spike"){
			onSpikes = false;
			canMove = true;
		}
		if (obj.tag == "Door"){
			interactText.text = "";
			canToggleDoor = false;
		}
	}
	
	// when player enters range of something
	void OnTriggerEnter(Collider obj){
		if(obj.tag == "Powerup"){
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
				if (currentHealth >= maxHealth){
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
				transform.GetComponent<PhotonView>().RPC("InvisC", PhotonTargets.AllBuffered, 0.2f, 1f);
				invisDuration = 15f;
            }
            // exp boost
            else if (pup.powerupType == 3)
            {
				currentEXP += 100f;
            }
            //Debug.Log("destroy " + obj);
            //transform.GetComponent<PhotonView>().RPC("destroyPU", PhotonTargets.MasterClient, obj);
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.Destroy(obj.gameObject);
            }

        }
        // put spikes here because we dont want spikes displacing the player
        if (obj.tag == "Spike"){
			onSpikes = true;
			canMove = false;
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
        return this.currentHealth;
    }

    /* Adds a new Skill to the Skills Learned by the Hunter Character */
    public void addLearnedSkill(int skillID)
    {
        /* Checks if Skill has already been Learned */
        if (!(this.getLearnedSkills().Contains(skillID)))
        {
            /* Checks if Hunter Character has enough Skill Points to add the Skill */

            /* Checks Skill Tier for type of Skill Point required */
            if (catSkill.getCharSkills()[skillID].getSkillTier() == 0)
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

            else if (catSkill.getCharSkills()[skillID].getSkillTier() == 1)
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

    /* Sets the Skills currently learned by the Hunter Character */
    public void setLearnedSkills(List<int> learnedSkills)
    {
        this.learnedSkills = learnedSkills;
    }

    /* Gets the Skills currently learned by the Hunter Character */
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
}