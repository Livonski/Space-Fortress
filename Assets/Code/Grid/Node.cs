using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    private Vector2Int _gridPosition { get; }
    private Vector3 _worldPosition;

    private GameObject _tile;

    int _index;
    public int _heapIndex
    {
        get
        {
            return _index;
        }
        set
        {
            _index = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        return 0;
    }

    public Vector3 worldPosition
    {
        get { return _worldPosition; }
    }
    public Node(Vector2Int gridPosition, Vector3 worldPosition, GameObject defaultTile)
    {
        _gridPosition = gridPosition;
        _worldPosition = worldPosition;
        _tile = defaultTile;
    }
}

