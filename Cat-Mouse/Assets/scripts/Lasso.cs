using UnityEngine;
using System.Collections;

public class Lasso : MonoBehaviour {
	private float maxLength = 10f;
	private bool pullObject = false;
	private GameObject target;
	
	public Vector3 start;
	public Vector3 end;
	public bool onWayBack = false;
	public Vector3 dir;
	public Vector3 current;
	private Vector3 mid;
	private float length;
	
	private bool hitUnpullable = false;
	private bool begin = false;
	
	private GameObject cat;
	
	private Vector3 initialLocation;
	private float startTime;
	
	private float travelTime;
	
	/* Detect if something is hit AS SOON AS LASSO IS CAST */
	public void Initialize(Vector3 st, Vector3 en, GameObject c){
		initialLocation = st;
		startTime = Time.time;
		start = st;
		end = en;
		cat = c;
		dir = Vector3.Normalize(end - start);
		current = start;
		mid = (end - start)/2f + start;
		transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
		transform.localScale = new Vector3(0.05f, 0f, 0.05f);
		transform.position = mid;
		
		// ray cast and get first object hit
		RaycastHit hitInfo;
		
        if (Physics.SphereCast(start, 0.2f, dir, out hitInfo, 10f))
        {
            Debug.Log("raycast hit : " + hitInfo.collider.name);
			target = hitInfo.collider.gameObject;
			if (hitInfo.collider.name == "Monster(Clone)" || hitInfo.collider.name == "MonsterElite(Clone)" || hitInfo.collider.name == "Boss(Clone)" || hitInfo.collider.name == "Mouse(Clone)" || hitInfo.collider.name == "Ball"){
				pullObject = true;
				//onWayBack = true;
				// stop actions until the lasso finishes pulling object
				if (hitInfo.collider.name == "Mouse(Clone)"){
					target.GetComponent<MouseMovement>().denyPlayerMovement();
				}
				else if (hitInfo.collider.name == "Monster(Clone)" || hitInfo.collider.name == "MonsterElite" || hitInfo.collider.name == "Boss"){
					target.GetComponent<MonsterAI>().denyMonsterMovement();
				}
			}
			else{
				hitUnpullable = true;
			}
		}
		
		begin = true;
	}
	
	void Update(){
		if (begin){
			if (hitUnpullable){
				Debug.Log(Vector3.Distance(initialLocation + (10f * dir * (Time.time - startTime)), target.transform.parent.transform.position));
				Vector3 distToWall = initialLocation + (10f * dir * (Time.time - startTime));
				if (Vector3.Distance(new Vector3(distToWall.x, 0f, distToWall.z), target.transform.parent.transform.position) <= 1f){
					Debug.Log("hit wall");
					onWayBack = true;
				}
			}
			
			cat.GetComponent<CatMovement>().denyPlayerMovement();
			if (pullObject && onWayBack){
				if (target.tag == "Ball"){
					target.transform.position = current;
				}
				else{
					target.GetComponent<PhotonView>().RPC("ForcePositionChange", PhotonTargets.AllBuffered, new Vector3(current.x, 0f, current.z));
				}
			}
			
			if (onWayBack){
				current -= dir * Time.deltaTime * 10f;
				length = (current - start).magnitude;
				if (Vector3.Distance(current, end) >= maxLength-1.5f){
					if (pullObject){
						if (target.tag == "Mouse"){
							target.GetComponent<MouseMovement>().allowPlayerMovement();
						}
						else if (target.tag == "Monster" || target.tag == "MonsterElite" || target.tag == "Boss"){
							target.GetComponent<MonsterAI>().allowMonsterMovement();
						}
					}
					cat.GetComponent<CatMovement>().allowPlayerMovement();
					PhotonNetwork.Destroy(this.gameObject);
				}
			}
			else{
				current += dir * Time.deltaTime * 10f;
				length = (current - start).magnitude;
				if (length >= maxLength){
					onWayBack = true;
				}
				if (pullObject || hitUnpullable){
					if (Vector3.Distance(current, target.transform.position) <= 1f){
						onWayBack = true;
					}
				}
			}
			mid = (current - start)/2f + start;
			transform.localScale = new Vector3(0.05f, length/2f, 0.05f);
			transform.position = mid;
		}
	}

	
	/*
	void OnCollisionEnter(Collision obj){
		Debug.Log("Lasso hit " + obj.gameObject.tag);
		if (obj.gameObject.tag == "Monster" || obj.gameObject.tag == "MonsterElite" || obj.gameObject.tag == "Boss" || obj.gameObject.tag == "Mouse(Clone)"	|| obj.gameObject.tag == "Ball" ){
			hitThisTarget(obj.gameObject);
		}
	}*/
}
