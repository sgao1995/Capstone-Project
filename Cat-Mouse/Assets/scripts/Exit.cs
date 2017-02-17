using UnityEngine;
using System.Collections;

public class Exit : MazeWall {
	
	// initialize the walls with their material
	public override void Initialize (MazeCell cellOne, MazeCell cellTwo, MazeDirection direction) {
		base.Initialize(cellOne, cellTwo, direction);
	}
	
	// interact with the door
	public void Interact(){
		if (GameObject.Find("WinObj").GetComponent<WinScript>().numPuzzlePiecesHeld() == 3){
			GameObject.Find("WinObj").GetComponent<WinScript>().openExit();
		}
	}
}
