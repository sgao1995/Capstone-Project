using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {
	public GameObject musicObject;
	public AudioClip music;
	public AudioSource musicPlayer;
	public bool fadeOut = false;
	public float fadeOutDuration = 0.5f;
	
	void Awake(){
		musicObject = GameObject.Find("audBGM");
		musicObject = this.gameObject;
		musicPlayer = GetComponent<AudioSource>(); 
		musicPlayer.clip = music;
		musicPlayer.Play();
		DontDestroyOnLoad(musicObject);
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
