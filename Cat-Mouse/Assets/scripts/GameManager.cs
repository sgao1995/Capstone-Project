using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Photon.PunBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;
    SpawnC[] sc;
    SpawnM[] sm;
    List<int> allPuzzleTypes = new List<int>();
    List<int> activePuzzleTypes = new List<int>();
    public BGM music;
    // monsters
    List<MonsterSpawn> monsterSpawnList = new List<MonsterSpawn>(); public MonsterSpawn mSpawn;
    public int numMonsters = 0;
    public float delayUntilSpawn = 10f;
    public bool startSpawnCountdown = false;
    // powerups
    private List<Powerup> powerupList = new List<Powerup>();
    // puzzle room cleared
    private bool ballRoomCleared = false;
    private bool bossRoomCleared = false;
    private GameObject[] ballArray;
    private GameObject[] targetArray;

    private void Start()
    {
        music = GameObject.Find("audBGM").GetComponent<BGM>();
        music.fadeOut = true;
        //s = GameObject.FindObjectsOfType<Spawn>();
        sc = GameObject.FindObjectsOfType<SpawnC>();
        sm = GameObject.FindObjectsOfType<SpawnM>();
        PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.automaticallySyncScene = true;
        // create the monster spawn locations
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector3 spawnPos = new Vector3(40f - i * 20f, 0, 40f - j * 20f);
                Quaternion spawnRot = new Quaternion(0f, 0f, 0f, 0f);
                MonsterSpawn newSpawn = Instantiate(mSpawn) as MonsterSpawn;
                mSpawn.transform.position = spawnPos;
                mSpawn.transform.rotation = spawnRot;
                monsterSpawnList.Add(newSpawn);
            }
        }

        for (int p = 0; p < 6; p++)
        {
            allPuzzleTypes.Add(p);
        }
        for (int p = 0; p < 3; p++)
        {
            int getPuzzle = Random.Range(0, allPuzzleTypes.Count);
            activePuzzleTypes.Add(allPuzzleTypes[getPuzzle]);
            allPuzzleTypes.RemoveAt(getPuzzle);
        }
        SpawnMaze();
        if (GameObject.Find("TeamSelectionOBJ").GetComponent<teamselectiondata>().playertype == 0)
        {
            SpawnCat();
        }
        else
        {
            SpawnMouse();
        }
        // spawn basic monsters and elite monsters
        for (int i = 0; i < monsterSpawnList.Count; i++)
        {
            SpawnMonsters(i);
        }
        // spawn boss monster
        SpawnBoss();

        SpawnKeysAndChests();
        // spawn 5 powerups
        for (int i = 0; i < 5; i++)
        {
            SpawnPowerup();
        }

        // if applicable, get ball/target lists
        if (activePuzzleTypes.IndexOf(3) != -1)
        {
            ballArray = GameObject.FindGameObjectsWithTag("Ball");
            targetArray = GameObject.FindGameObjectsWithTag("Target");
        }

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
            /*List<int> tempTypes = new List<int>();
            tempTypes.Add(5);
            tempTypes.Add(3);
            tempTypes.Add(2);
            mazeInstance.GeneratePuzzles(tempTypes);
            Debug.Log(tempTypes[0] + " " + tempTypes[1] + " " + tempTypes[2]);
			activePuzzleTypes = tempTypes;*/
            mazeInstance.GenerateChestLocations();
            mazeInstance.GeneratePuzzles(activePuzzleTypes);
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

    void SpawnMonsters(int spawn)
    {
        if (PhotonNetwork.isMasterClient)
        {
            // majority will be weaker ones upon initial spawn, but 
            // will get stronger spawns as game goes on
            MonsterSpawn monsterSpawn = monsterSpawnList[spawn];
            int formation = Random.Range(0, 1000) + (int)Time.time;
            // 1 normal monster
            if (formation < 500)
            {
                GameObject monsterGO = (GameObject)PhotonNetwork.Instantiate("Monster", monsterSpawn.transform.position, monsterSpawn.transform.rotation, 0);
                monsterGO.GetComponent<MonsterAI>().enabled = true;
                MonsterAI monster = monsterGO.GetComponent<MonsterAI>();
                monster.setMonsterType("Monster");
                numMonsters++;
            }
            // 2 normal monsters
            else if (formation < 800)
            {
                for (int i = 0; i < 2; i++)
                {
                    GameObject monsterGO = (GameObject)PhotonNetwork.Instantiate("Monster", monsterSpawn.transform.position, monsterSpawn.transform.rotation, 0);
                    monsterGO.GetComponent<MonsterAI>().enabled = true;
                    MonsterAI monster = monsterGO.GetComponent<MonsterAI>();
                    monster.setMonsterType("Monster");
                    numMonsters++;
                }
            }
            // 1 normal monster and 1 elite monster
            else if (formation <= 1000)
            {
                GameObject monsterGO = (GameObject)PhotonNetwork.Instantiate("Monster", monsterSpawn.transform.position, monsterSpawn.transform.rotation, 0);
                monsterGO.GetComponent<MonsterAI>().enabled = true;
                MonsterAI monster = monsterGO.GetComponent<MonsterAI>();
                monster.setMonsterType("Monster");

                GameObject monsterGO2 = (GameObject)PhotonNetwork.Instantiate("MonsterElite", monsterSpawn.transform.position, monsterSpawn.transform.rotation, 0);
                monsterGO2.GetComponent<MonsterAI>().enabled = true;
                MonsterAI monster2 = monsterGO2.GetComponent<MonsterAI>();
                monster2.setMonsterType("MonsterElite");
                numMonsters += 2;
            }
            // 2 elite monsters
            else if (formation > 1000)
            {
                for (int i = 0; i < 2; i++)
                {
                    GameObject monsterGO = (GameObject)PhotonNetwork.Instantiate("MonsterElite", monsterSpawn.transform.position, monsterSpawn.transform.rotation, 0);
                    monsterGO.GetComponent<MonsterAI>().enabled = true;
                    MonsterAI monster = monsterGO.GetComponent<MonsterAI>();
                    monster.setMonsterType("MonsterElite");
                    numMonsters++;
                }
            }
        }
    }

    void SpawnBoss()
    {
        // pick a random spawn to spawn at
        int location = Random.Range(0, monsterSpawnList.Count);
        MonsterSpawn bossSpawn = monsterSpawnList[location];
        GameObject monsterGO = (GameObject)PhotonNetwork.Instantiate("Boss", bossSpawn.transform.position, bossSpawn.transform.rotation, 0);
        monsterGO.GetComponent<MonsterAI>().enabled = true;
        MonsterAI monster = monsterGO.GetComponent<MonsterAI>();
        monster.setMonsterType("Boss");
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
                // if the puzzle room is either the ball/target room or the boss room
                // then dont spawn the keys until the room is passed
                if (activePuzzleTypes[i / 2] == 3 || activePuzzleTypes[i / 2] == 5)
                {
                }
                else
                {
                    Vector3 keyPos = new Vector3(keyLocations[i], 1, keyLocations[i + 1]);
                    Quaternion keyRot = new Quaternion(0f, 0f, 0f, 0f);
                    PhotonNetwork.Instantiate("Key", keyPos, keyRot, 0);
                }
                Vector3 chestPos = new Vector3(chestLocations[i], 0.35f, chestLocations[i + 1]);
                Quaternion chestRot = new Quaternion(0f, 0f, 0f, 0f);
                GameObject chest = (GameObject)PhotonNetwork.Instantiate("Chest", chestPos, chestRot, 0);
                Chest newChest = chest.GetComponent<Chest>();
                newChest.whichPieceInside = (i / 2) + 1;
            }
        }
    }

    // spawn powerups
    void SpawnPowerup()
    {
        if (PhotonNetwork.isMasterClient)
        {
            // need to add 0.5 or else they spawn on edges
            Vector3 spawnPos = new Vector3(0.5f + Random.Range(-45, 46), 0.5f, 0.5f + Random.Range(-45, 46));
            Quaternion spawnRot = new Quaternion(0f, 0f, 0f, 0f);
            GameObject newGO = (GameObject)PhotonNetwork.Instantiate("Powerup", spawnPos, spawnRot, 0);
            Powerup newPowerup = newGO.GetComponent<Powerup>();
            newPowerup.setType(Random.Range(0, 4));
            powerupList.Add(newPowerup);
        }
    }

    public void decreaseMonsterCount()
    {
        numMonsters--;
    }

    // called when the room is cleared, and spawns the key
    // we also call this from MonsterAI, when the puzzle room boss is killed
    public void ClearedRoomSoSpawnKey(int whichRoom)
    {
        List<float> keyLocations = mazeInstance.getKeySpawns();
        int whichPuzzle = activePuzzleTypes.IndexOf(whichRoom) * 2;
        Vector3 keyPos = new Vector3(keyLocations[whichPuzzle], 1, keyLocations[whichPuzzle + 1]);
        Quaternion keyRot = new Quaternion(0f, 0f, 0f, 0f);
        PhotonNetwork.Instantiate("Key", keyPos, keyRot, 0);
    }

    void Update()
    {
        // spawn additional monsters when they die, difficulty scaling with time
        if (numMonsters < 30 && startSpawnCountdown == false)
        {
            startSpawnCountdown = true;
            delayUntilSpawn = 10f;
        }
        if (startSpawnCountdown)
        {
            delayUntilSpawn -= Time.deltaTime;
        }
        if (delayUntilSpawn < 0 && startSpawnCountdown)
        {
            startSpawnCountdown = false;
            delayUntilSpawn = 10f;
            for (int i = 0; i < 5; i++)
            {
                int randSpawn = Random.Range(0, monsterSpawnList.Count);
                SpawnMonsters(randSpawn);
            }
        }
        // spawn additional powerups
        if (powerupList.Count < 5)
        {
            SpawnPowerup();
        }

        // if ball/target room is active then check to see if balls are on targets
        if (activePuzzleTypes.IndexOf(3) != -1)
        {
            if (ballRoomCleared == false)
            {
                bool allBallsOnTargets = true;
                for (int i = 0; i < ballArray.Length; i++)
                {
                    // if a ball is not on any of the targets then its false
                    if (Vector3.Distance(ballArray[i].transform.position, targetArray[0].transform.position) > 1f
                    && Vector3.Distance(ballArray[i].transform.position, targetArray[1].transform.position) > 1f
                    && Vector3.Distance(ballArray[i].transform.position, targetArray[2].transform.position) > 1f)
                    {
                        allBallsOnTargets = false;
                    }
                }
                if (allBallsOnTargets)
                {
                    ballRoomCleared = true;
                    ClearedRoomSoSpawnKey(3);
                }
            }
        }
    }

}