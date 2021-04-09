using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRaycaster : MonoBehaviour
{
    public LayerMask layerMask;

    [SerializeField]
    private int distance = 100;
    private Camera rayCamera;
    private Interactable currentInteractable;
    
    void Start()
    {
        rayCamera = Camera.main;
    }
    
    void Update()
    {
        Raycast();
    }

    private void Raycast()
    {
        Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, distance, layerMask))
        {
            if (currentInteractable != raycastHit.collider.GetComponent<Interactable>())
            {
                StopAllCoroutines();
                currentInteractable = raycastHit.collider.GetComponent<Interactable>();
                currentInteractable.Highlight(true);
                StartCoroutine(StartInteraction());
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.Highlight(false);
            currentInteractable = null;
            StopAllCoroutines();
        }
    }

    private IEnumerator StartInteraction()
    {
        yield return new WaitForSeconds(0.7f);

        currentInteractable.Interact();
    }
}
