using UnityEngine;
using UnityEngine.AI;

public class eemyfollow : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float stoppingDistance = 2f;

    [Header("Climbing")]
    public float climbSpeed = 3f;
    public float wallCheckDistance = 1.2f;
    public float climbHeight = 3f;
    public LayerMask wallLayer;

    private NavMeshAgent agent;
    private bool isClimbing = false;
    private float climbedAmount = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        if (player == null) return;

        if (isClimbing)
        {
            Climb();
            return;
        }

        // Normal chasing
        agent.SetDestination(player.position);

        // Check for wall in front
        if (Physics.Raycast(transform.position, transform.forward, wallCheckDistance, wallLayer))
        {
            StartClimbing();
        }
    }

    void StartClimbing()
    {
        isClimbing = true;
        climbedAmount = 0f;

        agent.enabled = false; // disable NavMeshAgent
    }

    void Climb()
    {
        float step = climbSpeed * Time.deltaTime;
        transform.position += Vector3.up * step;
        climbedAmount += step;

        if (climbedAmount >= climbHeight)
        {
            StopClimbing();
        }
    }

    void StopClimbing()
    {
        isClimbing = false;

        agent.enabled = true;
        agent.Warp(transform.position); // sync agent with new position
    }
}