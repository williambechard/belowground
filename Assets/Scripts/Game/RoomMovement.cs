using System.Collections.Generic;
using UnityEngine;

public class RoomMovement : MonoBehaviour
{
    public Room room;

    public GameObject[,] floor;
    public GameObject floorTile;

    public List<GameObject> f = new();

    public int totalFloorTiles = 0;
    public Vector2 size;


    // Start is called before the first frame update
    void Start()
    {
        room = transform.parent.GetComponentInParent<Room>();

        if (f.Count == 0)
        {
            foreach (FloorTile FT in GetComponentsInChildren<FloorTile>())
            {
                f.Add(FT.gameObject);
            }
        }
    }

    public int ConvertPosTo1D(int x, int y)
    {
        return (int)(y * size.x + x);
    }
    public void createFloor()
    {

        var tempArray = new GameObject[this.transform.childCount];

        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = this.transform.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            DestroyImmediate(child);
        }

        f.Clear();

        floor = new GameObject[(int)size.x, (int)size.y];

        //create floor grid
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                GameObject g = Instantiate(floorTile);
                g.transform.position = new Vector3(0, 0, 0);
                g.transform.SetParent(this.transform);
                g.transform.localPosition = new Vector3(x - (size.x / 2), y - (size.y / 2), 1);

                g.name = "Tile[" + x + "," + y + "]";

                FloorTile FT = g.GetComponent<FloorTile>();
                FT.x = x;
                FT.y = y;

                FT.origColor = FT.GetComponentInChildren<SpriteRenderer>().color;

                if ((y == size.y - 1 && !(x == size.x / 2 || x == size.x / 2 - 1)) ||
                (y == 0 && !(x == size.x / 2 || x == size.x / 2 - 1)) ||
                (x == 0 && !(y == size.y / 2 || y == size.y / 2 - 1)) ||
                (x == size.x - 1 && !(y == size.y / 2 || y == size.y / 2 - 1)))
                {

                    if (!(x == Mathf.CeilToInt(size.x / 2) && y == size.y - 1) &&
                    !(x == Mathf.CeilToInt(size.x / 2) - 1 && y == size.y - 1) &&
                    !(x == Mathf.CeilToInt(size.x / 2) && y == 0) &&
                    !(x == Mathf.CeilToInt(size.x / 2) - 1 && y == 0) &&
                    !(x == 0 && y == Mathf.CeilToInt(size.y / 2)) &&
                    !(x == 0 && y == Mathf.CeilToInt(size.y / 2) - 1) &&
                    !(x == size.x - 1 && y == Mathf.CeilToInt(size.y / 2)) &&
                    !(x == size.x - 1 && y == Mathf.CeilToInt(size.y / 2) - 1)
                    )
                    {
                        FT.type = FloorTile.TileType.Solid;
                    }

                }
                //else
                //{
                //    FT.type = FloorTile.TileType.None;
                //}

                floor[x, y] = g;
                f.Add(g);
            }
        }


        //create the doorways

        //createDoorWayTiles();

        totalFloorTiles = floor.Length;

    }

    public void createDoorWayTiles()
    {
        //top
        GameObject g = Instantiate(floorTile);
        g.transform.position = new Vector3(0, 0, 0);
        g.transform.SetParent(this.transform);
        int x, y;
        x = (int)Mathf.FloorToInt((size.x - 2) / 2);
        y = (int)size.y - 2;

        g.transform.localPosition = new Vector3(x, y, 1) - new Vector3(size.x / 2, size.y / 2, 0);

        g.name = "Tile[" + x + "," + y + "]";

        FloorTile FT = g.GetComponent<FloorTile>();
        FT.x = x;
        FT.y = y;

        floor[x, y] = g;

        GameObject g2 = Instantiate(floorTile);
        g2.transform.position = new Vector3(0, 0, 0);
        g2.transform.SetParent(this.transform);

        x = (int)Mathf.FloorToInt((size.x - 2) / 2) + 1;
        y = (int)size.y - 3;

        g2.transform.localPosition = new Vector3(x, y, 1) - new Vector3(size.x / 2, size.y / 2, 0);

        g2.name = "Tile[" + x + "," + y + "]";

        FloorTile FT2 = g2.GetComponent<FloorTile>();
        FT2.x = x;
        FT2.y = y;

        floor[x, y] = g2;

    }

}
