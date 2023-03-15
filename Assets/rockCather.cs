using UnityEngine;

public class rockCather : MonoBehaviour
{
    public RockMove RM;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        float x = collision.transform.localPosition.x;
        Destroy(collision.gameObject);

        RM.createNewRock(x);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
