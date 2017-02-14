using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {

    public Text text;
	// Use this for initialization
	void Start () {
        text= GameObject.Find("Text").GetComponent<Text>();
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    void updateWinText()
    {

    }
}
