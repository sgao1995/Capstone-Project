using UnityEngine;
using System.Collections;

public class MazeDoor : MazePassage {
	public Transform hinge;
	public bool doorOpen;
	public bool doorOpening = false;
	public bool doorClosing = false;
	public bool inTransition = false;
	public float duration = 30f;

	float newRot = 0;
	Vector3 newPos;
	
	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		base.Initialize(primary, other, direction);
		doorOpen = false;
	}

	// interact with the door
	public void Interact(){
		if (inTransition == false){
			if (doorOpen){
				doorClosing = true;
				doorOpening = false;
			}
			else if(doorOpen == false){
				doorOpen = true;
				doorOpening = true;
			}
			inTransition = true;
		}
	}
	
	void Update(){
		if (doorOpen){
			// transform the door
			if (doorOpening){
				newRot += 80f/duration;
				newPos = new Vector3(hinge.localPosition.x, 0.0f, hinge.localPosition.z + 0.1f/duration);
				if (newRot >= 80f + 80f/duration){
					doorOpening = false;
					inTransition = false;
				}

				Quaternion newQuat = Quaternion.Euler(0f, newRot, 0f);
				hinge.localRotation = newQuat;
				hinge.localPosition = newPos;
			}
			else if (doorClosing){
				newRot -= 80f/duration;
				newPos = new Vector3(hinge.localPosition.x, 0.0f, hinge.localPosition.z - 0.1f/duration);
				if (newRot <= 0){
					doorClosing = false;
					doorOpen = false;
					inTransition = false;
				}
				
				Quaternion newQuat = Quaternion.Euler(0f, newRot, 0f);
				hinge.localRotation = newQuat;
				hinge.localPosition = newPos;
			}
		}
	}
}
