using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {
	public Transform hinge;
	bool chestOpen = false;
	
	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -80f, 0f);
	
	// open the chest
	public void Interact(){
		if (chestOpen == false){
			OpenChest();
		}
	}
	
	// give a puzzle piece
	private void OpenChest(){
		// give the mouse TEAM a random puzzle piece
		Debug.Log("open chest");
		chestOpen = true;
		hinge.localRotation = normalRotation;
	}
}
