using UnityEngine;

public class EnemyFollowClimb : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float speed = 3f;
    public float rotationSpeed = 5f;

    [Header("Climbing")]
    public float climbSpeed = 2f;
    public float wallCheckDistance = 1f;
    public float climbDuration = 2f;
    public LayerMask wallLayer;

    private bool isClimbing = false;
    private float climbTimer = 0f;

    void Update()
    {
        if (player == null) return;

        if (isClimbing)
        {
            HandleClimb();
        }
        else
        {
            FollowPlayer();
            CheckForWall();
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position);
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            // Rotate smoothly
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move forward
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    void CheckForWall()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, wallCheckDistance, wallLayer))
        {
            StartClimb();
        }
    }

    void StartClimb()
    {
        isClimbing = true;
        climbTimer = 0f;
    }

    void HandleClimb()
    {
        // Move upward
        transform.position += Vector3.up * climbSpeed * Time.deltaTime;

        climbTimer += Time.deltaTime;

        // Stop climbing after duration
        if (climbTimer >= climbDuration)
        {
            isClimbing = false;
        }
    }
}