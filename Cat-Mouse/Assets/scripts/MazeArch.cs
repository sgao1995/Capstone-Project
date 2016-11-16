using UnityEngine;

// an arch in the maze, to get from room to room
public class MazeArch : MazePassage {
	public Transform archPart1;
	public Transform archPart2;
	public Transform archPart3;

	// initialize the walls with their material
	public override void Initialize (MazeCell cellOne, MazeCell cellTwo, MazeDirection direction) {
		base.Initialize(cellOne, cellTwo, direction);
		archPart1.GetComponent<Renderer>().material = cellOne.room.roomSettings.wallMaterial;
		archPart2.GetComponent<Renderer>().material = cellOne.room.roomSettings.wallMaterial;
		archPart3.GetComponent<Renderer>().material = cellOne.room.roomSettings.wallMaterial;
	}
}