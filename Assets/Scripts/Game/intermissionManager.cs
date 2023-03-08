using UnityEngine;

public class intermissionManager : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    void SetupListener()
    {
        EventManager.StartListening("LevelLoadDone", Handle_LevelSetup);
    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }
    private void OnDisable()
    {
        if (EventManager.instance != null)
            EventManager.StopListening("LevelLoadDone", Handle_LevelSetup);
    }

    void Handle_LevelSetup()
    {
        Debug.Log("Level is dont setting up");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
