using System.Collections.Generic;
using UnityEngine;

public class TileRoom : MonoBehaviour
{
    public Vector2 pos;

    public int x;
    public int y;
    public Vector2 size = new Vector2(12, 12);

    //[0] Top
    //[1] Right
    //[2] Bottom
    //[3] Left
    public List<bool> openDoors = new();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void FreezeRoom()
    {
        //get all characters in room and set their state to frozen

    }

    // Update is called once per frame
    void Update()
    {

    }
}
