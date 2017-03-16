using UnityEngine;
using System.Collections;

public class Lasso : MonoBehaviour {
	private float length = 0f;
	private float maxLength = 10f;
	public Vector3 originalPosition;
	private bool hitObject = false;
	private bool pullObject = false;
	private GameObject target;
	
	// Update is called once per frame
	void Update () {
		if (hitObject == false){
			length += Time.deltaTime;		
			/* Extends the lasso as time goes on, if it hits a non pullable object then it is destroyed */
			if (length < maxLength){
				transform.localScale = new Vector3(0.03f, length, 0.03f);
				transform.position = transform.position + transform.forward/Time.deltaTime;
			}
			else{
				PhotonNetwork.Destroy(this.gameObject);
			}
		}
		else{
			length -= Time.deltaTime;
			if (length < 0f){
				PhotonNetwork.Destroy(this.gameObject);
			}
			// pull the object along
			if (pullObject){
				target.transform.position -= originalPosition/Time.deltaTime;
			}
		}
	}
	
	// collision with objects
	void OnCollisionEnter(Collision obj){
		// dont hit the cat
		if (obj.gameObject.tag != "Cat"){
			hitObject = true;
			if (obj.gameObject.tag == "Mouse" || obj.gameObject.tag == "Monster" || obj.gameObject.tag == "MonsterElite" || obj.gameObject.tag == "Boss"){
				pullObject = true;
				target = obj.gameObject;
			}
		}
	}
}
