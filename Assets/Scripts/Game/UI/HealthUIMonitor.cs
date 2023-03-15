using System.Collections.Generic;
using UnityEngine;

public class HealthUIMonitor : MonoBehaviour
{

    public GameObject healthSlider;
    void SetupListener()
    {
        EventManager.StartListening("Health", Handle_HealthChange);

    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
        Debug.Log("Local position " + healthSlider.GetComponent<RectTransform>().localPosition);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.StopListening("Health", Handle_HealthChange);

        }
    }

    public void Handle_HealthChange(Dictionary<string, object> message)
    {

        //player?
        GameObject g = (GameObject)message["target"];
        Player player = g.GetComponent<Player>();
        if (player != null)
        {

            //player health changed!
            healthSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Lerp(0, 64, ((float)player.Health / (float)player.MaxHealth)), healthSlider.GetComponent<RectTransform>().rect.height);

        }
    }
}
