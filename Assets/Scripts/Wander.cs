using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{
    public float pursuitSpeed;
    public float wanderSpeed;
    float currentSpeed;
    // 2
    public float directionChangeInterval;
    // 3
    public bool followPlayer;
    // 4
    Coroutine moveCoroutine;
    // 5
    Rigidbody2D rb2d;
    Animator animator;
    // 6
    Transform targetTransform = null;
    // 7
    Vector3 endPosition;
    // 8
    float currentAngle = 0;

    CircleCollider2D circleCollider;


    void Start()
    {
        // 1
        animator = GetComponent<Animator>();
        // 2
        currentSpeed = wanderSpeed;
        // 3
        rb2d = GetComponent<Rigidbody2D>();
        // 4
        StartCoroutine(WanderRoutine());

        circleCollider = GetComponent<CircleCollider2D>();
    }
    // 1
    public IEnumerator WanderRoutine()
    {
        // 2
        while (true)
        {
            // 3
            ChooseNewEndpoint();
            //4
            if (moveCoroutine != null)
            {
                // 5
                StopCoroutine(moveCoroutine);
            }
            // 6
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
            // 7
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    // 1
    void ChooseNewEndpoint()
    {
        // 2
        currentAngle += Random.Range(0, 360);
        // 3
        currentAngle = Mathf.Repeat(currentAngle, 360);
        // 4
        endPosition += Vector3FromAngle(currentAngle);
    }

    /// <summary>
    /// Gets an vector from an angle
    /// </summary>
    /// <param name="inputAngleDegrees">The input angle degrees.</param>
    /// <returns></returns>
    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        // 1
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;
        // 2
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        // to retrieve the rough distance remaining between the current position of the Enemy and the destination
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        // 2
        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null)
            {
                endPosition = targetTransform.position;
            }
            // 4
            if (rigidBodyToMove != null)
            {
                // 5
                animator.SetBool("isWalking", true);
                // 6
                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);
                // 7
                rb2d.MovePosition(newPosition);
                // 8
                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            // 9
            yield return new WaitForFixedUpdate();
        }
        // 10
        animator.SetBool("isWalking", false);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            currentSpeed = pursuitSpeed;

            // Set this variable so the Move coroutine can use it to follow the player.
            targetTransform = collision.gameObject.transform;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            // At this point, endPosition is now player object's transform, ie: will now move towards the player
            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isWalking", false);
            currentSpeed = wanderSpeed;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            // Since we no longer have a target to follow, set this to null
            targetTransform = null;
        }
    }

    void OnDrawGizmos()
    {
        // 1
        if (circleCollider != null)
        {
            // 2
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }

    void Update()
    {
        // 1
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }
}


