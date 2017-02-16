using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {
    public Text timerTxt;
    public float sTime;
    float timer;
    // Use this for initialization
    void Start () {
        timerTxt = GameObject.Find("Timer").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.RoundToInt(timer % 60);
        string time=string.Format("{0:00}:{1:00}", minutes, seconds);
      
        timerTxt.text = "Match time: "+ time;
        }
}
