using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    //prefabs - for Room and the Bridge
    public List<GameObject> roomBase = new();

    public bool keyFound = false;


    public GameObject roomBaseOneOpen;
    public GameObject roomBaseTwoOpen;
    public GameObject roomBaseTwoOpenOpp;
    public GameObject roomBaseThreeOpen;
    public GameObject roomBaseFourOpen;

    public TileRoom RoomPrefab;
    public GameObject BridgePrefab;
    public int level;
    //Adjustable properties
    public int targetNumberOfRooms;
    public float timeToWait;
    public float spaceBetween;

    public Minimap miniMap;
    public TextMeshProUGUI Text;
    //Lists that keep track of Rooms and room like things :)
    public List<TileRoom> AllRooms = new(); //All our created rooms exist here
    public List<Vector2> PossibleRooms = new(); //Store grid position of potential spaces that rooms could spawn
    public List<TileRoom> OpenRooms = new(); //Any room that has at least 3 openings
    public List<GameObject> AllBridges = new(); //will store all the bridges that we create

    public List<GameObject> allEnemies = new();
    public List<GameObject> allItems = new();
    public GameObject KeyPREFAB;

    public List<GameObject> StartRoom = new();
    public List<GameObject> EndRoom = new();
    public List<GameObject> RandomRoom = new();

    public int currentRoom;

    public int GridToIndex(int x, int y)
    {
        for (int i = 0; i < AllRooms.Count; i++)
        {
            if (AllRooms[i].x == x && AllRooms[i].y == y)
                return i;
        }
        return 0;
    }
    // Start is called before the first frame update
    void Start()
    {



        StartCoroutine(BuildRandomMap());
    }

    //Coroutine to allow us to see the map being built
    IEnumerator BuildRandomMap()
    {
        level = 1;
        currentRoom = 0;
        //create initial room 
        spawnRoom(new Vector2(0, 0), RoomPrefab);
        //and initial layout
        while (AllRooms.Count < targetNumberOfRooms)
        {
            if (PossibleRooms.Count > 0)
            {
                int index = Random.Range(0, PossibleRooms.Count);
                Vector2 newRoomPosition = PossibleRooms[index];
                PossibleRooms.RemoveAt(index);
                if (AllRooms.Count < targetNumberOfRooms)
                    spawnRoom(newRoomPosition, RoomPrefab);
                else spawnRoom(newRoomPosition, RoomPrefab);

                yield return new WaitForSeconds(timeToWait);
            }
            else
            {
                Debug.LogError("Possible rooms ran out");
            }
        }

        closeOffDoors();
        closeExtra();

        foreach (TileRoom r in AllRooms)
        {
            //parent inner based on open doors
            int numOpen = 0;
            foreach (bool b in r.openDoors) if (b) numOpen++;
            GameObject roomBase;
            Door[] doors = new Door[0];
            switch (numOpen)
            {
                case 1:
                    roomBase = Instantiate(roomBaseOneOpen);
                    roomBase.transform.parent = r.transform;
                    roomBase.transform.localPosition = new Vector3(0, 0, 0);
                    //determine rotation
                    int openIndex = 0;
                    for (int i = 0; i < r.openDoors.Count; i++)
                    {
                        if (r.openDoors[i]) openIndex = i;
                    }

                    if (openIndex != -1)
                    {
                        doors = roomBase.GetComponentsInChildren<Door>();
                        foreach (Door door in doors)
                        {
                            door.FacingDirection += openIndex;
                            if (door.FacingDirection > 3) door.FacingDirection -= 4;
                        }
                        roomBase.transform.eulerAngles = new Vector3(0, 0, -90 * openIndex);
                    }
                    roomBase.name = "1";
                    break;
                case 2:


                    int Index1 = -1;
                    int Index2 = -1;
                    for (int i = 0; i < r.openDoors.Count; i++)
                    {
                        if (r.openDoors[i])
                        {
                            if (Index1 == -1) Index1 = i;
                            else Index2 = i;
                        }
                    }

                    if (Index2 + Index1 == 2 || Index1 + Index2 == 4)
                    {
                        //opp
                        roomBase = Instantiate(roomBaseTwoOpenOpp);
                        roomBase.transform.parent = r.transform;
                        roomBase.transform.localPosition = new Vector3(0, 0, 0);
                        doors = roomBase.GetComponentsInChildren<Door>();


                        if (Index1 + Index2 != 2)
                        {
                            roomBase.transform.eulerAngles = new Vector3(0, 0, -90);
                            foreach (Door door in doors)
                                door.FacingDirection++;
                        }

                    }
                    else
                    {
                        roomBase = Instantiate(roomBaseTwoOpen);
                        roomBase.transform.parent = r.transform;
                        roomBase.transform.localPosition = new Vector3(0, 0, 0);
                        int numRot = 0;
                        if (Index1 == 0 && Index2 == 3) numRot = 3;
                        else numRot = Index2 - 1;
                        doors = roomBase.GetComponentsInChildren<Door>();
                        foreach (Door door in doors)
                        {
                            door.FacingDirection += numRot;
                            if (door.FacingDirection > 3) door.FacingDirection -= 4;

                        }
                        roomBase.transform.eulerAngles = new Vector3(0, 0, -90 * numRot);
                        roomBase.name = "2 " + numRot;

                    }
                    break;
                case 3:
                    int lastIndex = -1;
                    for (int i = 0; i < r.openDoors.Count; i++)
                    {
                        if (!r.openDoors[i])
                            lastIndex = i;
                    }
                    roomBase = Instantiate(roomBaseThreeOpen);

                    roomBase.transform.parent = r.transform;
                    roomBase.transform.localPosition = new Vector3(0, 0, 0);
                    int numTurns = 0;
                    if (lastIndex == 2)
                    {
                        numTurns = 0;
                    }
                    else if (lastIndex == 3)
                    {
                        numTurns = 1;
                    }
                    else if (lastIndex == 0)
                    {
                        numTurns = 2;
                    }
                    else numTurns = 3;
                    roomBase.transform.eulerAngles = new Vector3(0, 0, -90 * numTurns);
                    doors = roomBase.GetComponentsInChildren<Door>();
                    foreach (Door door in doors)
                    {
                        door.FacingDirection += numTurns;
                        if (door.FacingDirection > 3) door.FacingDirection -= 4;

                    }
                    roomBase.name = "3";

                    break;
                case 4:
                    roomBase = Instantiate(roomBaseFourOpen);
                    roomBase.transform.parent = r.transform;
                    roomBase.transform.localPosition = new Vector3(0, 0, 0);
                    roomBase.name = "4";
                    break;
            }
        }

        //assign start room inner
        GameObject sRoom = Instantiate(StartRoom[0]);
        sRoom.transform.parent = AllRooms[0].transform;
        sRoom.transform.localPosition = new Vector3(0, 0, 0);

        //get all spawn points in current level
        List<SpawnLocation> allSpawnLocations = GetComponentsInChildren<SpawnLocation>().ToList();

        //create key
        GameObject key = Instantiate(KeyPREFAB);
        int keyIndex = Random.Range(0, allSpawnLocations.Count);
        key.name = "Key";
        key.transform.parent = allSpawnLocations[keyIndex].transform;
        key.transform.localPosition = new Vector3(0, 0, 0);
        allSpawnLocations.RemoveAt(keyIndex);

        //create enemies
        foreach (SpawnLocation location in allSpawnLocations)
        {
            //spawn random enemey or item
            GameObject enemy = Instantiate(allEnemies[Random.Range(0, allEnemies.Count)]);
            enemy.transform.parent = location.transform;
            enemy.transform.localPosition = new Vector3(0, 0, 0);
            enemy.transform.parent = location.transform.parent.transform;
            Destroy(location.gameObject, .01f);
        }


        //if we have spacee between the room, then place bridges there
        BuildBridges();
        if (EventManager.instance != null)
            EventManager.TriggerEvent("LevelLoadDone", null);
        miniMap = FindObjectOfType<Minimap>();
        Text.text = "Floor " + GameManager.instance.currentFloor.ToString();
    }

    bool attemptCloseDoor(TileRoom room, Vector2 targetDirection, int doorIndex)
    {
        int targetIndex;
        targetIndex = AllRooms.FindIndex(i => i.x == targetDirection.x && i.y == targetDirection.y);

        if (targetIndex < 0)
        {
            //close door
            room.openDoors[doorIndex] = false;

            return false;
        }
        else return true;
    }


    void closeOffDoors()
    {
        //now close off all doors that do not load to anything
        foreach (TileRoom room in AllRooms)
        {
            Vector2 topDirection = new Vector2(room.x, room.y) + new Vector2(0, +1);
            Vector2 bottomDirection = new Vector2(room.x, room.y) + new Vector2(0, -1);
            Vector2 leftDirection = new Vector2(room.x, room.y) + new Vector2(-1, 0);
            Vector2 rightDirection = new Vector2(room.x, room.y) + new Vector2(1, 0);

            int openings = 0;

            if (attemptCloseDoor(room, topDirection, 0)) openings++;
            if (attemptCloseDoor(room, bottomDirection, 2)) openings++;
            if (attemptCloseDoor(room, leftDirection, 3)) openings++;
            if (attemptCloseDoor(room, rightDirection, 1)) openings++;

            if (openings >= 3) OpenRooms.Add(room);

        }
    }


    bool DFS2(TileRoom targetRoom, int doorToCloseIndex)
    {

        Stack<TileRoom> currentPath = new Stack<TileRoom>();
        int otherDoorIndex = 0;
        TileRoom otherRoom = null;
        TileRoom r = targetRoom;

        currentPath.Push(r);

        bool found = false;

        int escape = 0;

        //close door
        targetRoom.openDoors[doorToCloseIndex] = false;

        Vector2 topDirection = new Vector2(targetRoom.x, targetRoom.y) + new Vector2(0, 1);
        Vector2 bottomDirection = new Vector2(targetRoom.x, targetRoom.y) + new Vector2(0, -1);
        Vector2 leftDirection = new Vector2(targetRoom.x, targetRoom.y) + new Vector2(-1, 0);
        Vector2 rightDirection = new Vector2(targetRoom.x, targetRoom.y) + new Vector2(1, 0);

        int index = 0;
        switch (doorToCloseIndex)
        {
            case 0:
                index = AllRooms.FindIndex(i => i.x == topDirection.x && i.y == topDirection.y);
                if (index >= 0)
                {
                    //AllRooms[index].openDoors.Remove(AllRooms[index].bottomDoor);
                    AllRooms[index].openDoors[2] = false;
                    otherDoorIndex = 2;
                    otherRoom = AllRooms[index];
                    AllRooms[index].openDoors[2] = false;
                }
                break;
            case 2:
                index = AllRooms.FindIndex(i => i.x == bottomDirection.x && i.y == bottomDirection.y);
                if (index >= 0)
                {
                    AllRooms[index].openDoors[0] = false;
                    otherDoorIndex = 0;
                    otherRoom = AllRooms[index];
                    AllRooms[index].openDoors[0] = false;
                }
                break;
            case 3:
                index = AllRooms.FindIndex(i => i.x == leftDirection.x && i.y == leftDirection.y);
                if (index >= 0)
                {
                    AllRooms[index].openDoors[1] = false;
                    otherDoorIndex = 1;
                    otherRoom = AllRooms[index];
                    AllRooms[index].openDoors[1] = false;
                }
                break;
            case 1:
                index = AllRooms.FindIndex(i => i.x == rightDirection.x && i.y == rightDirection.y);
                if (index >= 0)
                {
                    AllRooms[index].openDoors[3] = false;
                    otherDoorIndex = 3;
                    otherRoom = AllRooms[index];
                    AllRooms[index].openDoors[3] = false;
                }
                break;
        }

        int NumberOfRooms = 0;
        int tIndex = -1;
        List<TileRoom> DiscoveredRoom = new List<TileRoom>();
        DiscoveredRoom.Add(targetRoom);
        while (currentPath.Count > 0)
        {
            escape++;
            if (escape >= 1000) break;

            TileRoom tRoom = currentPath.Pop();


            Vector2 tDirection = new Vector2(tRoom.x, tRoom.y) + new Vector2(0, 1);
            Vector2 bDirection = new Vector2(tRoom.x, tRoom.y) + new Vector2(0, -1);
            Vector2 lDirection = new Vector2(tRoom.x, tRoom.y) + new Vector2(-1, 0);
            Vector2 rDirection = new Vector2(tRoom.x, tRoom.y) + new Vector2(1, 0);



            tIndex = AllRooms.FindIndex(i => i.x == tDirection.x && i.y == tDirection.y);
            if (tIndex >= 0 && tRoom.openDoors[0] && !DiscoveredRoom.Contains(AllRooms[tIndex]))
            {
                DiscoveredRoom.Add(AllRooms[tIndex]);
                currentPath.Push(AllRooms[tIndex]);

            }

            tIndex = AllRooms.FindIndex(i => i.x == bDirection.x && i.y == bDirection.y);
            if (tIndex >= 0 && tRoom.openDoors[2] && !DiscoveredRoom.Contains(AllRooms[tIndex]))
            {
                DiscoveredRoom.Add(AllRooms[tIndex]);
                currentPath.Push(AllRooms[tIndex]);

            }


            tIndex = AllRooms.FindIndex(i => i.x == rDirection.x && i.y == rDirection.y);
            if (tIndex >= 0 && tRoom.openDoors[1] && !DiscoveredRoom.Contains(AllRooms[tIndex]))
            {
                DiscoveredRoom.Add(AllRooms[tIndex]);
                currentPath.Push(AllRooms[tIndex]);

            }


            tIndex = AllRooms.FindIndex(i => i.x == lDirection.x && i.y == lDirection.y);
            if (tIndex >= 0 && tRoom.openDoors[3] && !DiscoveredRoom.Contains(AllRooms[tIndex]))
            {
                DiscoveredRoom.Add(AllRooms[tIndex]);
                currentPath.Push(AllRooms[tIndex]);

            }

        }


        if (DiscoveredRoom.Count == AllRooms.Count) found = true;

        if (!found)
        {
            //restore
            otherRoom.openDoors[otherDoorIndex] = true;
            targetRoom.openDoors[doorToCloseIndex] = true;
        }

        return found;
    }

    bool bridgePossible(Vector2 pos)
    {
        bool canBuild = true;
        foreach (GameObject g in AllBridges)
        {
            if ((Vector2)g.transform.position == pos)
            {
                canBuild = false;
                break;
            }
        }
        return canBuild;
    }

    void createBridge(Vector2 pos, string id, string name, Vector2 bridgePos)
    {
        GameObject b = Instantiate<GameObject>(BridgePrefab);
        b.transform.position = pos;
        if (name == "Top" || name == "Bottom")
            b.transform.eulerAngles = new Vector3(0, 0, 90);

        b.GetComponent<Bridge>().pos = bridgePos;

        b.transform.localScale = new Vector3(spaceBetween, b.transform.localScale.y, 1);

        b.transform.parent = this.transform;
        b.name = "Bridge for " + id + " " + name;

        AllBridges.Add(b);
    }

    void BuildBridges()
    {
        foreach (TileRoom room in AllRooms)
        {

            Vector2 Top = (Vector2)room.transform.position + new Vector2(0, (room.size.y + spaceBetween) * .5f);
            Vector2 Bottom = (Vector2)room.transform.position + new Vector2(0, -(room.size.y + spaceBetween) * .5f);
            Vector2 Left = (Vector2)room.transform.position + new Vector2(-(room.size.x + spaceBetween) * .5f, 0);
            Vector2 Right = (Vector2)room.transform.position + new Vector2((room.size.x + spaceBetween) * .5f, 0);

            for (int i = 0; i < room.openDoors.Count; i++)
            {
                if (room.openDoors[i])
                {
                    switch (i)
                    {
                        case 0:
                            if (bridgePossible(Top)) createBridge(Top, room.name, "Top", new Vector2(room.x, room.y + .5f));
                            break;
                        case 2:
                            if (bridgePossible(Bottom)) createBridge(Bottom, room.name, "Bottom", new Vector2(room.x, room.y - .5f));
                            break;
                        case 3:
                            if (bridgePossible(Left)) createBridge(Left, room.name, "Left", new Vector2(room.x - .5f, room.y));
                            break;
                        case 1:
                            if (bridgePossible(Right)) createBridge(Right, room.name, "Right", new Vector2(room.x + .5f, room.y));
                            break;
                    }
                }
            }




        }
    }



    void closeExtra()
    {
        while (OpenRooms.Count > 0)
        {

            TileRoom room = OpenRooms[(int)Random.Range(0, OpenRooms.Count)]; //(int)Random.Range(0, openDoors.Count)

            Vector2 topDirection = new Vector2(room.x, room.y) + new Vector2(0, 1);
            Vector2 bottomDirection = new Vector2(room.x, room.y) + new Vector2(0, -1);
            Vector2 leftDirection = new Vector2(room.x, room.y) + new Vector2(-1, 0);
            Vector2 rightDirection = new Vector2(room.x, room.y) + new Vector2(1, 0);
            int index = 0;
            //determine what doors are open and randomly close one
            List<int> openDoorIndex = new List<int>();

            for (int i = 0; i < room.openDoors.Count; i++)
            {
                if (room.openDoors[i]) openDoorIndex.Add(i);
            }


            if (openDoorIndex.Count > 1)
            {
                int doorToCloseIndex = openDoorIndex[Random.Range(0, openDoorIndex.Count)];

                //use path finding to ensure path to start room is accessible
                //double check that room that this connects too has another opening
                DFS2(room, doorToCloseIndex);


            }

            //remove from open rooms
            OpenRooms.Remove(room);

        }

    }


    void addItems()
    {

    }

    void addEnemies()
    {

    }

    void addKey()
    {

    }

    void spawnRoom(Vector2 pos, TileRoom RoomPREFAB)
    {
        TileRoom r = Instantiate<TileRoom>(RoomPREFAB); //RoomPrefab
        r.transform.parent = this.transform;
        r.transform.localPosition = pos * new Vector2(12 + spaceBetween, 12 + spaceBetween);
        r.pos = r.transform.localPosition;




        //r.setupDoors();
        //r.setupWalls();


        r.x = (int)pos.x;
        r.y = (int)pos.y;
        r.name = "[" + r.x + "," + r.y + "]";

        //add floorPlan
        /*
        RoomMovement fPlan = Instantiate<RoomMovement>(floorPlan);
        fPlan.transform.parent = r.transform;
        fPlan.transform.localPosition = new Vector3(0, 0, 0);
        r.floorTiles = fPlan;
        */

        //if not beginning or end room add room
        if (AllRooms.Count > 0 && AllRooms.Count < targetNumberOfRooms - 1)
        {
            int randomIndex = Random.Range(0, RandomRoom.Count);
            GameObject roomInterior = Instantiate(RandomRoom[randomIndex]);
            roomInterior.transform.parent = r.transform;
            roomInterior.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (AllRooms.Count == targetNumberOfRooms - 1)
        {
            int randomIndex = Random.Range(0, EndRoom.Count);
            GameObject roomInterior = Instantiate(EndRoom[randomIndex]);
            roomInterior.transform.parent = r.transform;
            roomInterior.transform.localPosition = new Vector3(0, 0, 0);
        }

        AllRooms.Add(r);

        Vector2 topPosition = new Vector2(r.x, r.y) + new Vector2(0, 1);
        Vector2 bottomPosition = new Vector2(r.x, r.y) + new Vector2(0, -1);
        Vector2 leftPosition = new Vector2(r.x, r.y) + new Vector2(-1, 0);
        Vector2 rightPosition = new Vector2(r.x, r.y) + new Vector2(1, 0);

        //determine possible other spawn locations and
        // add them to our list of possible rooms
        if (AllRooms.FindIndex(i => i.x == topPosition.x && i.y == topPosition.y) < 0 && !PossibleRooms.Contains(topPosition))
            PossibleRooms.Add(topPosition);


        if (AllRooms.FindIndex(i => i.x == bottomPosition.x && i.y == bottomPosition.y) < 0 && !PossibleRooms.Contains(bottomPosition))
            PossibleRooms.Add(bottomPosition);


        if (AllRooms.FindIndex(i => i.x == rightPosition.x && i.y == rightPosition.y) < 0 && !PossibleRooms.Contains(rightPosition))
            PossibleRooms.Add(rightPosition);


        if (AllRooms.FindIndex(i => i.x == leftPosition.x && i.y == leftPosition.y) < 0 && !PossibleRooms.Contains(leftPosition))
            PossibleRooms.Add(leftPosition);
    }

    void SetupListener()
    {
        EventManager.StartListening("Pickup", Handle_Pickup);


    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.StopListening("Pickup", Handle_Pickup);


        }
    }


    public void Handle_Pickup(Dictionary<string, object> message)
    {
        if ((string)message["value"] == "Key")
        {
            keyFound = true;
        }

    }

}
