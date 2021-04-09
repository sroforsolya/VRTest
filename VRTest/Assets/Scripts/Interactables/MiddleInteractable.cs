using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleInteractable : Interactable
{
    PuzzleManager puzzleManager;

    void Awake()
    {
        puzzleManager = GetComponentInParent<PuzzleManager>();
    }

    public override void Interact()
    {
        puzzleManager.SetNode(gameObject.GetComponent<Collider>());
    }
}
