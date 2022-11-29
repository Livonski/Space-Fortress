using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private int nodeSize;

    private GameObject[,] grid;
    public void CreateGrid(GameObject defaultTile)
    {
        grid = new GameObject[gridSize.x, gridSize.y];
        Vector2 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.up * gridSize.y / 2;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector2.right * (x * nodeSize + nodeSize/2) + Vector2.up * (y * nodeSize + nodeSize/2);
                GameObject newGridObject = Instantiate(defaultTile, worldPoint, Quaternion.identity);
                newGridObject.name = "Tile " + x + " " + y;
                newGridObject.transform.parent = transform;
                newGridObject.GetComponent<Tile>().AddEntity(new Entity(newGridObject.GetComponent<SpriteRenderer>().sprite));
                grid[x, y] = newGridObject;
            }
        }
    }

    public void AddEntity(Entity entity, Vector2Int TilePosition)
    {
        if(TilePosition.x<gridSize.x && TilePosition.y < gridSize.y)
        {
            grid[TilePosition.x, TilePosition.y].GetComponent<Tile>().AddEntity(entity);
        }
    }

    public Vector2Int MoveEntity(Vector2Int entityPosition, Entity entity, Vector2Int moveDirection)
    {
        if (entityPosition.x + moveDirection.x < gridSize.x && entityPosition.y + moveDirection.y < gridSize.y && entityPosition.x + moveDirection.x >= 0 && entityPosition.y + moveDirection.y >= 0)
        {
            grid[entityPosition.x, entityPosition.y].GetComponent<Tile>().RemoveEntity(entity);
            grid[entityPosition.x + moveDirection.x, entityPosition.y + moveDirection.y].GetComponent<Tile>().AddEntity(entity);
            return (entityPosition + moveDirection);
        }
        return(entityPosition);
    }

    private void OnDrawGizmos()
    {

        if (grid != null)
        {
            foreach (GameObject g in grid)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(g.transform.position, Vector3.one * (nodeSize-0.05f));
            }
        }
    }
}
