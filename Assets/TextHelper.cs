using TMPro;
using UnityEngine;

public class TextHelper : MonoBehaviour
{
    MapGenerator map;
    public TextMeshProUGUI TEXT;
    // Start is called before the first frame update
    void Start()
    {
        map = FindObjectOfType<MapGenerator>();
        if (map != null)
        {
            map.Text = TEXT;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
