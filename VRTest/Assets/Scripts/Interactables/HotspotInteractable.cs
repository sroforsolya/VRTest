using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotspotInteractable : Interactable
{
    private MovementController movementController;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
    }

    public override void Interact()
    {
        movementController.SetTarget(transform);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }
}
