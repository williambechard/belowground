using UnityEngine;

public class Stairs : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision Enter Stairs!");
        Player player = null;
        if (collision.transform.parent != null)
        {
            player = collision.transform.parent.GetComponent<Player>();
        }

        if (player != null)
        {
            if (EventManager.instance != null)
                EventManager.TriggerEvent("ExitLevel", null);
        }
    }
}
