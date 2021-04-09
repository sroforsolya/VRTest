using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://gist.github.com/mstevenson/4552515
/// </summary>
public class DragRigidbody : MonoBehaviour
{
    public float force = 600;
    public float damping = 6;

    Transform jointTransform;
    Vector3 mousePos;

    Rigidbody rb;
    PuzzleManager puzzleManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        puzzleManager = GetComponentInParent<PuzzleManager>();
    }

    public void HandleInputBegin(Vector3 screenPosition)
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 6));
        jointTransform = AttachJoint(rb, mousePos);
    }

    public void HandleInput(Vector3 screenPosition)
    {
        if (jointTransform == null)
            return;

        jointTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 6));
    }

    public void HandleInputEnd(Vector3 screenPosition)
    {
        Destroy(jointTransform.gameObject);
    }

    Transform AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        GameObject go = new GameObject("Attachment Point");
        go.hideFlags = HideFlags.HideInHierarchy;
        go.transform.position = attachmentPosition;

        var newRb = go.AddComponent<Rigidbody>();
        newRb.isKinematic = true;

        var joint = go.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rb;
        joint.configuredInWorldSpace = true;
        joint.xDrive = NewJointDrive(force, damping);
        joint.yDrive = NewJointDrive(force, damping);
        joint.zDrive = NewJointDrive(force, damping);
        joint.slerpDrive = NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        return go.transform;
    }

    private JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive();
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.collider.gameObject.layer == 10 && collisionInfo.relativeVelocity == Vector3.zero)
        {
            puzzleManager.FinishedMoving();
        }
    }
}
