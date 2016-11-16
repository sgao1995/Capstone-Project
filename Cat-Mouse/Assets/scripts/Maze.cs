using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// maze generation 
public class Maze : MonoBehaviour {
	// variables 
	public IntVector2 size;
	private MazeCell[,] cells;
	public MazeRoomSettings[] roomSettings;
	private List<MazeRoom> rooms = new List<MazeRoom>();

	// probability for arch to be created instead of regular wall
	public float archProbability = 0.13f;
	
	// prefabs
	public MazeCell cellPrefab;
	public MazePassage passagePrefab;
	public MazeWall[] wallPrefabs;
	public MazeArch archPrefab;
		
	// creates a new room 
	private MazeRoom CreateRoom (int roomType) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
		// exclude creating rooms of a certain roomType
		if (newRoom.settingsIndex == roomType) {
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
		}
		newRoom.roomSettings = roomSettings[newRoom.settingsIndex];
		rooms.Add(newRoom);
		return newRoom;
	}
	
	// returns the cell corresponding to the coordinates given
	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

	// generate the whole maze 
	public void StartMazeCreation () {
		cells = new MazeCell[size.x, size.z];
		// list of all the cells that are still not yet fully initialized
		List<MazeCell> uninitializedCells = new List<MazeCell>();
		MazeCell newCell = CreateCell(GenerateRandomCoords);
		newCell.Initialize(CreateRoom(-1));
		uninitializedCells.Add(newCell);
		while (uninitializedCells.Count > 0) {
			GenerateNextStep(uninitializedCells);
		}
	}

	// do the next step of the maze generation
	private void GenerateNextStep (List<MazeCell> uninitializedCells) {
		int currentIndex = uninitializedCells.Count - 1;
		MazeCell currentCell = uninitializedCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			uninitializedCells.RemoveAt(currentIndex);
			return;
		}
		// move in random directions
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coords = currentCell.coordinates + direction.ToIntVector2();
		// if the coordinates are inside the maze, then...
		if (ContainsCoordinates(coords)) {
			MazeCell neighbor = GetCell(coords);
			// if cell not filled yet
			if (neighbor == null) {
				neighbor = CreateCell(coords);
				CreatePassage(currentCell, neighbor, direction);
				uninitializedCells.Add(neighbor);
			}
			// if the cells are in the same room
			else if (currentCell.room == neighbor.room) {
				CreateSameRoomPassage(currentCell, neighbor, direction);
			}
			// create a wall between the cells
			else {
				CreateWall(currentCell, neighbor, direction);
			}
		}
		// create a wall if it would go outside of the maze
		else {
			CreateWall(currentCell, null, direction);
		}
	}

	// create a passage bewteen two cells in the same room
	private void CreateSameRoomPassage (MazeCell firstCell, MazeCell secondCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(firstCell, secondCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(secondCell, firstCell, direction.GetOpposite());
	}
	
	// make two cells have passages towards each other
	private void CreatePassage (MazeCell firstCell, MazeCell secondCell, MazeDirection direction) {
		// chance that an arch is created
		float passageType = Random.value;
		MazePassage prefabType;
		if (passageType < archProbability){
			prefabType = archPrefab;
		}
		else{
			prefabType = passagePrefab;
		}
		// instantiate it once for each cell
		MazePassage passage = Instantiate(prefabType) as MazePassage;
		passage.Initialize(firstCell, secondCell, direction);
		passage = Instantiate(prefabType) as MazePassage;
		// create passage between two rooms
		if (passage is MazeArch) {
			secondCell.Initialize(CreateRoom(firstCell.room.settingsIndex));
		}
		else {
			secondCell.Initialize(firstCell.room);
		}
		passage.Initialize(secondCell, firstCell, direction.GetOpposite());
	}

	// create a wall between two cells
	private void CreateWall (MazeCell firstCell, MazeCell secondCell, MazeDirection direction) {
		// get a random wall type
		MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
		wall.Initialize(firstCell, secondCell, direction);
		// instantiate it again for the other cell (if it exists)
		if (secondCell != null) {
			wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
			wall.Initialize(secondCell, firstCell, direction.GetOpposite());
		}
	}
	
	// a function to return a random cell in the coordinate range
	public IntVector2 GenerateRandomCoords {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	// check to see if the coordinates are inside the maze 
	public bool ContainsCoordinates (IntVector2 coords) {
		return coords.x >= 0 && coords.x < size.x && coords.z >= 0 && coords.z < size.z;
	}

	// Create a new cell
	private MazeCell CreateCell (IntVector2 coords) {
		// instantiate the cells, set coordinates
		MazeCell temp = Instantiate(cellPrefab) as MazeCell;
		cells[coords.x, coords.z] = temp;
		temp.coordinates = coords;
		temp.transform.parent = transform;
		temp.transform.localPosition =
			new Vector3(coords.x - size.x * 0.5f + 0.5f, 0f, coords.z - size.z * 0.5f + 0.5f);
		return temp;
	}
}