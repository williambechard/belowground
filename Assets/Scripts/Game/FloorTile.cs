using UnityEngine;

public class FloorTile : MonoBehaviour
{

    public enum TileType
    {
        Solid,
        Bridge,
        Item,
        Enemy,
        Exit,
        None
    }

    public int x, y;
    public GameObject PREFAB;
    public bool onlyOne = true;
    public TileType type;
    public Color32 origColor;
    public SpriteRenderer borderSprite;
    public bool Solid;




    // Start is called before the first frame update
    void Start()
    {


        if (PREFAB != null)
        {
            if (!onlyOne || (onlyOne && !GameObject.Find(PREFAB.name)))
            {
                GameObject g = Instantiate<GameObject>(PREFAB);
                g.transform.parent = this.transform;
                g.transform.localPosition = new Vector3(.5f, .5f, 0);
                g.transform.parent = this.transform.parent.transform;
                g.name = PREFAB.name;
            }


        }
    }

    public void applyTypeEffect()
    {
        /*
            switch (type)
            {
                case TileType.Solid:
                    GetComponentInChildren<SpriteRenderer>().color = Color.red;
                    break;
                case TileType.Bridge:
                    GetComponentInChildren<SpriteRenderer>().color = Color.green;
                    break;
                case TileType.Enemy:
                    GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
                    break;
                case TileType.Exit:
                    GetComponentInChildren<SpriteRenderer>().color = Color.black;
                    break;
                case TileType.None:
                    GetComponentInChildren<SpriteRenderer>().color = origColor;
                    break;
            }*/
    }




}
