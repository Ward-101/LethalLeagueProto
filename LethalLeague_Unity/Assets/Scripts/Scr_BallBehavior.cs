using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_BallBehavior : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField] private float ballBaseSpeed;
    [SerializeField] private LayerMask raycastHitLayer;

    [Header("States : DON'T TOUCH")]
    [SerializeField] private bool shouldMove = false;

    [Header("Others : DON'T TOUCH")]
    [SerializeField] private Vector2 directionVec = Vector2.zero;
    [SerializeField] private float ballCurrentSpeed;


    private RaycastHit2D raycastHit;
    private float timeUntilCollision;
    private float startTimeUntilCollision;

    private Collider2D lastHitCollider = null;

    private void Start()
    {
        ballCurrentSpeed = ballBaseSpeed;
    }

    private void Update()
    {
        if (shouldMove && directionVec != Vector2.zero)
        {
            BallMovement();
            CollisionDetection();
        }
    }

    private void BallMovement()
    {
        transform.Translate(directionVec * ballCurrentSpeed * Time.deltaTime);
    }

    public void UpdateDirection(Vector2 _directionVec)
    {
        directionVec = _directionVec;

        CollisionPrevention();
    }

    private void CollisionPrevention()
    {
        raycastHit = Physics2D.Raycast(transform.position, directionVec, 999f, raycastHitLayer);

        if (raycastHit.collider != null)
        {
            if (lastHitCollider != null)
            {
                lastHitCollider.gameObject.layer = LayerMask.NameToLayer("Level");
            }
            
            raycastHit.collider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            lastHitCollider = raycastHit.collider;

            startTimeUntilCollision = (float)AudioSettings.dspTime;
            timeUntilCollision = raycastHit.distance / ballCurrentSpeed;

            shouldMove = true;
        }
    }

    private void CollisionDetection()
    {
        float currentTime = (float)AudioSettings.dspTime;

        if (currentTime - startTimeUntilCollision >= timeUntilCollision)
        {
            shouldMove = false;
            HitAngleToNewDirection();
        }
    }

    private void HitAngleToNewDirection()
    {
        float angle = Vector2.Angle(directionVec, raycastHit.normal);
        print(angle);
    }


    #region EDITOR_ONLY
    [ContextMenu("Give the ball a random direction")]
    public void GiveRandomDirection()
    {
        Vector2 randomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        UpdateDirection(randomVector);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, directionVec * raycastHit.distance);
    }

}
