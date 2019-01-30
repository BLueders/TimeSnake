using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverserTreat : Collectible {

    public void Start()
    {
        positionX = TiledGameMap.Instance.WorldToGridX(transform.position);
        positionY = TiledGameMap.Instance.WorldToGridY(transform.position);
        TiledGameMap.Instance.AddOccupant(positionX, positionY, this);
    }

    public override void Advance()
    {

    }

    public override void OnOverlap(TileObject other)
    {
        SnakeBodyPart snakePart = other.GetComponent<SnakeBodyPart>();
        if (snakePart != null)
        {
            snakePart.snake.ReverseSnake();
            TiledGameMap.Instance.RemoveOccupantClean(positionX, positionY, this);
            Destroy(gameObject);
        }
    }
}
