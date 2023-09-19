using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    [SerializeField] float killDelay = 1f;

    GameObject ramMesh;
    int currentHealth = 0;
    bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
        ramMesh = HelperMethods.GetChildGameObject(gameObject, "RamMesh");
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
        Debug.Log(currentHealth.ToString());
        if (currentHealth <= 0)
        {
            ProcessKill();
        }
    }

    public void ProcessKill()
    {
        isAlive = false;
        ramMesh.GetComponent<MeshRenderer>().enabled = false;
        //GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        //audioPlayer.PlaySFXClipOnce(explosionSFX, explosionSFXVolume);
        //explosion.transform.parent = runtimeParent;
        StartCoroutine(DestroyEntity());
    }

    IEnumerator DestroyEntity()
    {
        yield return new WaitForSecondsRealtime(killDelay);

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
