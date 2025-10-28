using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    [Tooltip("Nodes this node connects to (branching allowed).")]
    public List<Node> connectedNodes = new List<Node>();
    public string nodeLabel;
    // so every node can have a type:
    public enum NodeType { Normal, Town, Battle, Shop }
    public NodeType nodeType = NodeType.Normal;
    
    private SpriteRenderer sr;
    private Color originalColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    public void Highlight(bool state)
    {
        if (sr == null) return;
        sr.color = state ? Color.yellow : originalColor;
    }
    
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
