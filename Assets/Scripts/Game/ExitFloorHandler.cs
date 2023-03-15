using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitFloorHandler : MonoBehaviour
{
    public float fadeSpeed;
    void SetupListener()
    {
        EventManager.StartListening("ExitLevel", Handle_LevelExit);

    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
        {

            EventManager.StopListening("ExitLevel", Handle_LevelExit);

        }
    }

    public void Handle_LevelExit(Dictionary<string, object> message)
    {
        //freeze player input

        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.currentState = Character.state.Frozen;
            StartCoroutine(FadeCharacterOut(player.GetComponentInChildren<SpriteRenderer>()));
        }

    }


    IEnumerator FadeCharacterOut(SpriteRenderer sr)
    {
        float elapsedTime = 0;

        Color origColor = sr.color;
        Color targetColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0);

        while (elapsedTime < fadeSpeed)
        {

            sr.color = Color.Lerp(origColor, targetColor, elapsedTime / fadeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(.5f);
        //unload and load elevator
        SceneManager.LoadScene("Elevator");
        GameManager.instance.currentFloor++;
        SceneManager.UnloadScene("Game");
    }

}
