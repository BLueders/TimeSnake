using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : TileObject
{

    public void Start(){
        positionX = TiledGameMap.Instance.WorldToGridX(transform.position);
        positionY = TiledGameMap.Instance.WorldToGridY(transform.position);
        TiledGameMap.Instance.AddOccupant(positionX,positionY, this);
    }

    public override void Advance()
    {
        
    }

    public override void OnOverlap(TileObject other)
    {
        SnakeBodyPart snakePart = other.GetComponent<SnakeBodyPart>();
        if(snakePart != null){
            snakePart.snake.Collect(this);
            TiledGameMap.Instance.RegisterOccupantForCleanRemoval(positionX, positionY, this);
            Collectible[] remaining = GameObject.FindObjectsOfType<Collectible>();
            if (remaining.Length == 1) {
                GameManager.NextLevel();
            }
            Destroy(gameObject);
        }
    }
}
