using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public GameObject bulletPREFAB;
    public Transform targetTransform;
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Fire(Vector3 forward)
    {
        Debug.Log("forward " + forward);
        GameObject bullet = Instantiate(bulletPREFAB);
        bullet.transform.position = targetTransform.position;
        Bullet b = bullet.GetComponent<Bullet>();
        b.Speed = Speed;
        b.velocity = forward;

    }


}
