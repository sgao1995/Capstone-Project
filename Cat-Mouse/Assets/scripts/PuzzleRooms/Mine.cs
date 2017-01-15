using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {
	public float mineSize;

	// initialize
	public void setMine (float size) {
		this.mineSize = size;
		transform.localScale = new Vector3(size, 0.5f, size);
	}
}
