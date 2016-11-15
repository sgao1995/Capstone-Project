using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	// maze variables
	public Maze mazePrefab;
	private Maze mazeInstance;

	// function that calls the function that starts the game 
	private	void Start () {
		BeginGame();
	}
	
	// space bar to restart the game
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}
	}
	
	// restart the game
	private void RestartGame () {
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}

	// start the game
	private void BeginGame () {
		// set minimap
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.StartMazeCreation();
		IntVector2 spawnPoint = new IntVector2(0, 0);
		Camera.main.clearFlags = CameraClearFlags.Depth;
		Camera.main.rect = new Rect (0f, 0f, 0.5f, 0.5f);
		Camera.main.transform.position = new Vector3(spawnPoint.x, 10, spawnPoint.z);
	}
}