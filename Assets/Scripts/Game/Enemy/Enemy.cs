using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    public List<Vector2> rMovement = new List<Vector2>();
    int moveIndex;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomMovement());
    }

    void ChangeMovement()
    {
        moveIndex = Random.Range(0, rMovement.Count);
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            ChangeMovement();
            if (currentState != state.Asleep && currentState != state.Frozen)
                Move(rMovement[moveIndex]);
            else Move(new Vector2(0, 0));

            yield return new WaitForSeconds(1);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChangeMovement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        //see if we hit the enemy!
        if (collision.gameObject.name == "Player")
        {
            //only count a hit if not frozen
            if (collision.transform.parent.GetComponent<Player>().currentState != state.Frozen)
            {
                MonoBehaviour[] list = collision.transform.parent.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour mb in list)
                {
                    if (mb is IDamageable)
                    {
                        IDamageable breakable = (IDamageable)mb;
                        breakable.Damage(Strength);
                    }
                }
            }
        }
        else { ChangeMovement(); }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
