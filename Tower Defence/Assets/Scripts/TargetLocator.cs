using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] float range = 15f;
    [SerializeField] ParticleSystem projectileParticles;
    Transform target;
    bool hasTargetInRange = false;

    void Update()
    {
        FindClosestTarget();
        if (target != null)
        {
            AimWeapon();
        }
        else
        {
            ResetTurret();
        }
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        if (enemies.Length > 0)
        {
            Transform closestNewTarget = null;

            float maxDistance = Mathf.Infinity;

            foreach (Enemy enemy in enemies)
            {
                float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

                if (targetDistance < maxDistance)
                {
                    closestNewTarget = enemy.transform;
                    maxDistance = targetDistance;
                }
            }

            if (!hasTargetInRange)
            {
                target = closestNewTarget;
            }
        }
    }

    private void AimWeapon()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);
        weapon.LookAt(target);

        if (target.gameObject.GetComponent<EnemyHealth>().IsAlive)
        {
            if (targetDistance < range)
            {
                hasTargetInRange = true;
                Attack(true);
            }
            else
            {
                ResetTurret();
            }
        }
        else
        {
            ResetTurret();
        }
    }

    private void ResetTurret()
    {
        hasTargetInRange = false;
        weapon.LookAt(new Vector3(0f, 0f, 0f));
        Attack(false);
    }

    private void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
