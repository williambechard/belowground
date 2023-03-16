using System.Collections.Generic;
using UnityEngine;

interface IControllable
{
    void Handle_PlayerMove(Dictionary<string, object> message);
}

public class Player : Character, IControllable
{
    public GameObject bulletPREFAB;
    public FireBullet weapon;
    public lookAt look;
    PlaySoundOneShot FireSoundFX;


    private void Awake()
    {
        weapon = GetComponent<FireBullet>();
        FireSoundFX = GetComponent<PlaySoundOneShot>();

    }



    void SetupListener()
    {
        EventManager.StartListening("PlayerMove", Handle_PlayerMove);
        EventManager.StartListening("PlayerFire", Handle_Fire);
    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
        {
            EventManager.StopListening("PlayerMove", Handle_PlayerMove);
            EventManager.StopListening("PlayerFire", Handle_Fire);
        }
    }

    public void Handle_Fire(Dictionary<string, object> message)
    {
        if (currentState != state.Frozen)
        {
            weapon.Strength = Strength;
            //fire projectile
            weapon.Fire(look.targetTransform.right);
            FireSoundFX.playSound();
        }
    }

    public void Handle_PlayerMove(Dictionary<string, object> message) => Move((Vector2)message["value"]);



}
