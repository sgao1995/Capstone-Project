using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {
	public float spikeSize;
	public float activationTimer;
	public float time = 0;
	public bool spikesGoingUp = false;

	// initialize
	public void setSpike (float size, float timer) {
		this.spikeSize = size;
		this.activationTimer = timer * 200f;
		transform.localScale = new Vector3(size, 1f, size);
	}
	
	void Update(){
		// move the spikes
		if (spikesGoingUp){
			float newY = transform.position.y + 0.02f;
			transform.position = new Vector3(transform.position.x, newY, transform.position.z);
			if (transform.position.y > 0.2f){
				time = 0;
				spikesGoingUp = false;
			}
		}
		else{
			float newY = transform.position.y - 0.02f;
			transform.position = new Vector3(transform.position.x, newY, transform.position.z);
		}
		
		// if timer is reached then set spikes to go up
		if (time > activationTimer){
			spikesGoingUp = true;
		}
		else
			time += 1;
	}
}
