using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 1;
    [SerializeField] float killDelay = 1f;
    [SerializeField] int difficultyIncrease = 1;

    GameObject ramMesh;
    Enemy enemy;
    int currentHealth = 0;
    bool isAlive;

    public bool IsAlive
    {
        get { return isAlive; }
    }


    private void OnEnable()
    {
        currentHealth = maxHealth;
        isAlive = true;
        ramMesh = HelperMethods.GetChildGameObject(gameObject, "RamMesh");
        ramMesh.GetComponent<MeshRenderer>().enabled = true;
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isAlive && other.tag == "Projectile")
        {
            ProcessHit(other.GetComponent<Projectile>().GetDamage());
        }
    }

    private void ProcessHit(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            ProcessKill();
        }
    }

    public void ProcessKill()
    {
        isAlive = false;
        maxHealth += difficultyIncrease;
        enemy.GoldReward += difficultyIncrease;
        ramMesh.GetComponent<MeshRenderer>().enabled = false;
        //GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        //audioPlayer.PlaySFXClipOnce(explosionSFX, explosionSFXVolume);
        //explosion.transform.parent = runtimeParent;
        enemy.RewardGold();
        StartCoroutine(DestroyEntity());
    }

    IEnumerator DestroyEntity()
    {
        yield return new WaitForSecondsRealtime(killDelay);

        gameObject.SetActive(false);
    }
}
