using UnityEngine;
using System.Collections;

public class SteelTrap : MonoBehaviour {
	public float timeActive;
	public float trapDuration = 5f;
	public bool trappedSomeone = false;
	private GameObject trappedPerson;
	
	// Use this for initialization
	void Start () {
		// lasts 300 seconds
		timeActive = 300f;
	}
	
	// Update is called once per frame
	void Update () {
		timeActive -= Time.deltaTime;
		
		if (trappedSomeone){
			trappedPerson.GetComponent<PhotonView>().RPC("ForcePositionChange", PhotonTargets.AllBuffered, new Vector3(transform.position.x, 0f, transform.position.z));	
			
			trapDuration -= Time.deltaTime;
		}
		if (timeActive <= 0f || trapDuration <= 0f){
				
				//trappedPerson.GetComponent<MonsterAI>().releaseFromTrap();
			PhotonNetwork.Destroy(this.gameObject);
		}
	}
	
	public void activate(GameObject trapThis){
		trappedSomeone = true;
		trappedPerson = trapThis;
	}	
}
