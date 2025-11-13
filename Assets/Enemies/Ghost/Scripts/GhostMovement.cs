using UnityEngine;
using Pathfinding;

public class GhostMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 200f;
    private Animator animator;
    public GhostAttack ghostAttack;

    public Transform target;

    public float nextWaypointDistance = 3f;
    public float pathUpdateInterval = 0.5f;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ghostAttack = GetComponent<GhostAttack>();
        seeker = GetComponent<Seeker>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) {
            target = playerObject.transform;
        }

        InvokeRepeating("UpdatePath", 0f, pathUpdateInterval);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (ghostAttack.GetIsAttacking())
        {
            animator.SetFloat("Vel", 0);
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            animator.SetFloat("Vel", 0);
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        animator.SetFloat("Vel", direction.magnitude);
        animator.SetFloat("xVel", direction.x);
        animator.SetFloat("yVel", direction.y);
        animator.SetFloat("lastXVel", direction.x);
        animator.SetFloat("lastYVel", direction.y);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
