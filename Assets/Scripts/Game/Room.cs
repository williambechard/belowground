using System.Collections.Generic;
using UnityEngine;
public enum NodeState
{
    Available,
    Current,
    Completed,
    Start,
    Final
}

public class Room : MonoBehaviour
{
    public List<GameObject> openDoors = new();

    //****
    //*
    //*
    public GameObject Wall_TL;
    public GameObject Wall_LT;

    //****
    //   *
    //   *
    public GameObject Wall_TR;
    public GameObject Wall_RT;


    //*
    //*
    //****
    public GameObject Wall_BL;
    public GameObject Wall_LB;

    //   *
    //   *
    //****
    public GameObject Wall_BR;
    public GameObject Wall_RB;


    public GameObject topDoor;
    public GameObject bottomDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;


    public Vector2 size;

    public Vector2 pos;

    public int x;
    public int y;

    public SpriteRenderer floor;
    public NodeState state;

    public void Start()
    {

        floor.transform.localScale = size;
    }

    public void setupDoors()
    {
        topDoor.transform.localPosition = new Vector3(0, size.y * .5f, 0);
        bottomDoor.transform.localPosition = new Vector3(0, -(size.y * .5f), 0);
        leftDoor.transform.localPosition = new Vector3(-(size.x * .5f), 0, 0);
        rightDoor.transform.localPosition = new Vector3(size.x * .5f, 0, 0);
    }

    public void setupWalls()
    {
        Wall_TL.transform.localPosition = new Vector3(-(size.x / 4) - .5f, (size.y * .5f) - .5f, 0);
        Wall_TL.transform.localScale = new Vector3((size.x / 2) - 1, 1, 1);

        Wall_LT.transform.localPosition = new Vector3(-(size.x / 2) + .5f, (size.y / 4) + .5f, 0);
        Wall_LT.transform.localScale = new Vector3(1, (size.x / 2) - 1, 1);

        Wall_TR.transform.localPosition = new Vector3((size.x / 4) + .5f, (size.y * .5f) - .5f, 0);
        Wall_TR.transform.localScale = new Vector3((size.x / 2) - 1, 1, 1);

        Wall_RT.transform.localPosition = new Vector3((size.x / 2) - .5f, (size.y / 4) + .5f, 0);
        Wall_RT.transform.localScale = new Vector3(1, (size.x / 2) - 1, 1);

        Wall_BL.transform.localPosition = new Vector3(-(size.x / 4) - .5f, -(size.y * .5f) + .5f, 0);
        Wall_BL.transform.localScale = new Vector3((size.x / 2) - 1, 1, 1);

        Wall_LB.transform.localPosition = new Vector3(-(size.x / 2) + .5f, -(size.y / 4) - .5f, 0);
        Wall_LB.transform.localScale = new Vector3(1, (size.x / 2) - 1, 1);

        Wall_BR.transform.localPosition = new Vector3((size.x / 4) + .5f, -(size.y * .5f) + .5f, 0);
        Wall_BR.transform.localScale = new Vector3((size.x / 2) - 1, 1, 1);

        Wall_RB.transform.localPosition = new Vector3((size.x / 2) - .5f, -(size.y / 4) - .5f, 0);
        Wall_RB.transform.localScale = new Vector3(1, (size.x / 2) - 1, 1);

    }

    public void SetState(NodeState state)
    {
        this.state = state;
        switch (state)
        {
            case NodeState.Available:
                floor.color = Color.white;
                break;
            case NodeState.Current:
                floor.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.color = Color.blue;
                break;
            case NodeState.Start:
                floor.color = Color.green;
                break;
            case NodeState.Final:
                floor.color = Color.red;
                break;
        }
    }

}
