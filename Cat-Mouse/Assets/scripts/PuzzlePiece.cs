using UnityEngine;
using System.Collections;

public class PuzzlePiece : MonoBehaviour {
	public float duration = 60f;
	public float time = 0;
	float newY = -1;
	float rotation = 0f;
	public int pieceID;
	
	// rotation and spawn animations
	void Update () {
		if (time < duration){
			newY += 1.3f/duration;
			Vector3 newPos = new Vector3(this.transform.position.x, newY, this.transform.position.z);
			this.transform.position = newPos;
			
			time++;
		}
		rotation += 1.5f;
		Quaternion newQuat = Quaternion.Euler(0f, rotation, 0f);
		this.transform.rotation = newQuat;
	}
	
	public int Interact(){
		PhotonNetwork.Destroy(this.gameObject);
		return pieceID;
	}
}
