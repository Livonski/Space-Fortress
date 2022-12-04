using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] Grid mainGrid;

    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float noiseScale;
    [SerializeField] private float worldSeed;

    [SerializeField] private AnimationCurve tilesProbability;

    [SerializeField] private GameObject[] possibleTiles;
    [SerializeField] private Sprite playerSprite;

    [SerializeField] private Vector2Int playerPosition;
    [SerializeField] private float playerSpeed;

    [SerializeField] private InputHandler playerInput;

    private Entity player;

    private void Awake()
    {
        mainGrid.CreateGrid(gridSize);
        worldSeed = Random.Range(1, 999999);
        FillWorld();
    }

    public void FillWorld()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                float xCoord = worldSeed + ((float)x / gridSize.x) * noiseScale;
                float yCoord = worldSeed + ((float)y / gridSize.y) * noiseScale;

                int TileID = (int)tilesProbability.Evaluate(Mathf.PerlinNoise(xCoord, yCoord));

                //Debug.Log("Perlin noise at x:y " + x + ":" + y + " = " + Mathf.PerlinNoise(xCoord, yCoord) + ", int value: " + TileID);
                GameObject newTile = possibleTiles[TileID];
                mainGrid.AddObjectToGrid(x, y, newTile);
            }
        }
    }

    private void Start()
    {
        player = new Entity(playerSprite);
        mainGrid.AddEntity(player, playerPosition);
        playerInput.SetupInputHandler(player, playerPosition, playerSpeed);
    }
}
