using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    //0 = top
    //1 = right
    //2 = bottom
    //3 = left

    public int FacingDirection;


    public MapGenerator map;



    private void Start()
    {
        map = FindAnyObjectByType<MapGenerator>();
    }


    public Vector2 DirectionToVector()
    {
        switch (FacingDirection)
        {
            case 0:
                return new Vector2(0, 1);
                break;
            case 1:
                return new Vector2(1, 0);
                break;
            case 2:
                return new Vector2(0, -1);
                break;
            case 3:
                return new Vector2(-1, 0);
                break;
        }
        return new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Player player = null;
        if (collision.transform.parent != null)
        {
            player = collision.transform.parent.GetComponent<Player>();
        }

        if (player != null)
        {
            //only move if player not frozen
            if (player.currentState != Character.state.Frozen)
            {
                //freeze player
                player.currentState = Character.state.Frozen;

                //move camera to new room
                // then once done teleport player to new tile
                if (EventManager.instance != null)
                {
                    Vector2 directionToMove = DirectionToVector();
                    Debug.Log("directionToMove " + directionToMove);
                    int x = map.AllRooms[map.currentRoom].x + (int)directionToMove.x;
                    int y = map.AllRooms[map.currentRoom].y + (int)directionToMove.y;
                    Debug.Log("New index = " + map.GridToIndex(x, y));
                    map.currentRoom = map.GridToIndex(x, y);

                    //set room to visited
                    map.miniMap.minimapRooms[map.currentRoom].GetComponentInChildren<Image>().gameObject.SetActive(true);
                    map.miniMap.mapClone.transform.localPosition += new Vector3(-directionToMove.x * 8, -directionToMove.y * 8, 0);
                    //clear all minimap rooms so that if they were blinking they are now clear
                    for (int i = 0; i < map.miniMap.minimapRooms.Count; i++)
                    {
                        map.miniMap.minimapRooms[i].GetComponent<Image>().color = Tools.Instance.ColorSwatch[1];
                    }


                    EventManager.TriggerEvent("CameraMove", new Dictionary<string, object> { { "value", (Vector2)map.AllRooms[map.GridToIndex(x, y)].transform.position },
                    { "direction", directionToMove } });
                }
                else Debug.LogError("Event Manager.instance in null!");


            }
            Debug.Log("Player entered trigger ");
        }


    }

}
