using UnityEngine;

// the walls in the maze
public class MazeWall : MazeCellEdge {
	public Transform wall;

	// initialize the walls with their material
	public override void Initialize (MazeCell cellOne, MazeCell cellTwo, MazeDirection direction) {
		base.Initialize(cellOne, cellTwo, direction);
		wall.GetComponent<Renderer>().material = cellOne.room.roomSettings.wallMaterial;
	}
}