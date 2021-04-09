using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableInteractable : Interactable
{
    private PuzzleManager puzzleManager;

    void Awake()
    {
        puzzleManager = GetComponentInParent<PuzzleManager>();
    }

    public override void Interact()
    {
        puzzleManager.SetCurrentMoveable(transform.parent.gameObject);
    }
}
