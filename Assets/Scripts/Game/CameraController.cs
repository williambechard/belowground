using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Camera gameCamera;
    public float cameraMoveSpeed;

    Player player;
    Vector2 moveDirection;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

    }
    void SetupListener()
    {
        EventManager.StartListening("CameraMove", Handle_CameraMove);
    }

    public void Handle_CameraMove(Dictionary<string, object> message)
    {

        Vector2 v2POS = (Vector2)message["value"];
        moveDirection = (Vector2)message["direction"];
        StartCoroutine(MoveCamera(new Vector3(v2POS.x, v2POS.y, -10)));
    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }
    private void OnDisable()
    {
        if (EventManager.instance != null)
            EventManager.StopListening("CameraMove", Handle_CameraMove);
    }
    IEnumerator MoveCamera(Vector3 targetPosition)
    {

        Debug.Log("Move " + targetPosition);
        float elapsedTime = 0;
        targetPosition += new Vector3(0, 1, 0);
        Vector2 oldPosition = gameCamera.transform.position;
        if (player == null) player = FindObjectOfType<Player>();
        Vector3 playerOldPosition = player.transform.position;
        Vector3 playerNewPosition = player.transform.position + (Vector3)(moveDirection * 3.16f);

        while (elapsedTime < cameraMoveSpeed)
        {
            gameCamera.transform.position = Vector3.Lerp(oldPosition, targetPosition, elapsedTime / cameraMoveSpeed);
            player.transform.position = Vector3.Lerp(playerOldPosition, playerNewPosition, elapsedTime / cameraMoveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //unfreeze player and others
        player.currentState = Character.state.Awake;
        gameCamera.transform.position = targetPosition;



        player.transform.position = playerNewPosition;


    }
}
