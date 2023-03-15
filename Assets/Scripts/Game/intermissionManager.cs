using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class intermissionManager : MonoBehaviour
{

    public GameObject playButton;
    public int AvailablePoints;
    public Player player;

    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI StrengthText;
    public TextMeshProUGUI FloorText;
    public void IncreaseHealth()
    {

    }

    public void DecreaseHealth()
    {

    }


    public void IncreaseStrength()
    {

    }

    public void DecreaseStrength()
    {

    }

    public void IncreaseSpeed()
    {

    }

    public void DecreaseSpeed()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<Player>();
        if (player == null)
        {
            //first play so setup based on init
            StrengthText.text = "1";
            SpeedText.text = "2";
            HealthText.text = "10";
        }
        else
        {

            StrengthText.text = player.Strength.ToString();
            SpeedText.text = player.Speed.ToString();
            HealthText.text = player.Health.ToString();
        }
        FloorText.text = "Floor " + (FindObjectOfType<MapGenerator>().level).ToString();
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

    public void LoadNextLevel()
    {

    }

    void Handle_LevelSetup(Dictionary<string, object> message)
    {
        Debug.Log("Level is dont setting up");



        playButton.SetActive(true);
    }

    public void playButtonClicked()
    {
        //
        LevelManager.Instance.UnloadScene("Elevator");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
