using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;
    private PlayerControls input = null;
    private Vector2 moveVector = Vector2.zero;

    public Input customInput;

    public bool verticalMovement = false;
    public bool horizontalMovement = false;


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
        input.Player.Move.canceled += OnMove;
        input.Player.Move.canceled += onMoveCancel;

        input.Player.Fire.performed += onFire;

    }

    private void OnDisable()
    {

        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        input.Player.Move.canceled -= onMoveCancel;
        input.Player.Fire.performed -= onFire;
    }

    public void onFire(InputAction.CallbackContext value)
    {
        if (EventManager.instance != null)
            EventManager.TriggerEvent("PlayerFire", null);

    }

    public void onMoveCancel(InputAction.CallbackContext value)
    {
        if (moveVector.x == 0) horizontalMovement = false;
        if (moveVector.y == 0) verticalMovement = false;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>().normalized;

        if (moveVector.x != 0) horizontalMovement = true;
        else horizontalMovement = false;

        if (moveVector.y != 0) verticalMovement = true;
        else verticalMovement = false;

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
