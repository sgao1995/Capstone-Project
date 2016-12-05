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
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.StartMazeCreation();
	}
}