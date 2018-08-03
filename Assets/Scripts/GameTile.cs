using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
	public bool isWalkable;

	public bool isOccupied { get { return occupant != null; } }

	public TileObject occupant = null;

	public int positionX;
	public int positionY;
}

