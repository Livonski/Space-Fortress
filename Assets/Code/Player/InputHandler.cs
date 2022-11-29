using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    private Entity _player;
    private MovableEntity _controller;
    private Vector2Int _playerPosition;
    private float _playerSpeed;

    public void SetupInputHandler(Entity player, Vector2Int playerPosition, float playerSpeed)
    {
        _player = player;
        _playerPosition = playerPosition;
        _playerSpeed = playerSpeed;

        _controller = new MovableEntity(_player,_playerPosition, _playerSpeed);
    }

    private void Update()
    {
        int y = (int)Clamp(Input.GetAxis("Vertical"));
        int x = (int)Clamp(Input.GetAxis("Horizontal"));

        if (Mathf.Abs(x) > 0 | Mathf.Abs(y) > 0)
        {
            Debug.Log(x + " " + y);
            Vector2Int moveDirection = new Vector2Int(x, y);
            //StartCoroutine(_controller.MoveEntity(moveDirection));
            _controller.MoveEntity(moveDirection);
        }
    }

    private float Clamp(float number)
    {
        if (Mathf.Abs(number) > 0)
        {
            if (number > 0)
            {
                return 1;
            }
            return -1;
        }
        return 0;
    }

}
