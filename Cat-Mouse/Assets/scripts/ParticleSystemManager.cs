using UnityEngine;
using System.Collections;

public class ParticleSystemManager : MonoBehaviour {
	private ParticleSystem ps;


	public void Start() 
	{
		ps = GetComponent<ParticleSystem>();
	}

	public void Update() 
	{
		if(ps)
		{
			if(!ps.IsAlive())
			{
				PhotonNetwork.Destroy(this.gameObject);
			}
		}
	}
}
