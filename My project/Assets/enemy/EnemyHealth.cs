using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 50f;

    public GameObject deathEffect; // optional particle effect
    public float destroyDelay = 1f;

    private bool isDead = false;

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // spawn effect (optional)
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // disable visuals
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            renderer.enabled = false;

        // disable collider so it no longer interacts
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // stop movement if it has Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero;

        // destroy after delay
        Destroy(gameObject, destroyDelay);
    }
}