using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileObject : TimeStepObject
{
	public int positionX;
	public int positionY;

	public bool blockMovement = false;

	public void MoveToTilePosition(GameTile tile){
		if(positionX == tile.positionX && positionY == tile.positionY){
			return;
		}
		TiledGameMap.Instance.RemoveOccupant (positionX, positionY, this);
		transform.position = tile.transform.position;
		positionX = tile.positionX;
		positionY = tile.positionY;
		TiledGameMap.Instance.AddOccupant (positionX, positionY, this);
	}

	public void MoveToGridPosition(int x, int y){
		MoveToTilePosition(TiledGameMap.Instance.GetTile(x,y));
	}

	public abstract void OnOverlap(TileObject other);
}

