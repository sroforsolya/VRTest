using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInteractable : Interactable
{
    PuzzleManager puzzleManager;
    
    void Awake()
    {
        puzzleManager = GetComponentInParent<PuzzleManager>();
    }

    public override void Interact()
    {
        puzzleManager.PuzzleStarted();
    }
}
