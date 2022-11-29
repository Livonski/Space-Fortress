using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] Grid mainGrid;
    [SerializeField] private GameObject defaultTile;
    [SerializeField] private Sprite playerSprite;

    [SerializeField] private Vector2Int playerPosition;
    [SerializeField] private float playerSpeed;

    [SerializeField] private InputHandler playerInput;

    private Entity player;

    private void Awake()
    {
        mainGrid.CreateGrid(defaultTile);
    }

    private void Start()
    {
        player = new Entity(playerSprite);
        mainGrid.AddEntity(player, playerPosition);
        playerInput.SetupInputHandler(player, playerPosition, playerSpeed);
    }
}
