using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    void SetupListener()
    {
        EventManager.StartListening("LevelComplete", Handle_LevelComplete);

    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.StopListening("LevelComplete", Handle_LevelComplete);

        }
    }

    public void Handle_LevelComplete(Dictionary<string, object> message)
    {
        // increase floor
        // save stats to game manager
        // load elevator
        // unload scene

    }
}
