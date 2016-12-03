using UnityEngine;

// an arch in the maze, to get from room to room
public class MazeArch : MazePassage {

	// initialize the walls
	public override void Initialize (MazeCell cellOne, MazeCell cellTwo, MazeDirection direction) {
		base.Initialize(cellOne, cellTwo, direction);
	}
}