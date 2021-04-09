using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInteractable : Interactable
{
    PuzzleManager puzzleManager;
    
    void Awake()
    {
        puzzleManager = GetComponentInParent<PuzzleManager>();
    }

    public override void Interact()
    {
        puzzleManager.PuzzleSolved();
    }
}
