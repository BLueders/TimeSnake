﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class TiledGameMap : MonoBehaviour
{
	private static TiledGameMap _instance;

	public static TiledGameMap Instance {
		get { 
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<TiledGameMap> ();
			}
			return _instance;
		}
		private set { 
			_instance = value;
		}
	}

	public enum OriginType
	{
		Scene
	}

	[MenuItem ("Tools/Create TiledGameMap from Scene")]
	public static void CreateInEditor ()
	{
		Instance.CreateFromScene ();
	}

	GameTile[,] map;

	public OriginType origin;
	public int tileWidth = 1;
	public int tileHeight = 1;

	public int width { get { return map.GetLength (0); } }

	public int height { get { return map.GetLength (1); } }



	void Awake ()
	{
		Instance = this;
		if (origin == OriginType.Scene) {
			CreateFromScene ();
		}
	}

	public bool IsWalkable (int x, int y)
	{
		return map [x, y].isWalkable;
	}

	public bool IsOccupied (int x, int y)
	{
		return map [x, y].isOccupied;
	}

	public bool IsFreeToOccupy (int x, int y)
	{
		return !IsOccupied (x, y) && IsWalkable (x, y);
	}

	public void SetOccupant (int x, int y, TileObject obj)
	{
		map [x, y].occupant = obj;
	}

	public TileObject GetOccupant (int x, int y)
	{
		return map [x, y].occupant;
	}

	public GameTile GetTile (int x, int y)
	{
		return map [x, y];
	}

	public bool IsInBounds (int x, int y)
	{
		return x >= 0 && y >= 0 && x < map.GetLength (0) && y < map.GetLength (1);
	}

	public int WorldToGridX (Vector3 position)
	{
		Vector3 pos0 = map [0, 0].transform.position;
		float offsetX = position.x - pos0.x;
		return (int)offsetX;
	}

	public int WorldToGridY (Vector3 position)
	{
		Vector3 pos0 = map [0, 0].transform.position;
		float offsetY = position.y - pos0.y;
		return (int)offsetY;
	}


	public Vector3 GridToWorld (int x, int y)
	{
		Vector3 pos0 = map [0, 0].transform.position;
		pos0 += new Vector3 (x * tileWidth, y * tileHeight);
		return pos0;
	}

	public bool hasMap ()
	{
		return map != null;
	}

	void CreateFromScene ()
	{
		GameTile[] tiles = GameObject.FindObjectsOfType<GameTile> ();
		int mapWidth = (int)Mathf.Sqrt (tiles.Length);
		float upperX = float.MinValue;
		float upperY = float.MinValue;
		float lowerX = float.MaxValue;
		float lowerY = float.MaxValue;
		foreach (GameTile tile in tiles) {
			if (tile.transform.position.x > upperX) {
				upperX = tile.transform.position.x;
			}
			if (tile.transform.position.y > upperY) {
				upperY = tile.transform.position.y;
			}
			if (tile.transform.position.x < lowerX) {
				lowerX = tile.transform.position.x;
			}
			if (tile.transform.position.y < lowerY) {
				lowerY = tile.transform.position.y;
			}
		}
		int mWidth = (int)(upperX - lowerX) + 1;
		int mHeight = (int)(upperY - lowerY) + 1;
		Debug.Log ("Dimensions X: " + lowerX + " - " + upperX);
		Debug.Log ("Dimensions Y: " + lowerY + " - " + upperY);
		Debug.Log ("Creating map with: " + mWidth + "x" + mHeight);
		map = new GameTile[mWidth, mHeight];
		int x, y;
		foreach (GameTile tile in tiles) {
			x = (int)(tile.transform.position.x - lowerX);
			y = (int)(tile.transform.position.y - lowerY);
			if (map [x, y] != null) {
				Debug.LogError ("Duplicate tile: " + tile + " at: " + tile.transform.position + ". Exiting creation.");
				break;
			}
			Debug.Log ("Inserting " + tile + " at: " + x + ", " + y);
			map [x, y] = tile;
			tile.positionX = x;
			tile.positionY = y;
		}
	}
}