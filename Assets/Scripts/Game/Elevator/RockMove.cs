using UnityEngine;

public class RockMove : MonoBehaviour
{
    public GameObject RockPREFAB;
    public int width, height;
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject g = Instantiate(RockPREFAB);

                g.transform.parent = this.transform;
                g.transform.localPosition = new Vector3((x * 10) - 60 + Random.Range(-4, 4), (y * 10) - 110 + Random.Range(-4, 4), 0);
                float r = Random.Range(.5f, 2);
                g.transform.localScale = new Vector3(r, r, 1);
            }
        }
    }



    public void createNewRock(float x)
    {
        GameObject g = Instantiate(RockPREFAB);

        g.transform.parent = this.transform;
        g.transform.localPosition = new Vector3(x + Random.Range(-4, 4), ((height - 1) * 10) - 110 + Random.Range(-4, 4), 0);
        float r = Random.Range(.5f, 2);
        g.transform.localScale = new Vector3(r, r, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
