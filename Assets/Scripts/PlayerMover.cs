using UnityEngine;
using System.Collections; // Needed for IEnumerator
using System.Collections.Generic;
using UnityEngine.InputSystem; // Required for the new Input System
            
public class PlayerMover : MonoBehaviour
{
    public bool canMove = false;
    public Node currentNode;    // The node where the player currently stands
    public float moveSpeed = 3f; // Speed of movement
    private bool isMoving = false;
    private int movementPoints = 0;
    private List<Node> reachableNodes = new List<Node>();

    void Start()
    {
        // Make sure the player starts at the current node
        if (currentNode != null)
            transform.position = currentNode.transform.position;
    }


    void Update()
    {
        if (!canMove || isMoving) return;
        
        // Check if the left mouse button was pressed this frame
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Get the mouse position in world coordinates
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0f; // Ensure z=0 for 2D

            // Detect if we clicked on a node (requires Collider2D on the nodes)
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);
            if (hitCollider != null)
            {
                Node targetNode = hitCollider.GetComponent<Node>();
                if (targetNode != null && reachableNodes.Contains(targetNode))
                {
                    StartCoroutine(MoveToNode(targetNode));
                }
            }
        }
    }

    public void SetMovementPoints(int points)
    {
        movementPoints = points;
        canMove = true;
        HighlightReachableNodes();
    }

    private void HighlightReachableNodes()
    {
        ClearHighlights();
        reachableNodes = GetReachableNodes(currentNode, movementPoints);
        foreach (var node in reachableNodes)
            node.Highlight(true);
    }

    private void ClearHighlights()
    {
        foreach (var node in reachableNodes)
            node.Highlight(false);
        reachableNodes.Clear();
    }

    private IEnumerator MoveToNode(Node target)
    {
        isMoving = true;
        ClearHighlights();

        // Smooth move
        Vector3 start = transform.position;
        Vector3 end = target.transform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
        currentNode = target;

        movementPoints = 0;
        canMove = false;
        isMoving = false;

        Debug.Log(name + " ended movement at " + target.name);
    }

    private List<Node> GetReachableNodes(Node start, int distance)
    {
        List<Node> result = new List<Node>();
        Queue<(Node, int)> frontier = new Queue<(Node, int)>();
        HashSet<Node> visited = new HashSet<Node>();

        frontier.Enqueue((start, 0));
        visited.Add(start);

        while (frontier.Count > 0)
        {
            var (current, depth) = frontier.Dequeue();

            if (depth > 0)
                result.Add(current);

            if (depth < distance)
            {
                foreach (var neighbor in current.connectedNodes)
                {
                    if (!visited.Contains(neighbor))
                    {
                        frontier.Enqueue((neighbor, depth + 1));
                        visited.Add(neighbor);
                    }
                }
            }
        }

        return result;
    }
}