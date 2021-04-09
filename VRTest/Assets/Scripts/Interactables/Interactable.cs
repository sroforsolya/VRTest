using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Behaviour halo;

    public virtual void Interact()
    {
    }

    public void Highlight(bool enabled)
    {
        if (halo)
            halo.enabled = enabled;
    }
}
