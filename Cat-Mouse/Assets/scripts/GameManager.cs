using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Photon.PunBehaviour {
    public Maze mazePrefab;
    private Maze mazeInstance;
    Spawn[] s;
    SpawnC[] sc;
    SpawnM[] sm;
    List<int> allPuzzleTypes = new List<int>();
    List<int> activePuzzleTypes = new List<int>();
    

    private	void Start () {
        s = GameObject.FindObjectsOfType<Spawn>();
        sc = GameObject.FindObjectsOfType<SpawnC>();
        sm = GameObject.FindObjectsOfType<SpawnM>();
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
        if(GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype == 0)
        {
            SpawnCat();
        }else
        {
            SpawnMouse();
        }
        for (int i = 0; i<9; i++)
        {
            SpawnMonsters(i);
        }
		SpawnKeysAndChests();
        GameObject.Find("Timer").GetComponent<Timer>().enabled = true;
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
        if (PhotonNetwork.isMasterClient)
        {
            List<int> tempTypes = new List<int>();
            tempTypes.Add(4);
            tempTypes.Add(3);
            tempTypes.Add(2);
            mazeInstance.GeneratePuzzles(tempTypes);
            Debug.Log(tempTypes[0] + " " + tempTypes[1] + " " + tempTypes[2]);
            mazeInstance.GenerateChestLocations();
            //mazeInstance.GeneratePuzzles(activePuzzleTypes);
        }
    }
    void SpawnCat()
    {
        SpawnC mys = sc[0];
        GameObject myCat = (GameObject)PhotonNetwork.Instantiate("Cat", mys.transform.position, mys.transform.rotation, 0);
        myCat.GetComponent<CatMovement>().enabled = true;
        myCat.transform.FindChild("CatCam").gameObject.SetActive(true);
        //myCat.GetComponent<NetworkPlayer>().enabled = false;
        //enables minimap:
        myCat.GetComponent<Minimap>().enabled = true;
    }

    void SpawnMouse()
    {
        SpawnM mys = sm[Random.Range(0, 2)];
        GameObject myMouse = (GameObject)PhotonNetwork.Instantiate("Mouse", mys.transform.position, mys.transform.rotation, 0);
        myMouse.GetComponent<MouseMovement>().enabled = true;
        myMouse.transform.FindChild("MouseCam").gameObject.SetActive(true);
        //myMouse.GetComponent<NetworkPlayer>().enabled = false;
        //enables minimap:
        myMouse.GetComponent<Minimap>().enabled = true;
    }

    void SpawnMonsters(int i)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Spawn monsterSpawn = s[i];
            GameObject monsterGO = (GameObject)PhotonNetwork.Instantiate("Monster", monsterSpawn.transform.position, monsterSpawn.transform.rotation, 0);
            monsterGO.GetComponent<MonsterAI>().enabled = true;
			MonsterAI monster = monsterGO.GetComponent<MonsterAI>();
			monster.setMonsterType("Monster");
        }
    }
	// spawn the keys and chests in the puzzle rooms
	void SpawnKeysAndChests()
	{
		// spawn each key and chest
        if (PhotonNetwork.isMasterClient)
        {
            List<float> keyLocations = mazeInstance.getKeySpawns();
            List<float> chestLocations = mazeInstance.getChestSpawns();
            for (int i = 0; i < 6; i += 2)
            {
                Vector3 keyPos = new Vector3(keyLocations[i], 1, keyLocations[i + 1]);
                Quaternion keyRot = new Quaternion(0f, 0f, 0f, 0f);
                GameObject key = (GameObject)PhotonNetwork.Instantiate("Key", keyPos, keyRot, 0);
                Vector3 chestPos = new Vector3(chestLocations[i], 0.35f, chestLocations[i + 1]);
                Quaternion chestRot = new Quaternion(0f, 0f, 0f, 0f);
                GameObject chest = (GameObject)PhotonNetwork.Instantiate("Chest", chestPos, chestRot, 0);
				Chest newChest = chest.GetComponent<Chest>();
				newChest.whichPieceInside = (i/2)+1;
            }
        }
	}
  
}