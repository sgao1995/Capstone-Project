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
	
	// prefabs
	public MazeCell cellPrefab;
	public MazePassage passagePrefab;
	public MazeWall[] wallPrefabs;
	public MazeArch archPrefab;
	public MazeDoor doorPrefab;
	public MazeWall exitPrefab;
	
	// puzzle room generation
	public List<MazeRoom> puzzleRooms = new List<MazeRoom>();
	public Material lavaMat;
	public Material iceMat;
	public List<Mine> mineList = new List<Mine>();
	public List<Target> targetList = new List<Target>();
	public List<Ball> ballList = new List<Ball>();
	public List<Spike> spikeList = new List<Spike>();
	
	// set values to generate same maze every time
	public static int mazeGenerationNumber = 40; // anything above 2 makes a decent maze (didnt test every number though), also dont go above the size
	public static IntVector2 startPoint = new IntVector2(mazeGenerationNumber, mazeGenerationNumber);
	public int roomTypeCount = 0;
	public float cellSize = 1;
	
	// chests and keys
	// format: [key1x, key1z, key2x, key2z, key3x, key3z]
	private List<float> keyLocations = new List<float>();
	private List<float> chestLocations = new List<float>();
		
	// creates a new room 
	private MazeRoom CreateRoom (int roomType) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.settingsIndex = roomTypeCount;
		// reserve last room setting type for puzzle rooms
		if (roomTypeCount < roomSettings.Length -2){
			roomTypeCount += 1;
		}
		else{
			roomTypeCount = 0;
		}
		// exclude creating rooms of a certain roomType
		if (newRoom.settingsIndex == roomType) {
			if (newRoom.settingsIndex < roomSettings.Length -2){
				newRoom.settingsIndex = newRoom.settingsIndex +1;	
			}
			else{
				newRoom.settingsIndex = newRoom.settingsIndex -1;	
			}

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
		MazeCell newCell = CreateCell(startPoint);
		newCell.Initialize(CreateRoom(-1));
		uninitializedCells.Add(newCell);
		while (uninitializedCells.Count > 0) {
			GenerateNextStep(uninitializedCells);
		}
		// completed maze
		CleanseWalls();
		CreatePuzzleRoom();

		Debug.Log("donemaze, " + rooms.Count);
		CreateExit();
		
	}

	// do the next step of the maze generation
	private void GenerateNextStep (List<MazeCell> uninitializedCells) {
		int currentIndex = uninitializedCells.Count -1;
		MazeCell currentCell = uninitializedCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			uninitializedCells.RemoveAt(currentIndex);
			return;
		}
		// move in set directions based on input number directions
		MazeDirection direction = currentCell.UninitializedDirection(mazeGenerationNumber);
		
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
		//passage = Instantiate(passagePrefab) as MazePassage;
		//passage.Initialize(secondCell, firstCell, direction.GetOpposite());
	}
	
	// make two cells have passages towards each other
	private void CreatePassage (MazeCell firstCell, MazeCell secondCell, MazeDirection direction) {
		// use perlin noise to calculate where arches should be
		float generatedNoise = Mathf.PerlinNoise((firstCell.coordinates.x * secondCell.coordinates.x)/(float)mazeGenerationNumber, (firstCell.coordinates.z * secondCell.coordinates.z)/(float)mazeGenerationNumber);
		MazePassage prefabType;
		if (generatedNoise < 0.2){
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
		// use perlin noise to calculate wall type
		float generatedNoise;
		if (secondCell == null){
			generatedNoise = 1;
		}
		else{
			generatedNoise = Mathf.PerlinNoise((firstCell.coordinates.x * secondCell.coordinates.x)/(float)mazeGenerationNumber, (firstCell.coordinates.z * secondCell.coordinates.z)/(float)mazeGenerationNumber);
		}
		MazeWall prefabType;
		// torch
		if (generatedNoise < 0.2){
			prefabType = wallPrefabs[1];
		}
		else{
			prefabType = wallPrefabs[0];
		}
		
		// set the wall type
		MazeWall wall = Instantiate(prefabType) as MazeWall;
		wall.Initialize(firstCell, secondCell, direction);
		// instantiate it again for the other cell (if it exists)
		if (secondCell != null) {
			wall = Instantiate(prefabType) as MazeWall;
			wall.Initialize(secondCell, firstCell, direction.GetOpposite());
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
		temp.name = "Maze Cell " + coords.x + ", " + coords.z;
		temp.transform.parent = transform;
		temp.transform.localPosition =
			new Vector3(coords.x - size.x * 0.5f + 0.5f, 0f, coords.z - size.z * 0.5f + 0.5f);
		return temp;
	}
	
	// returns a list of the existing walls between two cells
	private List<Collider> ExistingWalls (MazeCell firstCell, MazeCell secondCell){
		float avgX = (firstCell.transform.position.x + secondCell.transform.position.x)/2f;
		float avgZ = (firstCell.transform.position.z + secondCell.transform.position.z)/2f;
		Vector3 center = new Vector3(avgX, 0f, avgZ);
		Collider[] collider = Physics.OverlapSphere(center,0.8f);
		List<Collider> walls = new List<Collider>();

		// check the hitbox area
        int i = 0;
        while (i < collider.Length) {
			if (collider[i].tag == "Wall"){
				walls.Add(collider[i]);
			}
            i++;
        }
		return walls;
	}
	
		// returns a list of the existing walls between two cells
	private List<Collider> ExistingArches (MazeCell firstCell, MazeCell secondCell){
		float avgX = (firstCell.transform.position.x + secondCell.transform.position.x)/2f;
		float avgZ = (firstCell.transform.position.z + secondCell.transform.position.z)/2f;
		Vector3 center = new Vector3(avgX, 0f, avgZ);
		Vector3 aboveCenter = new Vector3(avgX, 3f, avgZ);
		Collider[] collider = Physics.OverlapCapsule(center, aboveCenter,0.7f);
		List<Collider> arches = new List<Collider>();

		// check the hitbox area
        int i = 0;
        while (i < collider.Length) {
			if (collider[i].tag == "Arch"){
				arches.Add(collider[i]);
			}
            i++;
        }
		return arches;
	}
	
	// cleanse duplicate walls, make the walls only 1 wall thick
	private void CleanseWalls(){
		// directional vectors
		List<IntVector2> dir = new List<IntVector2>();
		dir.Add(new IntVector2(0, 1));
		dir.Add(new IntVector2(1, 0));
		dir.Add(new IntVector2(0, -1));
		dir.Add(new IntVector2(-1, 0));
		
		// storage list
		List<Collider> existingWalls = new List<Collider>();
		List<Collider> existingArches = new List<Collider>();		
		for (int x = 0; x < size.x; x++){
			for (int z = 0; z < size.z; z++){
				for (int w = 0; w < 4; w++){
					MazeCell currentCell = GetCell(new IntVector2(x, z));
					if (ContainsCoordinates(new IntVector2(x, z) + dir[w])){
						MazeCell neighbor = GetCell(new IntVector2(x, z) + dir[w]);
						existingWalls = ExistingWalls(currentCell, neighbor);
						existingArches = ExistingArches(currentCell, neighbor);
						if (existingWalls.Count > 1){
							Destroy(existingWalls[1].transform.parent.gameObject);
						}
						if (existingArches.Count > 1){
							Destroy(existingArches[1].transform.parent.gameObject);
						}
					}

				}
			}
		}
	}
	
	// generate the puzzle rooms
	private void CreatePuzzleRoom(){
		// go through puzzle room candidates, different sized rooms can hold different puzzles
		// first cull small rooms from the list
		for (int r = 0; r < rooms.Count; r++){
			if (rooms[r].getCells().Count > 40 && rooms[r].getCells().Count < 50){
				puzzleRooms.Add(rooms[r]);
			}
		}
		// make sure there are only 3 puzzle rooms
		int num = puzzleRooms.Count;
		if (puzzleRooms.Count > 3){
			for (int r = 0; r < num -3; r++){
				puzzleRooms.RemoveAt(puzzleRooms.Count-1);
			}
		}
		// change the floor material for the rooms
		for (int r = 0; r < puzzleRooms.Count; r++){
			puzzleRooms[r].roomSettings = roomSettings[roomSettings.Length -1];	
			for (int c = 0; c < puzzleRooms[r].getCells().Count; c++){
				puzzleRooms[r].getCells()[c].updateMaterial(puzzleRooms[r]);
			}
		}
		// for each arch check if it connects a normal room with a puzzle room
		GameObject[] archList = GameObject.FindGameObjectsWithTag("ArchObject");
		for (int a = 0; a < archList.Length; a++){
			// add a door
			MazeArch arch = archList[a].GetComponent<MazeArch>();
			MazeCell cellOne;
			MazeCell cellTwo;
			MazeDirection dir;
			if (arch.cell.room.roomSettings.floorMaterial == roomSettings[roomSettings.Length -1].floorMaterial){
				cellOne = arch.cell;
				cellTwo = arch.otherCell;
				dir = arch.direction;
				MazePassage prefabType = doorPrefab; 
				MazePassage passage = Instantiate(prefabType) as MazePassage;
				passage.Initialize(cellOne, cellTwo, dir);
			}
		}
	}
	
	// return the key spawn locations
	public List<float> getKeySpawns(){
		return keyLocations;
	}
	
	// return the chest spawn locations
	public List<float> getChestSpawns(){
		return chestLocations;
	}
	
	// set the chests in random locations on the map
	public void GenerateChestLocations(){
		// for each chest
		for (int c = 0; c < 3; c++){
			// set a cell, 0 to size
			int cellX = (int)(Mathf.PerlinNoise(c*7/(float)mazeGenerationNumber, c*5/(float)mazeGenerationNumber) * size.x);
			int cellZ = (int)(Mathf.PerlinNoise(c*3/(float)mazeGenerationNumber, c*8/(float)mazeGenerationNumber) * size.z);
			MazeCell currentCell = GetCell(new IntVector2(cellX, cellZ));
			
			// set a bit of offset
			float chestX = currentCell.transform.position.x + Mathf.PerlinNoise(currentCell.coordinates.x/(float)mazeGenerationNumber, currentCell.coordinates.x/(float)mazeGenerationNumber);
			float chestZ = currentCell.transform.position.z + Mathf.PerlinNoise(currentCell.coordinates.z/(float)mazeGenerationNumber, currentCell.coordinates.z/(float)mazeGenerationNumber);
			
			// add to list
			chestLocations.Add(chestX);
			chestLocations.Add(chestZ);
		}
	}
	
	// generate the puzzles in the puzzle rooms
	public void GeneratePuzzles(List<int> activePuzzleTypes){
		for (int r = 0; r < 3; r++){
			int puzzleType = activePuzzleTypes[r];
			bool keyGenerated = false;
			// floor is lava, jump from safe point to safe point
			if (puzzleType == 0){
				for (int c = 0; c < puzzleRooms[r].getCells().Count; c++){
					// use Perlin noise to generate lava areas
					float generatedNoise;
					MazeCell currentCell = puzzleRooms[r].getCells()[c];
					generatedNoise = Mathf.PerlinNoise((currentCell.coordinates.x * currentCell.coordinates.x)/(float)mazeGenerationNumber, (currentCell.coordinates.z * currentCell.coordinates.z)/(float)mazeGenerationNumber);
					if (generatedNoise < 0.5){
						// lava
						currentCell.transform.GetChild(0).gameObject.tag = "Lava";
						currentCell.changeMaterial(lavaMat);
					}
					// spawn a key
					else if (keyGenerated == false){
						keyGenerated = true;
						keyLocations.Add(currentCell.transform.position.x + Mathf.PerlinNoise(currentCell.coordinates.x/(float)mazeGenerationNumber, currentCell.coordinates.x/(float)mazeGenerationNumber));
						keyLocations.Add(currentCell.transform.position.z + Mathf.PerlinNoise(currentCell.coordinates.z/(float)mazeGenerationNumber, currentCell.coordinates.z/(float)mazeGenerationNumber));
					}
				}
			}
			// minefield
			else if (puzzleType == 1){
				float mineMultiplier = puzzleRooms[r].getCells().Count/25f;
				// spawn mines of varying size and damage based on Perlin noise
				// number of mines varies with room size
				for (int c = 0; c < puzzleRooms[r].getCells().Count; c++){
					MazeCell currentCell = puzzleRooms[r].getCells()[c];
					float chanceForMine = Mathf.PerlinNoise((currentCell.coordinates.x * currentCell.coordinates.x)/(float)mazeGenerationNumber, (currentCell.coordinates.z * currentCell.coordinates.z)/(float)mazeGenerationNumber);
					if (chanceForMine < 0.25f * mineMultiplier){
						// generate a mine
						float mineX = currentCell.transform.position.x + Mathf.PerlinNoise(currentCell.coordinates.x/(float)mazeGenerationNumber, currentCell.coordinates.x/(float)mazeGenerationNumber);
						float mineZ = currentCell.transform.position.z + Mathf.PerlinNoise(currentCell.coordinates.z/(float)mazeGenerationNumber, currentCell.coordinates.z/(float)mazeGenerationNumber);
						float mineSize = Mathf.PerlinNoise(c/(float)mazeGenerationNumber, c/(float)mazeGenerationNumber);
						Vector3 spawnPos = new Vector3(mineX, -0.45f, mineZ);
						Quaternion spawnRot = new Quaternion(0f, 0f, 0f, 0f);
						GameObject newGO = (GameObject)PhotonNetwork.Instantiate("Mine", spawnPos, spawnRot, 0);
						Mine newMine = newGO.GetComponent<Mine>();
						newMine.setMine(mineSize);
						CapsuleCollider cap = newMine.GetComponent<CapsuleCollider>();
						cap.center = new Vector3(0f, 0.5f, 0f);
						cap.radius = 0.7f;
						mineList.Add(newMine);
					}
					// spawn a key
					else if (keyGenerated == false){
						keyGenerated = true;
						keyLocations.Add(currentCell.transform.position.x + Mathf.PerlinNoise(currentCell.coordinates.x/(float)mazeGenerationNumber, currentCell.coordinates.x/(float)mazeGenerationNumber));
						keyLocations.Add(currentCell.transform.position.z + Mathf.PerlinNoise(currentCell.coordinates.z/(float)mazeGenerationNumber, currentCell.coordinates.z/(float)mazeGenerationNumber));
					}
				}
			}
			// spike room
			else if (puzzleType == 2){
				float spikeMultiplier = puzzleRooms[r].getCells().Count/25f;
				// spawn spikes
				for (int c = 0; c < puzzleRooms[r].getCells().Count; c++){
					MazeCell currentCell = puzzleRooms[r].getCells()[c];
					float chanceForSpikes = Mathf.PerlinNoise((currentCell.coordinates.x * currentCell.coordinates.x)/(float)mazeGenerationNumber, (currentCell.coordinates.z * currentCell.coordinates.z)/(float)mazeGenerationNumber);
					if (chanceForSpikes < 0.3f * spikeMultiplier){
						// generate a set of spikes
						float spikeX = currentCell.transform.position.x + Mathf.PerlinNoise(currentCell.coordinates.x/(float)mazeGenerationNumber, currentCell.coordinates.x/(float)mazeGenerationNumber);
						float spikeZ = currentCell.transform.position.z + Mathf.PerlinNoise(currentCell.coordinates.z/(float)mazeGenerationNumber, currentCell.coordinates.z/(float)mazeGenerationNumber);
						float spikeSize = 4*Mathf.PerlinNoise(c/(float)mazeGenerationNumber, c/(float)mazeGenerationNumber);
						float spikeTimer = Mathf.PerlinNoise(c/(float)mazeGenerationNumber, c/(float)mazeGenerationNumber);
						Vector3 spawnPos = new Vector3(spikeX, 0.0f, spikeZ);
						Quaternion spawnRot = new Quaternion(0f, 0f, 0f, 0f);
						GameObject newGO = (GameObject)PhotonNetwork.Instantiate("Spike", spawnPos, spawnRot, 0);
						Spike newSpike = newGO.GetComponent<Spike>();
						newSpike.setSpike(spikeSize, spikeTimer);
						spikeList.Add(newSpike);
					}
					// spawn a key
					else if (keyGenerated == false){
						keyGenerated = true;
						keyLocations.Add(currentCell.transform.position.x + Mathf.PerlinNoise(currentCell.coordinates.x/(float)mazeGenerationNumber, currentCell.coordinates.x/(float)mazeGenerationNumber));
						keyLocations.Add(currentCell.transform.position.z + Mathf.PerlinNoise(currentCell.coordinates.z/(float)mazeGenerationNumber, currentCell.coordinates.z/(float)mazeGenerationNumber));
					}
				}
			}
			// ball room, roll balls onto targets
			else if (puzzleType == 3){
				// pick 6 areas
				List<int> itemCells = new List<int>();
				int counter = 0;
				int index = 0;
				while(index < 7){
					int cell = 0;
					counter++;
					// keep giving cell new values until its a new value not already in the list
					do {
						cell = (int)Random.Range(0f, puzzleRooms[r].getCells().Count -1);
					}while(itemCells.Contains(cell));
					itemCells.Add(cell);
					index++;
				}
				for(int i = 0; i < 7; i++){
					Debug.Log(itemCells[i]);}
				// first 3 cells contain balls
				for (int b = 0; b < 3; b++){
					Vector3 spawnPos = new Vector3(puzzleRooms[r].getCells()[itemCells[b]].transform.position.x, 0.5f, puzzleRooms[r].getCells()[itemCells[b]].transform.position.z);
					Quaternion spawnRot = new Quaternion(0f, 0f, 0f, 0f);
					GameObject newGO = (GameObject)PhotonNetwork.Instantiate("Ball", spawnPos, spawnRot, 0);
					Ball newBall = newGO.GetComponent<Ball>();
					newBall.setSize(0.5f);
					ballList.Add(newBall);
				}
				// last 3 cells contain holes
				for (int h = 3; h < 6; h++){
					Vector3 spawnPos = new Vector3(puzzleRooms[r].getCells()[itemCells[h]].transform.position.x, 0f, puzzleRooms[r].getCells()[itemCells[h]].transform.position.z);
					Quaternion spawnRot = new Quaternion(0f, 0f, 0f, 0f);
					GameObject newGO = (GameObject)PhotonNetwork.Instantiate("Target", spawnPos, spawnRot, 0);
					Target newTarget = newGO.GetComponent<Target>();
					newTarget.setSize(0.5f);
					targetList.Add(newTarget);
				}
				MazeCell currentCell = puzzleRooms[r].getCells()[itemCells[6]];
				keyLocations.Add(currentCell.transform.position.x + Mathf.PerlinNoise(currentCell.coordinates.x/(float)mazeGenerationNumber, currentCell.coordinates.x/(float)mazeGenerationNumber));
				keyLocations.Add(currentCell.transform.position.z + Mathf.PerlinNoise(currentCell.coordinates.z/(float)mazeGenerationNumber, currentCell.coordinates.z/(float)mazeGenerationNumber));
			}
			// ice room, low control over movement
			else if (puzzleType == 4){
				for (int c = 0; c < puzzleRooms[r].getCells().Count; c++){
					// change all floor cells to ice
					MazeCell changeThis = puzzleRooms[r].getCells()[c];
					changeThis.transform.GetChild(0).gameObject.tag = "Ice";
					changeThis.changeMaterial(iceMat);
				}
				float generatedNoise = Mathf.PerlinNoise(r*mazeGenerationNumber, r*mazeGenerationNumber);
				int keyCell = (int)(generatedNoise*puzzleRooms[r].getCells().Count);
				MazeCell currentCell = puzzleRooms[r].getCells()[keyCell];
				keyLocations.Add(currentCell.transform.position.x + Mathf.PerlinNoise(currentCell.coordinates.x/(float)mazeGenerationNumber, currentCell.coordinates.x/(float)mazeGenerationNumber));
				keyLocations.Add(currentCell.transform.position.z + Mathf.PerlinNoise(currentCell.coordinates.z/(float)mazeGenerationNumber, currentCell.coordinates.z/(float)mazeGenerationNumber));
			}
			// boss room, fight a strong monster
			else if (puzzleType == 5){
				// spawn boss
				MazeCell bossCell = puzzleRooms[r].getCells()[Random.Range(0, puzzleRooms[r].getCells().Count)];
				Vector3 bossPos = new Vector3(bossCell.transform.position.x, 0f, bossCell.transform.position.z);
				Quaternion bossRot = new Quaternion(0f, 0f, 0f, 0f);
				GameObject monsterGO = (GameObject)PhotonNetwork.Instantiate("PuzzleRoomBoss", bossPos, bossRot, 0);
				monsterGO.GetComponent<MonsterAI>().enabled = true;
				MonsterAI monster = monsterGO.GetComponent<MonsterAI>();
				monster.setMonsterType("PuzzleRoomBoss");
				
				// key location
				MazeCell keyCell = puzzleRooms[r].getCells()[Random.Range(0, puzzleRooms[r].getCells().Count)];
				keyLocations.Add(keyCell.transform.position.x + Mathf.PerlinNoise(keyCell.coordinates.x/(float)mazeGenerationNumber, keyCell.coordinates.x/(float)mazeGenerationNumber));
				keyLocations.Add(keyCell.transform.position.z + Mathf.PerlinNoise(keyCell.coordinates.z/(float)mazeGenerationNumber, keyCell.coordinates.z/(float)mazeGenerationNumber));
			}
		}
		
	}
	
	// generate the exit path
	// exit door will always be at the edge of the map
	private void CreateExit(){
		// determine which edge of the map it will be on
		float whichSide = Mathf.PerlinNoise(mazeGenerationNumber, mazeGenerationNumber);
		int cellCoord = (int)(Mathf.PerlinNoise(mazeGenerationNumber, mazeGenerationNumber)*49);
		Debug.Log(cellCoord);
		MazeCell whichCell;
		MazeDirection direction;
		if (whichSide < 0.25f){
			// x, 49 side
			whichCell = cells[cellCoord, 49];
			direction = (MazeDirection)0;
		}
		else if (whichSide < 0.5f){
			// x, 0 side
			whichCell = cells[cellCoord, 0];
			direction = (MazeDirection)2;
		}
		else if (whichSide < 0.75f){
			// 49, x side
			whichCell = cells[49, cellCoord];
			direction = (MazeDirection)1;
		}
		else{
			// 0, x side
			whichCell = cells[49, cellCoord];
			direction = (MazeDirection)3;
		}
		GameObject wallObject = whichCell.transform.Find("MazeWall(Clone)").gameObject;
		Destroy(wallObject);
		Exit currentWall = Instantiate(exitPrefab) as Exit;
		currentWall.Initialize(whichCell, null, direction);
	}
	
	void Update(){
		
		// if puzzle room is completed then spawn a key
		
	}
}