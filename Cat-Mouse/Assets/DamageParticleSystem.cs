using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageParticleSystem : MonoBehaviour {
	private ParticleSystem part;
	public List<ParticleCollisionEvent> collisionEvents;
	
	public void Start() 
	{
		part = GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
	}

	// destroy upon animation completion
	// also do damage to stuff in hitbox
	public void Update() 
	{
		if(part)
		{
			if(!part.IsAlive())
			{
				PhotonNetwork.Destroy(this.gameObject);
			}
		}
	}

	void OnParticleCollision(GameObject other)
    {
		Debug.Log(other.tag);
		if (other.tag == "Mouse"){
			other.GetComponent<MouseMovement>().SendMessage("TakeDamage", 3f);
		}
		else if (other.tag == "Cat"){
			other.GetComponent<CatMovement>().SendMessage("TakeDamage", 3f);
		}
    }
}
