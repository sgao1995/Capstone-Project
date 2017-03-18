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
	
	/* Detect if something is hit AS SOON AS LASSO IS CAST */
	public void Initialize(Vector3 st, Vector3 en, Ray ray, GameObject c){
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
			hitThisTarget(hitInfo.collider.gameObject);
		}
	}
	
	void Awake(){
		begin = true;
	}
	
	void Update(){
		if (begin){
			cat.GetComponent<CatMovement>().denyPlayerMovement();
			if (pullObject && onWayBack){
				if (target.tag != "Ball")
					target.transform.position = new Vector3(current.x, 0f, current.z);
				else{
					target.transform.position = current;
				}
			}
			
			if (onWayBack){
				current -= dir * Time.deltaTime * 10f;
				length = (current - start).magnitude;
				if (Vector3.Distance(current, end) >= maxLength-1.5f){
					if (pullObject){
						if (target.tag == "Mouse(Clone)"){
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

	public void hitThisTarget(GameObject go){
		target = go;
		if (target.tag == "Monster" || target.tag == "MonsterElite" || target.tag == "Boss" || target.tag == "Mouse(Clone)"	|| target.tag == "Ball" ){
			pullObject = true;
			//onWayBack = true;
			// stop actions until the lasso finishes pulling object
			if (target.tag == "Mouse(Clone)"){
				target.GetComponent<MouseMovement>().denyPlayerMovement();
			}
			else if (target.tag == "Monster" || target.tag == "MonsterElite" || target.tag == "Boss"){
				target.GetComponent<MonsterAI>().denyMonsterMovement();
			}
		}
		else{
			hitUnpullable = true;
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
