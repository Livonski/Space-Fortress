using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovableEntity
{
    private Entity _movableEntity;
    private Vector2Int _position;

    private float _speed;
    float moveDelay = 0f;
    public MovableEntity(Entity movableEntity, Vector2Int position, float speed)
    {
        _movableEntity = movableEntity;
        _position = position;
        _speed = speed;
    }

    //public IEnumerator MoveEntity(Vector2Int moveDirection)
    public void MoveEntity(Vector2Int moveDirection)
    {
        if(moveDirection != null && moveDelay <= 0)
        {
            _position = GameObject.Find("MainGrid").GetComponent<Grid>().MoveEntity(_position, _movableEntity, moveDirection);
            moveDelay = _speed;
        }

        moveDelay -= Time.deltaTime;
        //yield return new WaitForSeconds(100);
    }
}