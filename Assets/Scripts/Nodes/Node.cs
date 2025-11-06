using UnityEngine;
using System.Collections.Generic;
using System.Drawing;

// Ensure Color refers to Unity's Color type (avoids ambiguity with System.Drawing.Color)
using Color = UnityEngine.Color;

[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    [Tooltip("Nodes this node connects to (branching allowed).")]
    public List<Node> connectedNodes = new List<Node>();
    public string nodeLabel;
    
    // so every node can have a type - Edit and add later on
    public enum NodeType { Normal, Town, Battle, Shop }
    public NodeType nodeType = NodeType.Normal;
    public TownController townController;

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
        Gizmos.color = townController != null ? Color.red : Color.yellow;
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
