using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    private Grid grid;
    public GameObject roomMapPREFAB;
    public GameObject bridgeMapPREFAB;
    public float betweenRoomSize;
    public GameObject mapClone;
    public List<GameObject> minimapRooms = new List<GameObject>();
    GameObject currentRoom;
    bool blinkOn;
    // Start is called before the first frame update
    void Start()
    {

    }

    void SetupListener()
    {
        EventManager.StartListening("LevelLoadDone", Handle_LevelSetup);
    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }
    private void OnDisable()
    {
        if (EventManager.instance != null)
            EventManager.StopListening("LevelLoadDone", Handle_LevelSetup);
    }

    public void changeRoom()
    {
        currentRoom.GetComponent<Image>().color = Tools.Instance.ColorSwatch[2];
        //change to new room

        currentRoom.GetComponent<Image>().color = Tools.Instance.ColorSwatch[3];
    }

    IEnumerator blinkCurrentLocation()
    {
        yield return new WaitForSeconds(.25f);
        while (true)
        {

            if (blinkOn)
            {
                blinkOn = false;
                currentRoom.GetComponent<Image>().color = Tools.Instance.ColorSwatch[3];
            }
            else
            {
                blinkOn = true;
                currentRoom.GetComponent<Image>().color = Tools.Instance.ColorSwatch[1];
            }
            yield return new WaitForSeconds(.3f);
        }
    }

    void Handle_LevelSetup()
    {
        grid = FindFirstObjectByType<Grid>();
        foreach (Room room in grid.AllRooms)
        {
            GameObject g = Instantiate<GameObject>(roomMapPREFAB);

            g.transform.parent = mapClone.transform;
            g.transform.localPosition = new Vector2(room.x * betweenRoomSize, room.y * betweenRoomSize);
            minimapRooms.Add(g);

        }



        //set current room, which is room 0

        foreach (GameObject bridge in grid.AllBridges)
        {
            GameObject g = Instantiate<GameObject>(bridgeMapPREFAB);
            g.transform.parent = mapClone.transform;
            g.transform.localPosition = new Vector3(bridge.transform.position.x / (betweenRoomSize / 2.27f), bridge.transform.position.y / (betweenRoomSize / 2.27f), 1);
            g.transform.localRotation = bridge.transform.localRotation;
        }
        Debug.Log("grid captured" + grid);
        currentRoom = minimapRooms[0];
        StartCoroutine(blinkCurrentLocation());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
