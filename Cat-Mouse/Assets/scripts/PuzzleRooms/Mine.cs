using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {
	public float mineSize;
	public AudioClip explosionSound;
	public AudioSource soundPlayer;
	private float time = 0f;
	public bool exploded = false;

	// initialize
	public void setMine (float size) {
		this.mineSize = size;
		transform.localScale = new Vector3(size, 0.5f, size);
		soundPlayer = GetComponent<AudioSource>();
	}
	
	// play explosion sound and show particle effect
	[PunRPC]
	void explode(float t)
	{
		exploded = true;
		soundPlayer.PlayOneShot(explosionSound, t);
		Quaternion explosionRot = Quaternion.Euler(0, 0, 0);
		Vector3 explosionPos = transform.position;
		GameObject explosion = (GameObject)PhotonNetwork.Instantiate("Explosion", explosionPos, explosionRot, 0);
	}
	
	[PunRPC]
	void destroySelf(){
		PhotonNetwork.Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (exploded){
			time += Time.deltaTime;	
			// timer for sound and particle effect
			if (time > 2f){
				destroySelf();
			}
		}
	}
}
