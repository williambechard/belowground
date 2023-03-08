using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    //prefabs - for Room and the Bridge
    public Room RoomPrefab;
    public GameObject BridgePrefab;

    //Adjustable properties
    public int targetNumberOfRooms;
    public float timeToWait;
    public float spaceBetween;

    //Lists that keep track of Rooms and room like things :)
    public List<Room> AllRooms = new(); //All our created rooms exist here
    public List<Vector2> PossibleRooms = new(); //Store grid position of potential spaces that rooms could spawn
    public List<Room> OpenRooms = new(); //Any room that has at least 3 openings
    public List<GameObject> AllBridges = new(); //will store all the bridges that we create



    // Start is called before the first frame update
    void Start() => StartCoroutine(BuildRandomMap());


    //Coroutine to allow us to see the map being built
    IEnumerator BuildRandomMap()
    {
        //create initial room 
        spawnRoom(new Vector2(0, 0));
        //and initial layout
        while (AllRooms.Count < targetNumberOfRooms)
        {
            if (PossibleRooms.Count > 0)
            {
                int index = Random.Range(0, PossibleRooms.Count);
                Vector2 newRoomPosition = PossibleRooms[index];
                PossibleRooms.RemoveAt(index);
                spawnRoom(newRoomPosition);

                yield return new WaitForSeconds(timeToWait);
            }
            else
            {
                Debug.LogError("Possible rooms ran out");
            }
        }

        closeOffDoors();
        closeExtra();

        //if we have spacee between the room, then place bridges there
        if (spaceBetween > 0) BuildBridges();
        if (EventManager.instance != null)
            EventManager.TriggerEvent("LevelLoadDone");

    }

    bool attemptCloseDoor(Room room, Vector2 targetDirection, GameObject door)
    {
        int targetIndex;
        targetIndex = AllRooms.FindIndex(i => i.x == targetDirection.x && i.y == targetDirection.y);
        if (targetIndex < 0)
        {
            door.SetActive(true);
            room.openDoors.Remove(door);
            return false;
        }
        else return true;
    }

    void closeOffDoors()
    {
        //now close off all doors that do not load to anything
        foreach (Room room in AllRooms)
        {
            Vector2 topDirection = new Vector2(room.x, room.y) + new Vector2(0, +1);
            Vector2 bottomDirection = new Vector2(room.x, room.y) + new Vector2(0, -1);
            Vector2 leftDirection = new Vector2(room.x, room.y) + new Vector2(-1, 0);
            Vector2 rightDirection = new Vector2(room.x, room.y) + new Vector2(1, 0);

            int openings = 0;

            if (attemptCloseDoor(room, topDirection, room.topDoor)) openings++;
            if (attemptCloseDoor(room, bottomDirection, room.bottomDoor)) openings++;
            if (attemptCloseDoor(room, leftDirection, room.leftDoor)) openings++;
            if (attemptCloseDoor(room, rightDirection, room.rightDoor)) openings++;

            if (openings >= 3) OpenRooms.Add(room);

        }
    }


    bool DFS2(Room targetRoom, GameObject doorToClose)
    {

        Stack<Room> currentPath = new Stack<Room>();
        GameObject otherDoor = null;
        Room otherRoom = null;
        Room r = targetRoom;

        currentPath.Push(r);

        //r.SetState(NodeState.Start);
        bool found = false;

        int escape = 0;
        //close door
        targetRoom.openDoors.Remove(doorToClose);
        doorToClose.SetActive(true);

        Vector2 topDirection = new Vector2(targetRoom.x, targetRoom.y) + new Vector2(0, 1);
        Vector2 bottomDirection = new Vector2(targetRoom.x, targetRoom.y) + new Vector2(0, -1);
        Vector2 leftDirection = new Vector2(targetRoom.x, targetRoom.y) + new Vector2(-1, 0);
        Vector2 rightDirection = new Vector2(targetRoom.x, targetRoom.y) + new Vector2(1, 0);

        int index = 0;
        switch (doorToClose.name)
        {
            case "Top":
                index = AllRooms.FindIndex(i => i.x == topDirection.x && i.y == topDirection.y);
                if (index >= 0)
                {
                    AllRooms[index].openDoors.Remove(AllRooms[index].bottomDoor);
                    otherDoor = AllRooms[index].bottomDoor;
                    otherRoom = AllRooms[index];
                    AllRooms[index].bottomDoor.SetActive(true);
                }
                break;
            case "Bottom":
                index = AllRooms.FindIndex(i => i.x == bottomDirection.x && i.y == bottomDirection.y);
                if (index >= 0)
                {
                    AllRooms[index].openDoors.Remove(AllRooms[index].topDoor);
                    otherDoor = AllRooms[index].topDoor;
                    otherRoom = AllRooms[index];
                    AllRooms[index].topDoor.SetActive(true);
                }
                break;
            case "Left":
                index = AllRooms.FindIndex(i => i.x == leftDirection.x && i.y == leftDirection.y);
                if (index >= 0)
                {
                    AllRooms[index].openDoors.Remove(AllRooms[index].rightDoor);
                    otherDoor = AllRooms[index].rightDoor;
                    otherRoom = AllRooms[index];
                    AllRooms[index].rightDoor.SetActive(true);
                }
                break;
            case "Right":
                index = AllRooms.FindIndex(i => i.x == rightDirection.x && i.y == rightDirection.y);
                if (index >= 0)
                {
                    AllRooms[index].openDoors.Remove(AllRooms[index].leftDoor);
                    otherDoor = AllRooms[index].leftDoor;
                    otherRoom = AllRooms[index];
                    AllRooms[index].leftDoor.SetActive(true);
                }
                break;
        }

        int NumberOfRooms = 0;
        int tIndex = -1;
        List<Room> DiscoveredRoom = new List<Room>();
        DiscoveredRoom.Add(targetRoom);
        while (currentPath.Count > 0)
        {
            escape++;
            if (escape >= 1000) break;

            Room tRoom = currentPath.Pop();


            Vector2 tDirection = new Vector2(tRoom.x, tRoom.y) + new Vector2(0, 1);
            Vector2 bDirection = new Vector2(tRoom.x, tRoom.y) + new Vector2(0, -1);
            Vector2 lDirection = new Vector2(tRoom.x, tRoom.y) + new Vector2(-1, 0);
            Vector2 rDirection = new Vector2(tRoom.x, tRoom.y) + new Vector2(1, 0);



            tIndex = AllRooms.FindIndex(i => i.x == tDirection.x && i.y == tDirection.y);
            if (tIndex >= 0 && tRoom.openDoors.Contains(tRoom.topDoor) && !DiscoveredRoom.Contains(AllRooms[tIndex]))
            {
                DiscoveredRoom.Add(AllRooms[tIndex]);
                currentPath.Push(AllRooms[tIndex].GetComponent<Room>());

            }

            tIndex = AllRooms.FindIndex(i => i.x == bDirection.x && i.y == bDirection.y);
            if (tIndex >= 0 && tRoom.openDoors.Contains(tRoom.bottomDoor) && !DiscoveredRoom.Contains(AllRooms[tIndex]))
            {
                DiscoveredRoom.Add(AllRooms[tIndex]);
                currentPath.Push(AllRooms[tIndex].GetComponent<Room>());

            }


            tIndex = AllRooms.FindIndex(i => i.x == rDirection.x && i.y == rDirection.y);
            if (tIndex >= 0 && tRoom.openDoors.Contains(tRoom.rightDoor) && !DiscoveredRoom.Contains(AllRooms[tIndex]))
            {
                DiscoveredRoom.Add(AllRooms[tIndex]);
                currentPath.Push(AllRooms[tIndex].GetComponent<Room>());

            }


            tIndex = AllRooms.FindIndex(i => i.x == lDirection.x && i.y == lDirection.y);
            if (tIndex >= 0 && tRoom.openDoors.Contains(tRoom.leftDoor) && !DiscoveredRoom.Contains(AllRooms[tIndex]))
            {
                DiscoveredRoom.Add(AllRooms[tIndex]);
                currentPath.Push(AllRooms[tIndex].GetComponent<Room>());

            }

        }


        if (DiscoveredRoom.Count == AllRooms.Count) found = true;

        if (!found)
        {
            //restore
            otherDoor.SetActive(false);
            otherRoom.openDoors.Add(otherDoor);
            targetRoom.openDoors.Add(doorToClose);
            doorToClose.SetActive(false);
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

    void createBridge(Vector2 pos, string id, string name)
    {
        GameObject b = Instantiate<GameObject>(BridgePrefab);
        b.transform.position = pos;
        if (name == "Top" || name == "Bottom")
            b.transform.eulerAngles = new Vector3(0, 0, 90);

        b.transform.localScale = new Vector3(spaceBetween, b.transform.localScale.y, 1);

        b.transform.parent = this.transform;
        b.name = "Bridge for " + id + " " + name;

        AllBridges.Add(b);
    }
    void BuildBridges()
    {
        foreach (Room room in AllRooms)
        {

            Vector2 Top = (Vector2)room.transform.position + new Vector2(0, (room.size.y + spaceBetween) * .5f);
            Vector2 Bottom = (Vector2)room.transform.position + new Vector2(0, -(room.size.y + spaceBetween) * .5f);
            Vector2 Left = (Vector2)room.transform.position + new Vector2(-(room.size.x + spaceBetween) * .5f, 0);
            Vector2 Right = (Vector2)room.transform.position + new Vector2((room.size.x + spaceBetween) * .5f, 0);

            //how many doorways?
            foreach (GameObject door in room.openDoors)
            {
                switch (door.name)
                {
                    case "Top":
                        if (bridgePossible(Top)) createBridge(Top, room.name, door.name);
                        break;
                    case "Bottom":
                        if (bridgePossible(Bottom)) createBridge(Bottom, room.name, door.name);
                        break;
                    case "Left":
                        if (bridgePossible(Left)) createBridge(Left, room.name, door.name);
                        break;
                    case "Right":
                        if (bridgePossible(Right)) createBridge(Right, room.name, door.name);
                        break;
                }
            }


        }
    }

    void closeExtra()
    {
        while (OpenRooms.Count > 0)
        {

            Room room = OpenRooms[(int)Random.Range(0, OpenRooms.Count)]; //(int)Random.Range(0, openDoors.Count)

            Vector2 topDirection = new Vector2(room.x, room.y) + new Vector2(0, 1);
            Vector2 bottomDirection = new Vector2(room.x, room.y) + new Vector2(0, -1);
            Vector2 leftDirection = new Vector2(room.x, room.y) + new Vector2(-1, 0);
            Vector2 rightDirection = new Vector2(room.x, room.y) + new Vector2(1, 0);
            int index = 0;
            //determine what doors are open and randomly close one
            if (room.openDoors.Count > 1)
            {
                GameObject doorToClose = room.openDoors[Random.Range(0, room.openDoors.Count)];

                //use path finding to ensure path to start room is accessible
                //double check that room that this connects too has another opening
                DFS2(room, doorToClose);


            }

            //remove from open rooms
            OpenRooms.Remove(room);

        }

    }

    void spawnRoom(Vector2 pos)
    {
        Room r = Instantiate<Room>(RoomPrefab);
        r.transform.position = pos * new Vector2(r.size.x + spaceBetween, r.size.y + spaceBetween);
        r.pos = r.transform.position;
        r.setupDoors();
        r.setupWalls();

        r.transform.parent = this.transform;
        r.x = (int)pos.x;
        r.y = (int)pos.y;
        r.name = "[" + r.x + "," + r.y + "]";
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


}
