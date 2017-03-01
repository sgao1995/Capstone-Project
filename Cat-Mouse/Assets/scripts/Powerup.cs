using UnityEngine;
using System.Collections;

/*
*	Powerup types
*	0: Movement speed boost
*	1: Attack damage boost
*	2: Invisibility
*	3: EXP boost
*/

public class Powerup : MonoBehaviour {
	public int powerupType;
	float rotation = 0f;
	public Material expMat;
	public Material hpMat;
	public Material speedMat;
	public Material invisMat;
	
	// initialize
	public void setType (int type) {
		this.powerupType = type;
		if(type == 0){
			transform.GetComponent<Renderer>().material = speedMat;
		}
		else if (type == 1){
			transform.GetComponent<Renderer>().material = hpMat;	
		}
		else if (type == 2){
			transform.GetComponent<Renderer>().material = invisMat;
		}
		else if (type == 3){
			transform.GetComponent<Renderer>().material = expMat;
		}
	}
	
	// a rotating animation
	void Update(){
		rotation += 1f;
		Quaternion newQuat = Quaternion.Euler(0f, rotation, 0f);
		this.transform.rotation = newQuat;
	}
}
