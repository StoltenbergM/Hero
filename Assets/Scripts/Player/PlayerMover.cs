using UnityEngine;
using System.Collections; // Needed for IEnumerator
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Xml; // Required for the new Input System

public class PlayerMover : MonoBehaviour
{
    public bool canMove = false;
    public Node currentNode;    // The node where the player currently stands
    public float moveSpeed = 3f; // Speed of movement
    public TownUI townUI;
    public PlayerDeck playerDeck;
    public PlayerEconomy playerEconomy;
    private bool isMoving = false;
    private int movementPoints = 0;
    private List<Node> reachableNodes = new List<Node>();

    void Start()
    {
        Debug.Log("NodeType test: " + Node.NodeType.Normal);
        
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

        // Find path from current node to target
        List<Node> path = FindPath(currentNode, target);
        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("No path found!");
            isMoving = false;
            yield break;
        }

        // Move step-by-step along the path
        for (int i = 0; i < path.Count; i++)
        {
            Node step = path[i];
            Vector3 start = transform.position;
            Vector3 end = step.transform.position;
            float t = 0f;

            // If this is the last step, and it's a special node, ask first
            if (i == path.Count - 1 && step.nodeType != Node.NodeType.Normal)
            {
                bool waiting = true;
                bool proceed = false;

                UIManager.Instance.ShowConfirm(
                    $"Are you sure you want to enter {step.nodeType}?",
                    () => { proceed = true; waiting = false; },
                    () => { proceed = false; waiting = false; }
                );

                // Wait for player to choose
                yield return new WaitUntil(() => !waiting);

                // If player said No → stop before entering the special node
                if (!proceed)
                {
                    Debug.Log("Player cancelled entry.");
                    break;
                }

                // If this is a Town node, open the town UI
                if (step.nodeType == Node.NodeType.Town)
                {
                    // Get the TownController from this node
                    TownController town = step.GetComponent<TownController>();
                    if (town != null)
                    {
                        FindFirstObjectByType<TownUI>(FindObjectsInactive.Include).ShowTown(town, playerDeck, playerEconomy); // add playerdeck and economy later
                    }
                }
            }

            // Move toward the next node
            while (t < 1f)
            {
                t += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }

            transform.position = end;
            currentNode = step;
        }

        // When finished, stop movement for this turn
        isMoving = false;
        canMove = false;
        ClearHighlights();
    }
    
    // Basic breadth-first search (BFS) for shortest path
    private List<Node> FindPath(Node start, Node target)
    {
        Queue<Node> queue = new Queue<Node>();
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        queue.Enqueue(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            if (current == target)
                break;

            foreach (Node neighbor in current.connectedNodes)
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        if (!cameFrom.ContainsKey(target))
            return null; // No path found

        // Reconstruct path
        List<Node> path = new List<Node>();
        for (Node n = target; n != start; n = cameFrom[n])
        {
            path.Insert(0, n);
        }
        return path;
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