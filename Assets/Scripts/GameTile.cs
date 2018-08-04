using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
	public bool isWalkable;

	public bool isOccupied { get { return occupants.Count != 0; } }

	public List<TileObject> occupants = new List<TileObject>();

	public int positionX;
	public int positionY;
}

