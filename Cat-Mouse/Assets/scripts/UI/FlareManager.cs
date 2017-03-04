using UnityEngine;
using UnityEngine.UI;

public class FlareManager : MonoBehaviour {
	public GameObject[] mouseArray;
	public GameObject myMouse;

	public void DangerSignalClicked(){
		mouseArray= GameObject.FindGameObjectsWithTag("Mouse");
		for (int i = 0; i < mouseArray.Length; i++){
			if (mouseArray[i].transform.GetComponent<PhotonView>().isMine){
				myMouse = mouseArray[i];
			}
		}
		myMouse.transform.GetComponent<MouseMovement>().SendMessage("CastFlare", 1);
		myMouse.transform.GetComponent<MouseMovement>().SendMessage("HideMiniMenu");
	}
	public void AssistSignalClicked(){
		mouseArray= GameObject.FindGameObjectsWithTag("Mouse");
		for (int i = 0; i < mouseArray.Length; i++){
			if (mouseArray[i].transform.GetComponent<PhotonView>().isMine){
				myMouse = mouseArray[i];
			}
		}
		myMouse.transform.GetComponent<MouseMovement>().SendMessage("CastFlare", 2);
		myMouse.transform.GetComponent<MouseMovement>().SendMessage("HideMiniMenu");
	}
}
