using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public Maze mazePrefab;
    private Maze mazeInstance;
    Spawn[] s;
    List<int> allPuzzleTypes = new List<int>();
    List<int> activePuzzleTypes = new List<int>();

    private	void Start () {
        s = GameObject.FindObjectsOfType<Spawn>();
        PhotonNetwork.isMessageQueueRunning = true;
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

    void SpawnMaze()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        var mazeScript = mazeInstance.GetComponent<Maze>();
        if (mazeScript != null)
        {
            mazeScript.StartMazeCreation();
        }
        mazeInstance.GeneratePuzzles(activePuzzleTypes);
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