using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{

    public AnimationCurve curve;
    public float duration = 1f;
    Vector3 startPosition;
    MapGenerator map;
    // Start is called before the first frame update
    void SetupListener()
    {
        EventManager.StartListening("Hit", Handle_Shake);
        map = FindFirstObjectByType<MapGenerator>();

    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.StopListening("Hit", Handle_Shake);

        }
    }

    public void Handle_Shake(Dictionary<string, object> message)
    {

        StopCoroutine(Shaking());
        transform.position = new Vector3(map.AllRooms[map.currentRoom].transform.position.x, map.AllRooms[map.currentRoom].transform.position.y, -10);
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        startPosition = new Vector3(map.AllRooms[map.currentRoom].transform.position.x, map.AllRooms[map.currentRoom].transform.position.y + 1, -10);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = new Vector3(startPosition.x + (Random.insideUnitSphere.x / 4), startPosition.y + (Random.insideUnitSphere.y / 4), -10);
            yield return null;

        }
        transform.position = new Vector3(map.AllRooms[map.currentRoom].transform.position.x, map.AllRooms[map.currentRoom].transform.position.y + 1, -10);
    }
}
