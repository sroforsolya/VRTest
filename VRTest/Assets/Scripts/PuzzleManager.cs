using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Collider start, end;
    public List<Collider> middle;
    public List<Collider> moveables;
    public Color inactiveColor, activeColor;
    public GameObject hotspotToActivate = null;
    public string puzzleSolvedAnimName = "Solved";

    private LineRenderer lr;

    private bool puzzleStarted = false;

    private int nextPosition = 1;

    private MeshCollider meshCollider;

    private Animator animator;

    private DragRigidbody currentMoveable;

    private Vector3 mousePos;


    #region Unity Events

    private void Start()
    {
        // disable colliders
        ToggleObjectsSelectable(false);
        // enable moveable colliders
        ToggleMoveableSelectable(true);

        animator = GetComponent<Animator>();

        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, start.transform.position);
        // create mesh collider for line renderer
        meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    private void Update()
    {
        if (puzzleStarted)
        {
            if (!lr.enabled)
                lr.enabled = true;

            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 6));
            lr.SetPosition(nextPosition, mousePos);
        }
    }

    private void FixedUpdate()
    {
        if (currentMoveable)
        {
            currentMoveable.HandleInput(Input.mousePosition);
        }
    }

    /// <summary>
    /// Check if mouse is over line renderer's collider whle in puzzle mode
    /// Resets puzzle if it is => lines are intersecting
    /// </summary>
    private void OnMouseOver()
    {
        if (puzzleStarted)
            ResetPuzzle();
    }

    #endregion

    #region Public Functions

    /// <summary>
    /// Called when hovering over start point
    /// </summary>
    public void PuzzleStarted()
    {
        puzzleStarted = true;
        // disable start collider
        start.enabled = false;
        // enable colliders for other points
        ToggleObjectsSelectable(true);
        // disable moveable colliders
        ToggleMoveableSelectable(false);
        // set the color to active
        start.GetComponent<SpriteRenderer>().color = activeColor;
    }

    /// <summary>
    /// Called when hovering over middle buttons
    /// </summary>
    /// <param name="col"></param>
    public void SetNode(Collider col)
    {
        // change color
        col.GetComponent<SpriteRenderer>().color = activeColor;
        // disable collider to avoid selecting node twice
        col.enabled = false;
        // set the position of line renderer vertice
        lr.SetPosition(nextPosition, col.transform.position);
        // increment position count
        lr.positionCount++;
        // set next position as previous position - default is Vector3.zero - a white line is seen for a fraction of a second otherwise
        lr.SetPosition(nextPosition + 1, lr.GetPosition(nextPosition));

        // update mesh collider for line renderer with new line
        Mesh mesh = new Mesh();
        lr.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;

        // increment next position
        nextPosition++;
    }

    /// <summary>
    /// Called when hovering over end point, if puzzle is not complete -> reset -> return
    /// </summary>
    public void PuzzleSolved()
    {
        // not all points are connected, reset puzzle and return
        if (lr.positionCount < middle.Count + 2)
        {
            ResetPuzzle();
            return;
        }

        // change color for end element
        end.GetComponent<SpriteRenderer>().color = activeColor;
        // set position of line renderer
        lr.SetPosition(nextPosition, end.transform.position);
        // set puzzle started to false
        puzzleStarted = false;
        // disable colliders on objects
        ToggleObjectsSelectable(false);

        StartCoroutine(PlayPuzzleSolvedAnim());
    }

    /// <summary>
    /// Set the current moveable object and start input
    /// </summary>
    /// <param name="moveable"></param>
    public void SetCurrentMoveable(GameObject moveable)
    {
        currentMoveable = moveable.GetComponent<DragRigidbody>();
        currentMoveable.GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(EnableMoveableCollider());
        start.enabled = false;
        ToggleMoveableSelectable(false);
        currentMoveable.HandleInputBegin(Input.mousePosition);
    }

    /// <summary>
    /// Finished moving once object's velocity is 0
    /// </summary>
    public void FinishedMoving()
    {
        currentMoveable.HandleInputEnd(Input.mousePosition);
        currentMoveable.GetComponent<Rigidbody>().isKinematic = true;
        currentMoveable.GetComponent<Collider>().enabled = false;
        currentMoveable = null;
        ToggleMoveableSelectable(true);
        start.enabled = true;
    }

    #endregion

    #region Private Functions

    /// <summary>
    /// enable moveable collider with a delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableMoveableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        currentMoveable.GetComponent<Collider>().enabled = true;
    }

    /// <summary>
    /// Set collider to enabled/disabled
    /// </summary>
    /// <param name="selectable"></param>
    private void ToggleObjectsSelectable(bool selectable)
    {
        end.enabled = selectable;

        foreach (Collider col in middle)
            col.enabled = selectable;
    }

    private void ToggleMoveableSelectable(bool selectable)
    {
        if (moveables != null)
        {
            foreach (Collider col in moveables)
                col.enabled = selectable;
        }
    }

    /// <summary>
    /// Reset colors to inactive
    /// </summary>
    private void ResetColor()
    {
        start.GetComponent<SpriteRenderer>().color = inactiveColor;
        end.GetComponent<SpriteRenderer>().color = inactiveColor;

        foreach (Collider col in middle)
            col.GetComponent<SpriteRenderer>().color = inactiveColor;
    }

    /// <summary>
    /// Reset the puzzle
    /// </summary>
    private void ResetPuzzle()
    {
        puzzleStarted = false;
        // disable colliders for other points and reset colors
        ToggleObjectsSelectable(false);
        ToggleMoveableSelectable(true);
        ResetColor();
        // enable start point's collider
        start.enabled = true;
        // reset line renderer
        lr.enabled = false;
        lr.positionCount = 2;
        nextPosition = 1;

        // clear mesh collider
        meshCollider.sharedMesh = null;
    }

    private IEnumerator PlayPuzzleSolvedAnim()
    {
        animator.Play(puzzleSolvedAnimName);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (hotspotToActivate != null)
            hotspotToActivate.SetActive(true);

        gameObject.SetActive(false);
    }
    #endregion
}
