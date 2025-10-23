using UnityEngine;
using System.Collections; // Needed for IEnumerator
using UnityEngine.InputSystem; // Required for the new Input System
            
public class PlayerMover : MonoBehaviour
{
    public Node currentNode;    // The node where the player currently stands
    public float moveSpeed = 3f; // Speed of movement

    private bool isMoving = false;

    void Start()
    {
        // Make sure the player starts at the current node
        if (currentNode != null)
            transform.position = currentNode.transform.position;
    }

    void Update()
    {
        // Check if the left mouse button was pressed this frame
        if (!isMoving && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Get the mouse position in world coordinates
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0f; // Ensure z=0 for 2D

            // Detect if we clicked on a node (requires Collider2D on the nodes)
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);
            if (hitCollider != null)
            {
                Node targetNode = hitCollider.GetComponent<Node>();
                if (targetNode != null && currentNode.connectedNodes.Contains(targetNode))
                {
                    StartCoroutine(MoveToNode(targetNode));
                }
            }
        }
    }

    private IEnumerator MoveToNode(Node target)
    {
        isMoving = true;
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
        isMoving = false;
    }
}