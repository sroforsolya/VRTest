using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Range(1, 10)]
    public float movementSpeed = 1;

    private bool movingToTarget;
    private Vector3 targetPos;
    
    void Update()
    {
        if (movingToTarget)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.3f)
            movingToTarget = false;
    }

    /// <summary>
    /// Called from hotspot interactable to set position
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        movingToTarget = true;
        targetPos = target.position;
    }
}
