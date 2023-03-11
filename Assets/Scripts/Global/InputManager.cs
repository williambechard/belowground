using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;
    private PlayerControls input = null;
    private Vector2 moveVector = Vector2.zero;

    public Input customInput;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            input = new PlayerControls();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += OnMove;
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>().normalized;

        if (EventManager.instance != null)
            EventManager.TriggerEvent("PlayerMove", new Dictionary<string, object> { { "value", moveVector } });
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
