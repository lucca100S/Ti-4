using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] private List<Graph> _connectedNodes = new List<Graph>();
    public IReadOnlyList<Graph> ConnectedNodes => _connectedNodes;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var node in _connectedNodes)
        {
            if (node != null)
                Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }

    public Graph GetRandomConnectedNode()
    {
        if (_connectedNodes.Count == 0) return null;
        return _connectedNodes[Random.Range(0, _connectedNodes.Count)];
    }
}