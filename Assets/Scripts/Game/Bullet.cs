using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public Vector2 velocity;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(this.gameObject);
    }
}
