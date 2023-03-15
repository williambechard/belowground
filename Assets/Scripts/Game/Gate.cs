using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject Sprite;
    MapGenerator map;

    // Start is called before the first frame update
    void Start()
    {
        map = FindFirstObjectByType<MapGenerator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision " + collision);
        Player player = null;
        if (collision.transform.parent != null)
        {
            player = collision.transform.parent.GetComponent<Player>();
        }
        Debug.Log("player " + player);
        if (player != null)
        {
            //player have key?
            if (map.keyFound)
            {
                Sprite.SetActive(false);
            }
        }
    }

}
