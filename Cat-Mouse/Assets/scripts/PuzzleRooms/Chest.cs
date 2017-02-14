using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {
	public Transform hinge;
	public bool chestOpen = false;
	bool chestOpening = false;
	public float duration = 30f;
	float newRot = 0;
	public int whichPieceInside;
	public bool puzzlePieceSpawned = false;	
	public PuzzlePiece[] puzzlePiecePrefabs;
	
	// open the chest
	public int Interact(){
		if (chestOpen == false){
			chestOpening = true;
			return 1;
		}
		return 0;
	}
	
	// animate chest opening
	void Update(){
		if (chestOpening){
			newRot += 70f/duration;
			if (newRot >= 70f + 70f/duration){
				chestOpening = false;
				chestOpen = true;
			}
			Quaternion newQuat = Quaternion.Euler(newRot, 0f, 0f);
			hinge.localRotation = newQuat;
		}
		if(chestOpen && !puzzlePieceSpawned){
			Vector3 spawnPos = new Vector3(this.transform.position.x, -1f, this.transform.position.z);
			Quaternion spawnRot = new Quaternion(0f, 0f, 0f, 0f);
			string whichPiece = "PuzzlePiece" + whichPieceInside;
			GameObject newGO = (GameObject)PhotonNetwork.Instantiate(whichPiece, spawnPos, spawnRot, 0);
			puzzlePieceSpawned = true;
			PuzzlePiece newPiece = newGO.GetComponent<PuzzlePiece>();
			newPiece.pieceID = whichPieceInside;
		}
	}
}
