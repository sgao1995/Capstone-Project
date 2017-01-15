using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public float ballSize;
	public int ballType;

	// initialize
	public void setBall (int type, float size) {
		this.ballType = type;
		this.ballSize = size;
	}
}
