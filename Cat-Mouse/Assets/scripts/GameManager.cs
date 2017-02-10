using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Photon.PunBehaviour {
    public Maze mazePrefab;
    private Maze mazeInstance;
    Spawn[] s;
    List<int> allPuzzleTypes = new List<int>();
    List<int> activePuzzleTypes = new List<int>();

    private	void Start () {
        s = GameObject.FindObjectsOfType<Spawn>();
        PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.automaticallySyncScene = true;
        for (int p = 0; p < 6; p++)
        {
            allPuzzleTypes.Add(p);
        }
        for (int p = 0; p < 6; p++)
        {
            int getPuzzle = Random.Range(0, allPuzzleTypes.Count);
            //	int getPuzzle = 1;
            activePuzzleTypes.Add(allPuzzleTypes[getPuzzle]);
            allPuzzleTypes.RemoveAt(getPuzzle);
            Debug.Log(activePuzzleTypes);
        }
        SpawnMaze();
        SpawnCat();
        SpawnMonsters();
    }
    void OnGUI()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            GUILayout.BeginArea(new Rect(10, 10, 100, 100));
            if (GUILayout.Button("EXITGAME"))
            {
                LeaveRoom();
            }
            GUILayout.EndArea();
        }
    }
    void OnLeftRoom()
    {
        SceneManager.LoadScene("lobby", LoadSceneMode.Single);
    }
    void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.isMessageQueueRunning = false;
    }
    void SpawnMaze()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        var mazeScript = mazeInstance.GetComponent<Maze>();
        if (mazeScript != null)
        {
            mazeScript.StartMazeCreation();
        }
		
		List<int> tempTypes = new List<int>();
		tempTypes.Add(0);		
		tempTypes.Add(1);		
		tempTypes.Add(2);
		mazeInstance.GeneratePuzzles(tempTypes);
		Debug.Log(tempTypes[0] + " " + tempTypes[1] + " " + tempTypes[2]);
        //mazeInstance.GeneratePuzzles(activePuzzleTypes);
    }
    void SpawnCat()
    {

        Spawn mys = s[Random.Range(0, s.Length)];
        GameObject myCat = (GameObject)PhotonNetwork.Instantiate("Cat_Test", mys.transform.position, mys.transform.rotation, 0);
        myCat.GetComponent<CatMovement>().enabled = true;
        myCat.transform.FindChild("CatCam").gameObject.SetActive(true);
        //myCat.GetComponent<NetworkPlayer>().enabled = false;
        //enables minimap:
        myCat.GetComponent<Minimap>().enabled = true;
    }
    void SpawnMonsters()
    {
        Spawn monsterSpawn = s[1];
        GameObject monster = (GameObject)PhotonNetwork.Instantiate("Monster", monsterSpawn.transform.position, monsterSpawn.transform.rotation, 0);
        monster.GetComponent<MonsterAI>().enabled = true;
    }

}