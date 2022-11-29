using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2Int _gridPosition { get; }
    private Vector3 _worldPosition;

    private List<Entity> _Entities;
    public Vector3 worldPosition
    {
        get { return _worldPosition; }
    }
    private void Awake()
    {
        _Entities = new List<Entity>();
    }
    public void AddEntity(Entity newEntity)
    {
        _Entities.Add(newEntity);
        RenderEntities();
    }

    public void RemoveEntity(Entity entity)
    {
        if (_Entities.Contains(entity))
        {
            _Entities.Remove(entity);
        }
        RenderEntities();
    }

    private void RenderEntities()
    {
        transform.GetComponent<SpriteRenderer>().sprite = _Entities.Last<Entity>().GetEntitySprite();
    }
}
