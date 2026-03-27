using System.Collections.Generic;
using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public float fireRate = 1f;
    public float rotationSpeed = 5f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    private float fireCooldown = 0f;

    private List<Transform> enemiesInRange = new List<Transform>();

    void Update()
    {
        Transform target = GetNearestEnemy();

        if (target != null)
        {
            AimAt(target);

            if (fireCooldown <= 0f)
            {
                Shoot(target);
                fireCooldown = 1f / fireRate;
            }
        }

        fireCooldown -= Time.deltaTime;
    }

    Transform GetNearestEnemy()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(transform.position, enemy.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    void AimAt(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    void Shoot(Transform target)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = (target.position - firePoint.position).normalized;

        rb.linearVelocity = direction * 20f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
        }
    }
}