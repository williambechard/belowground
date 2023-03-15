using System.Collections.Generic;
using UnityEngine;

public class KeyFound : MonoBehaviour
{
    public GameObject toShow;
    void SetupListener()
    {
        EventManager.StartListening("Pickup", Handle_Pickup);

    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.StopListening("Pickup", Handle_Pickup);

        }
    }

    public void Handle_Pickup(Dictionary<string, object> message)
    {
        if ((string)message["value"] == "Key")
        {
            toShow.SetActive(true);
        }

    }

}
