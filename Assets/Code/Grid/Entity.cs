using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    [SerializeField] private Sprite _sprite;

    public Entity(Sprite sprite)
    {
        _sprite = sprite;
    }
    public Sprite GetEntitySprite()
    {
        return _sprite;
    }
}
