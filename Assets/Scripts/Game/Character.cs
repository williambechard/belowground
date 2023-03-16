using System.Collections;
using System.Collections.Generic;
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
    public GameObject hitEffect;
    public GameObject dieEffect;

    public int Health;
    public int Strength;
    public float Speed;
    public int MaxHealth;
    int maxStrength;
    float maxSpeed;

    public state currentState = state.Awake;
    public float XP;
    public SpriteRenderer sr;
    Color origColor;

    public bool isMoving = false;
    public Vector2 velocity;
    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        origColor = sr.color;



    }

    private void OnEnable()
    {
        MaxHealth = Health;
        maxStrength = Strength;
        maxSpeed = Speed;
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

        if (EventManager.instance != null)
            EventManager.TriggerEvent("Hit", null);
        Health -= amount;
        Debug.Log("amount " + amount);
        Debug.Log("health " + Health);

        if (Health <= 0)
        {
            //DEAD
            Debug.Log("DEAD");

            GameObject d = Instantiate(dieEffect);


            //if player
            if (this.gameObject.GetComponent<Player>() != null)
            {

                //save player data

                PlayerPrefs.SetInt("MaxHealth", MaxHealth);
                PlayerPrefs.Save();

                PlayerPrefs.SetInt("Strength", maxStrength);
                PlayerPrefs.Save();

                PlayerPrefs.SetFloat("Speed", maxSpeed);
                PlayerPrefs.Save();

                PlayerPrefs.SetFloat("XP", XP);
                PlayerPrefs.Save();


                StartCoroutine(PlayerDeath());
                d.transform.position = this.GetComponentInChildren<SpriteRenderer>().transform.position;
            }
            else
            {
                d.transform.position = this.transform.position;
                Destroy(this.gameObject);

                //give player xp
                GameManager.instance.XP += .5f;
            }



        }
        else
        {
            currentState = state.Frozen;
            GameObject h = Instantiate(hitEffect);
            h.transform.position = this.transform.position;

            //if player
            if (this.gameObject.GetComponent<Player>() != null)
            {


                h.transform.position = this.GetComponentInChildren<SpriteRenderer>().transform.position;
            }



            StartCoroutine(onHit());

        }

        if (EventManager.instance != null)
            EventManager.TriggerEvent("Health", new Dictionary<string, object> { { "value", amount }, { "target", this.gameObject } });
    }

    IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(.01f);
        currentState = state.Frozen;
        this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        //save player data
        yield return new WaitForSeconds(2);
        LevelManager.Instance.LoadScene("Elevator");
        LevelManager.Instance.UnloadScene("Game");
    }

    IEnumerator onHit()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        if (sr == null)
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            origColor = sr.color;
        }

        //flash on and off red
        int i = 0;
        while (i < 5)
        {
            sr.color = Tools.Instance.ColorSwatch[4];
            yield return new WaitForSeconds(.1f);
            sr.color = origColor;
            yield return new WaitForSeconds(.1f);
            i++;
        }

        currentState = state.Awake;
        rb.bodyType = RigidbodyType2D.Dynamic;
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

        if (currentState != state.Frozen) rb.velocity = velocity * Speed;
        else rb.velocity = new Vector2(0, 0);

    }


}
