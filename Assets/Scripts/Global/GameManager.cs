using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    public int currentFloor;

    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!gameManager)
                {
                    Debug.LogError(" No game manager!");
                }
                else
                {
                    gameManager.Init();

                    //  Sets this to not be destroyed when reloading scene
                    DontDestroyOnLoad(gameManager);
                }
            }
            return gameManager;
        }
    }


    void Init()
    {
        currentFloor = 1;
    }
}
