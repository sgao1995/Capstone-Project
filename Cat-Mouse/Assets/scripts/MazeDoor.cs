using UnityEngine;
using System.Collections;

public class MazeDoor : MazePassage {
	public Transform hinge;
	public bool doorOpen;
	// initialize
	//public override void Initialize (MazeCell cellOne, MazeCell cellTwo, MazeDirection direction) {
	///	base.Initialize(cellOne, cellTwo, direction);
	//	doorOpen = false;
	//}

	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -80f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 80f, 0f);

	private bool isMirrored;
	
	
	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		base.Initialize(primary, other, direction);
		doorOpen = false;
	}
	
	// interact with the door
	public void Interact(){
		if (doorOpen == true){
			CloseDoor();
		}
		else if(doorOpen == false){
			OpenDoor();
		}
	}
	
	// open the door
	private void OpenDoor(){
		Debug.Log("open");
		doorOpen = true;
		hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
		hinge.localPosition = new Vector3(0.25f, 0.7f, 0.15f);
	}
	
	// close the door
	private void CloseDoor(){
		Debug.Log("close");
		doorOpen = false;
		hinge.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		hinge.localPosition = new Vector3(0f, 0.7f, 0.5f);
	}
}
