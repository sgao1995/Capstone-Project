using UnityEngine;

// the edges of a maze cell
public abstract class MazeCellEdge : MonoBehaviour {
	// variables corresponding to an edge
	public MazeCell cell; 
	public MazeCell otherCell;
	public MazeDirection direction;
	
	// the base method for initializing an edge
	// other methods extend this class and override this function
	public virtual void Initialize (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation();
	}
}