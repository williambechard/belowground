using UnityEngine;


interface IMoveable
{
    void Move(Vector2 moveVector);
}

interface IDamageable
{
    void Damage(int amount);
}

interface IAttack
{
    void Attack(float Str);
}
public class Character : MonoBehaviour, IMoveable, IDamageable, IAttack
{
    public int Health;
    public int Strength;
    public float Speed;

    public state currentState = state.Awake;

    public bool isMoving = false;
    public Vector2 velocity;
    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    public enum state
    {
        Frozen,
        OnFire,
        Poisoned,
        Asleep,
        Confused,
        Awake
    }

    public void Attack(float Str)
    {

    }

    public void Damage(int amount)
    {
        throw new System.NotImplementedException();
    }
    public void Move(Vector2 moveVector)
    {

        velocity = moveVector * Speed;

        if (moveVector != new Vector2(0, 0))
            isMoving = true;
        else isMoving = false;
    }

    private void FixedUpdate()
    {

        if (currentState != state.Frozen)
        {
            rb.velocity = velocity * Speed;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }


    }


}
