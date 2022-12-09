using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float nodeSize;
    [SerializeField] private GameObject defaultTile;
    private Vector2Int gridSize;

    private Vector2 worldBottomLeft;

    private GameObject[,] grid;
    public void CreateGrid(Vector2Int GridSize)
    {
        gridSize = GridSize;
        grid = new GameObject[GridSize.x, GridSize.y];
        worldBottomLeft = transform.position - Vector3.right * GridSize.x / 2 - Vector3.up * GridSize.y / 2;
    }

    public void AddObjectToGrid(int x, int y, GameObject Tile)
    {
        if (grid[x,y] != null)
        {
            Destroy(grid[x, y]);
        }
        Vector3 worldPoint = worldBottomLeft + Vector2.right * (x * nodeSize + nodeSize / 2) + Vector2.up * (y * nodeSize + nodeSize / 2);
        GameObject newGridObject = Instantiate(Tile, worldPoint, Quaternion.identity);
        newGridObject.name = "Tile " + x + " " + y;
        newGridObject.transform.parent = transform;
        newGridObject.GetComponent<Tile>().AddEntity(new Entity(newGridObject.GetComponent<SpriteRenderer>().sprite));
        grid[x, y] = newGridObject;
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
