using UnityEngine;
using System.Collections;

public class SignalFlare : MonoBehaviour {
	private float time = 0f;
	private float newY;

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		// move the flare
		if (time > 15f){
			PhotonNetwork.Destroy(this.gameObject);
		}
		else{
			newY = transform.position.y + 0.15f;
			transform.position = new Vector3(transform.position.x, newY, transform.position.z);
		}
	}
}
