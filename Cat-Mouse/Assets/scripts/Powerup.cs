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

	// initialize
	public void setType (int type) {
		this.powerupType = type;
	}
}
