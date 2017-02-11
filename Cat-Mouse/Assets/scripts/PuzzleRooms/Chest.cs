using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {
	public Transform hinge;
	public bool chestOpen = false;
	bool chestOpening = false;
	public float duration = 30f;
	float newRot = 0;
	
	// open the chest
	public void Interact(){
		if (chestOpen == false){
			chestOpening = true;
		}
	}
	
	// animate chest opening
	void Update(){
		if (chestOpening){
			newRot += 70f/duration;
			if (newRot >= 70f + 70f/duration){
				chestOpening = false;
				chestOpen = true;
			}
			Quaternion newQuat = Quaternion.Euler(newRot, 0f, 0f);
			hinge.localRotation = newQuat;
		}
	}
}
