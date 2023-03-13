using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isMoving = false;
    public float moveSpeed = 1.0f;
    public TileRoom playerRoom;

    bool firstSpawn = true;
    bool freezePlayer = false;

    private void Start()
    {
        //get player camera and parent it to the player


        playerEnteredRoom();
    }

    public void playerEnteredRoom()
    {
        playerRoom = transform.parent.GetComponentInParent<TileRoom>();

        if (!firstSpawn)
        {
            //move camera to new room

            //freeze player input
            freezePlayer = true;
        }
    }


    void SetupListener()
    {
        EventManager.StartListening("PlayerMove", Handle_PlayerMove);
    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
            EventManager.StopListening("PlayerMove", Handle_PlayerMove);
    }

    IEnumerator Move(Vector2 targetPosition)
    {

        Debug.Log("Move " + targetPosition);
        float elapsedTime = 0;
        isMoving = true;
        Vector2 oldPosition = transform.position;

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector3.Lerp(oldPosition, targetPosition, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;

    }

    void Handle_PlayerMove(Dictionary<string, object> message)
    {
        if (!freezePlayer)
        {
            if (!isMoving)
            {
                //up down, or left right, not both
                Vector2 v = (Vector2)message["value"];

                if (v.x != 0 && v.y != 0) v = new Vector2(v.x, 0);

                Vector2 move = (Vector2)transform.position + v;

                //valid movement?
                Vector2 targetTile = move + new Vector2(5.5f, 5.5f);
                int targetX = (int)Mathf.CeilToInt(targetTile.x);
                int targetY = (int)Mathf.CeilToInt(targetTile.y);
                Debug.Log("Target Tile " + targetX + " " + targetY);

                //future move is valid - within the floor grid
                if (targetX > 0 && targetY > 0 && targetX < playerRoom.size.x && targetY < playerRoom.size.y)
                {
                    //is it non solid? 
                    /*if (playerRoom.floorTiles.f[playerRoom.floorTiles.ConvertPosTo1D(targetX, targetY)].GetComponent<FloorTile>().type != FloorTile.TileType.Solid)
                    {

                        StartCoroutine(Move(move));
                    }
                    else
                    {
                        //maybe a slight bounce animation
                    }*/

                }
            }
        }
    }
}
