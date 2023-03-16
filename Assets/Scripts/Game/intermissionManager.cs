using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class intermissionManager : MonoBehaviour
{

    public GameObject playButton;
    public int AvailablePoints;
    public Player player;

    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI StrengthText;
    public TextMeshProUGUI FloorText;

    public TextMeshProUGUI TotalPoints;
    public Animator anim;
    public List<GameObject> SpeedControls = new List<GameObject>();
    public List<GameObject> HealthControls = new List<GameObject>();
    public List<GameObject> StrengthControls = new List<GameObject>();

    public int newHealth, newStrength;
    public float newSpeed;

    bool blinkOn = true;
    public void ModifyHealth(int amount)
    {
        int targetArrowIndex = 0;

        if (amount < 0) targetArrowIndex = 1;

        if (HealthControls[targetArrowIndex].GetComponent<Image>().color.a > .5f)
        {
            if (player == null) player = FindObjectOfType<Player>();
            newHealth += amount;

            if (newHealth == player.Health) HealthControls[1].GetComponent<Image>().color = new Color(HealthControls[1].GetComponent<Image>().color.r, HealthControls[1].GetComponent<Image>().color.g, HealthControls[1].GetComponent<Image>().color.b, .5f);

            if (amount < 0)
                AvailablePoints++;
            else
                AvailablePoints--;


            if (AvailablePoints <= 0) disableAllControls();

            TotalPoints.text = AvailablePoints.ToString();
            HealthText.text = newHealth.ToString();
        }

    }



    public void ModifyStrength(int amount)
    {
        int targetArrowIndex = 0;

        if (amount < 0) targetArrowIndex = 1;
        if (StrengthControls[targetArrowIndex].GetComponent<Image>().color.a > .5f)
        {
            if (player == null) player = FindObjectOfType<Player>();
            newStrength += amount;

            if (newStrength == player.Strength) StrengthControls[1].GetComponent<Image>().color = new Color(StrengthControls[1].GetComponent<Image>().color.r, StrengthControls[1].GetComponent<Image>().color.g, StrengthControls[1].GetComponent<Image>().color.b, .5f);

            if (amount < 0)
                AvailablePoints++;
            else
                AvailablePoints--;

            StrengthText.text = newStrength.ToString();

            if (AvailablePoints <= 0) disableAllControls();
            TotalPoints.text = AvailablePoints.ToString();
        }
    }



    public void ModifySpeed(int amount)
    {
        int targetArrowIndex = 0;

        if (amount < 0) targetArrowIndex = 1;
        if (SpeedControls[targetArrowIndex].GetComponent<Image>().color.a > .5f)
        {
            if (player == null) player = FindObjectOfType<Player>();
            newSpeed += (amount * .25f);

            if (newSpeed == player.Speed) SpeedControls[1].GetComponent<Image>().color = new Color(SpeedControls[1].GetComponent<Image>().color.r, SpeedControls[1].GetComponent<Image>().color.g, SpeedControls[1].GetComponent<Image>().color.b, .5f);

            if (amount <= 0)
                AvailablePoints++;
            else
                AvailablePoints--;
            SpeedText.text = newSpeed.ToString();

            if (newSpeed >= 4)
            {
                SpeedControls[0].GetComponent<Image>().color = new Color(SpeedControls[1].GetComponent<Image>().color.r, SpeedControls[1].GetComponent<Image>().color.g, SpeedControls[1].GetComponent<Image>().color.b, .5f);

                SpeedControls[1].GetComponent<Image>().color = new Color(SpeedControls[1].GetComponent<Image>().color.r, SpeedControls[1].GetComponent<Image>().color.g, SpeedControls[1].GetComponent<Image>().color.b, .5f);
            }
            if (AvailablePoints <= 0) disableAllControls();
            TotalPoints.text = AvailablePoints.ToString();
        }
    }

    void disableAllControls()
    {
        SpeedControls[0].gameObject.SetActive(false);
        SpeedControls[1].gameObject.SetActive(false);
        StrengthControls[0].gameObject.SetActive(false);
        StrengthControls[1].gameObject.SetActive(false);
        HealthControls[0].gameObject.SetActive(false);
        HealthControls[1].gameObject.SetActive(false);

        playButton.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {

        Invoke("SetupListener", .1f);


    }

    IEnumerator blinkPoints()
    {
        yield return new WaitForSeconds(.25f);
        while (AvailablePoints > 0)
        {

            if (blinkOn)
            {
                blinkOn = false;

                TotalPoints.color = new Color(TotalPoints.color.r, TotalPoints.color.g, TotalPoints.color.b, .1f);
            }
            else
            {
                blinkOn = true;
                TotalPoints.color = new Color(TotalPoints.color.r, TotalPoints.color.g, TotalPoints.color.b, 1);
            }
            yield return new WaitForSeconds(.3f);
        }
        TotalPoints.color = new Color(TotalPoints.color.r, TotalPoints.color.g, TotalPoints.color.b, 1);
    }

    void SetupListener()
    {
        EventManager.StartListening("LevelLoadDone", Handle_LevelSetup);

        newHealth = GameManager.instance.Health;
        newSpeed = GameManager.instance.Speed;
        newStrength = GameManager.instance.Strength;


        AvailablePoints = Mathf.FloorToInt(GameManager.instance.XP);

        if (AvailablePoints == 0) AvailablePoints = 1;

        StrengthText.text = newStrength.ToString();
        SpeedText.text = newSpeed.ToString();
        HealthText.text = newHealth.ToString();


        TotalPoints.text = AvailablePoints.ToString();
        if (AvailablePoints == 0) disableAllControls();
        FloorText.text = "Floor " + GameManager.instance.currentFloor.ToString();

        Image HC = HealthControls[1].GetComponent<Image>();
        Image SC = StrengthControls[1].GetComponent<Image>();
        Image SPC = SpeedControls[1].GetComponent<Image>();

        HC.color = new Color(HC.color.r, HC.color.g, HC.color.b, .5f);
        SC.color = new Color(SC.color.r, SC.color.g, SC.color.b, .5f);
        SPC.color = new Color(SPC.color.r, SPC.color.g, SPC.color.b, .5f);


        if (newSpeed == 4)
        {
            SpeedControls[0].GetComponent<Image>().color = new Color(SpeedControls[1].GetComponent<Image>().color.r, SpeedControls[1].GetComponent<Image>().color.g, SpeedControls[1].GetComponent<Image>().color.b, .5f);
            SpeedControls[1].GetComponent<Image>().color = new Color(SpeedControls[1].GetComponent<Image>().color.r, SpeedControls[1].GetComponent<Image>().color.g, SpeedControls[1].GetComponent<Image>().color.b, .5f);
        }

        StartCoroutine(blinkPoints());
    }

    private void OnEnable()
    {



    }
    private void OnDisable()
    {
        if (EventManager.instance != null)
            EventManager.StopListening("LevelLoadDone", Handle_LevelSetup);
    }

    public void LoadNextLevel()
    {
        //provide updated stats to the player
        GameManager.instance.Health = newHealth;
        GameManager.instance.Speed = newSpeed;
        GameManager.instance.Strength = newStrength;
        GameManager.instance.XP = 0;

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.Health = newHealth;
            player.Speed = newSpeed;
            player.Strength = newStrength;
            player.XP = 0;
        }

        FindFirstObjectByType<MapGenerator>().MusicStart.SetActive(true);
        FindFirstObjectByType<Player>().currentState = Character.state.Awake;
        LevelManager.Instance.UnloadScene("Elevator");
    }

    void Handle_LevelSetup(Dictionary<string, object> message)
    {

        if (AvailablePoints == 0)
            playButton.SetActive(true);
    }

    public void playButtonClicked()
    {
        playButton.SetActive(false);
        anim.SetBool("Continue", true);
    }



    // Update is called once per frame
    void Update()
    {

    }
}
