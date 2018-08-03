using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
	public int positionX;
	public int positionY;

	public void MoveToTilePosition(GameTile tile){
		TiledGameMap.Instance.SetOccupant (positionX, positionY, null);
		transform.position = tile.transform.position;
		positionX = tile.positionX;
		positionY = tile.positionY;
		TiledGameMap.Instance.SetOccupant (positionX, positionY, this);
	}

	public void MoveToGridPosition(int x, int y){
		MoveToTilePosition(TiledGameMap.Instance.GetTile(x,y));
	}
}

