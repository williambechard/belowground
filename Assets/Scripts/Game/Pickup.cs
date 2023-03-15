using System.Collections.Generic;
using UnityEngine;

public interface IConsumable
{
    void Consume();


}

public class Pickup : MonoBehaviour
{
    public GameObject effectPREFAB;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Player player = null;
        if (collision.transform.parent != null)
        {
            player = collision.transform.parent.GetComponent<Player>();
        }

        if (player != null)
        {

            GameObject effect = Instantiate(effectPREFAB);
            effect.transform.position = this.transform.position;

            Debug.Log("collision.name " + this.name);

            //emit pickup
            if (EventManager.instance != null)
                EventManager.TriggerEvent("Pickup", new Dictionary<string, object> { { "value", this.name } });

            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
