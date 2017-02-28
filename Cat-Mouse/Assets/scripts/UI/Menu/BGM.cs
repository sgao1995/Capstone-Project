using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {
	public GameObject musicObject;
	public AudioClip music;
	public AudioSource musicPlayer;
	public bool fadeOut = false;
	public float fadeOutDuration = 0.5f;
	
	// makes sure only 1 audio player is playing at once
	void Awake(){
		DontDestroyOnLoad(musicObject);
		// if there is already an audio player then destroy it
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
		// otherwise initiate it
		else{
			musicObject = GameObject.Find("audBGM");
			musicObject = this.gameObject;
			musicPlayer = GetComponent<AudioSource>(); 
			musicPlayer.clip = music;
			musicPlayer.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeOut){
			musicPlayer.volume = fadeOutDuration;
			fadeOutDuration -= 0.01f;
			if (fadeOutDuration <= 0f){
				Destroy(this.gameObject);
			}
		}
	}
}
