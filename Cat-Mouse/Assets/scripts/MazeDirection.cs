using UnityEngine;

// an enumerator for directions
public enum MazeDirection {
	North,
	East,
	South,
	West
}

// functions for direction
public static class MazeDirections {

	// only 4 main directions
	public const int numDirections = 4;

	// return a random value 
	public static MazeDirection RandomValue {
		get {
			return (MazeDirection)Random.Range(0, numDirections);
		}
	}
	
	// vector form of the directions
	private static IntVector2[] vectors = {
		new IntVector2(0, 1),
		new IntVector2(1, 0),
		new IntVector2(0, -1),
		new IntVector2(-1, 0)
	};
	
	// opposites of the original directions
	private static MazeDirection[] directionOpposite = {
		MazeDirection.South,
		MazeDirection.West,
		MazeDirection.North,
		MazeDirection.East
	};
	
	// 3f vector form of the directions
	private static Quaternion[] directionRotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};

	// convert a direction to int vector
	public static IntVector2 ToIntVector2 (this MazeDirection direction) {
		return vectors[(int)direction];
	}
	
	// convert a direction into its opposite
	public static MazeDirection GetOpposite (this MazeDirection direction) {
		return directionOpposite[(int)direction];
	}
	
	// convert a direction into its rotational vector
	public static Quaternion ToRotation (this MazeDirection direction) {
		return directionRotations[(int)direction];
	}
}