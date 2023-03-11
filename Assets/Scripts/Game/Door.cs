using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    RoomMovement RM;
    // Start is called before the first frame update
    public List<Vector2> DoorwayTiles = new();

    void Start()
    {

    }

    private void OnEnable()
    {
        if (RM == null)
            RM = transform.parent.transform.parent.GetComponentInChildren<RoomMovement>();



        switch (name)
        {
            case "Top":
                DoorwayTiles.Add(new Vector2(Mathf.CeilToInt(RM.size.x / 2), RM.size.y - 1));
                DoorwayTiles.Add(new Vector2(Mathf.CeilToInt(RM.size.x / 2) - 1, RM.size.y - 1));
                break;
            case "Bottom":
                DoorwayTiles.Add(new Vector2(Mathf.CeilToInt(RM.size.x / 2), 0));
                DoorwayTiles.Add(new Vector2(Mathf.CeilToInt(RM.size.x / 2) - 1, 0));
                break;
            case "Left":
                DoorwayTiles.Add(new Vector2(0, Mathf.CeilToInt(RM.size.y / 2)));
                DoorwayTiles.Add(new Vector2(0, Mathf.CeilToInt(RM.size.y / 2) - 1));
                break;
            case "Right":
                DoorwayTiles.Add(new Vector2(RM.size.x - 1, Mathf.CeilToInt(RM.size.y / 2)));
                DoorwayTiles.Add(new Vector2(RM.size.x - 1, Mathf.CeilToInt(RM.size.y / 2) - 1));
                break;
        }

        foreach (Vector2 v in DoorwayTiles)
        {
            RM.f[RM.ConvertPosTo1D((int)v.x, (int)v.y)].GetComponent<FloorTile>().type = FloorTile.TileType.Solid;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
