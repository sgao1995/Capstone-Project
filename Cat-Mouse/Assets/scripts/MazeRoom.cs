using UnityEngine;
using System.Collections.Generic;

// different rooms in the maze
public class MazeRoom : ScriptableObject {
	public MazeRoomSettings roomSettings;
	public int settingsIndex;

	// a list of the cells each room contains
	private List<MazeCell> cells = new List<MazeCell>();
	
	// add a new area to the room
	public void Add (MazeCell cell) {
		cell.room = this;
		cells.Add(cell);
	}
	
	public List<MazeCell> getCells(){
		return cells;
	}
}