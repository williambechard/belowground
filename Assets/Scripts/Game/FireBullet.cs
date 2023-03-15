using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public GameObject bulletPREFAB;
    public Transform targetTransform;
    public float Speed;
    public int Strength;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Fire(Vector3 forward)
    {

        GameObject bullet = Instantiate(bulletPREFAB);
        bullet.transform.position = targetTransform.position;
        Bullet b = bullet.GetComponent<Bullet>();
        b.Speed = Speed;
        b.Strength = Strength;
        b.velocity = forward;

    }


}
