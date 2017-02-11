using UnityEngine;
using System.Collections;

public class MazeDoor : MazePassage {
	public Transform hinge;
	public bool doorOpen;
	public bool doorOpening = false;
	public bool doorClosing = false;
	public float duration = 30f;

	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -80f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 80f, 0f);

	private bool isMirrored;
	float newRot = 0;
	Vector3 newPos;
	
	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		base.Initialize(primary, other, direction);
		doorOpen = false;
	}
	
	// interact with the door
	public void Interact(){
		if (doorOpen == true){
			doorClosing = true;
			doorOpening = false;
		}
		else if(doorOpen == false){
			doorOpen = true;
			doorOpening = true;
		}
	}
	
	void Update(){
		if (doorOpen){
			// transform the door
			if (doorOpening){
				if (isMirrored){
					newRot += 80f/duration;
					newPos = new Vector3(hinge.localPosition.x, 0.0f, hinge.localPosition.z - 0.15f/duration);
					if (newRot >= 80f + 80f/duration){
						doorOpening = false;
					}
				}
				else{
					newRot -= 80f/duration;
					newPos = new Vector3(hinge.localPosition.x, 0.0f, hinge.localPosition.z - 0.15f/duration);
					if (newRot <= -80f - 80f/duration){
						doorOpening = false;
					}
				}
				Quaternion newQuat = Quaternion.Euler(0f, newRot, 0f);
				hinge.localRotation = newQuat;
				hinge.localPosition = newPos;
			}
			if (doorClosing){
				if (isMirrored){
					newRot -= 80f/duration;
					newPos = new Vector3(hinge.localPosition.x, 0.0f, hinge.localPosition.z + 0.15f/duration);
					if (newRot <= 0f){
						doorClosing = false;
						doorOpen = false;
					}
				}
				else{
					newRot += 80f/duration;
					newPos = new Vector3(hinge.localPosition.x, 0.0f, hinge.localPosition.z + 0.15f/duration);
					if (newRot >= 0f){
						doorClosing = false;
						doorOpen = false;
					}
				}
				Quaternion newQuat = Quaternion.Euler(0f, newRot, 0f);
				hinge.localRotation = newQuat;
				hinge.localPosition = newPos;
			}
		}
	}
}
