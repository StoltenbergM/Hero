using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    [Tooltip("Nodes this node connects to (branching allowed).")]
    public List<Node> connectedNodes = new List<Node>();

    // Optional: a friendly label for debugging
    public string nodeLabel;

    private void OnDrawGizmos()
    {
        // Draw the node itself
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.08f);

        // Draw lines to connected nodes
        Gizmos.color = Color.cyan;
        foreach (var n in connectedNodes)
        {
            if (n != null)
                Gizmos.DrawLine(transform.position, n.transform.position);
        }

        // Draw the label
        #if UNITY_EDITOR
        if (!string.IsNullOrEmpty(nodeLabel))
            UnityEditor.Handles.Label(transform.position + Vector3.up * 0.12f, nodeLabel);
        #endif
    }
}
