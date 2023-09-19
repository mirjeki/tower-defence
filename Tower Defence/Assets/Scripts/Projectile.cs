using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] int speed = 5;
    [SerializeField] AudioClip projectileSFX;

    public int GetDamage()
    {
        return damage;
    }

    public void PlaySFX()
    {
        //play SFX
    }
}
