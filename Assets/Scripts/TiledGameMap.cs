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
#if UNITY_EDITOR
    [MenuItem ("Tools/Create TiledGameMap from Scene (save)")]
#endif
	public static void CreateInEditorSave ()
	{
		Instance.CreateFromScene (false);
	}
#if UNITY_EDITOR
    [MenuItem("Tools/Create TiledGameMap from Scene (auto cleanup)")]
#endif
    public static void CreateInEditorAutoCleanUp()
    {
        Instance.CreateFromScene(true);
    }

    GameTile[,] map;

	public OriginType origin;
	public int tileWidth = 1;
	public int tileHeight = 1;

    public PlayerStartLocation playerStartLocation;

    public int width { get { return map.GetLength (0); } }

	public int height { get { return map.GetLength (1); } }

	struct PositionedTileObject{
		internal TileObject obj;
		internal int x;
		internal int y;
	}

	List<PositionedTileObject> removalList = new List<PositionedTileObject>();
	List<PositionedTileObject> additionList = new List<PositionedTileObject>();

	void Awake ()
	{
		Instance = this;
		if (origin == OriginType.Scene) {
			CreateFromScene (false);
		}
        CreatePlayer();
    }

    public bool IsBlocked(int x, int y) {
        bool blocked = false;
        foreach (TileObject obj in map[x, y].occupants)
        {
            if (obj.blockMovement)
            {
                blocked = true;
            }
        }
        return blocked;
    }

    public bool IsWalkable (int x, int y)
	{
		return map [x, y].isWalkable;
	}

	public bool IsOccupied (int x, int y)
	{
		return map [x, y].isOccupied;
	}

	public void AddOccupant (int x, int y, TileObject obj)
	{
		map [x, y].occupants.Add(obj);
	}

	public void RemoveOccupant (int x, int y, TileObject obj)
	{
		map [x, y].occupants.Remove(obj);
	}

/// <summary>
/// Add new object to board after physics step
/// </summary>
/// <param name="x"></param>
/// <param name="y"></param>
/// <param name="obj"></param>
	public void AddOccupantClean (int x, int y, TileObject obj)
	{
		PositionedTileObject aObj;
		aObj.obj = obj;
		aObj.x = x;
		aObj.y = y;
		additionList.Add(aObj);
	}
/// <summary>
/// Remove object from board after physics step
/// </summary>
/// <param name="x"></param>
/// <param name="y"></param>
/// <param name="obj"></param>
	public void RemoveOccupantClean(int x, int y, TileObject obj)
	{
		PositionedTileObject rObj;
		rObj.obj = obj;
		rObj.x = x;
		rObj.y = y;
		removalList.Add(rObj);
	}

	public List<TileObject> GetOccupants (int x, int y)
	{
		return map [x, y].occupants;
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

	public void ExecutePhysicsStep(){
		foreach(GameTile tile in map){
			foreach(TileObject tileObj in tile.occupants){
				foreach(TileObject overlapObject in tile.occupants){
					if(tileObj != overlapObject){
						tileObj.OnOverlap(overlapObject);
					}
				}
			}
		}
	}

	public void CleanAddObjectsToBoard(){
		foreach(PositionedTileObject aObj in additionList){
			map[aObj.x, aObj.y].occupants.Add(aObj.obj);
		}
		additionList.Clear();
	}

	public void CleanRemoveObjectsFromBoard(){
		foreach(PositionedTileObject rObj in removalList){
			map[rObj.x, rObj.y].occupants.Remove(rObj.obj);
		}
		removalList.Clear();
	}

    void CreateFromScene (bool autoCleanUp)
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
                if (autoCleanUp)
                {
                    Debug.LogWarning("Removed Duplicate tile: " + map[x, y] + " at: " + map[x, y].transform.position);
                    DestroyImmediate(map[x, y].gameObject);
                }
                else {
				    Debug.LogError ("Duplicate tile: " + tile + " at: " + tile.transform.position + ". Exiting creation.");
                    Debug.LogError("Duplicate tile: " + map[x, y] + " at: " + tile.transform.position + ". Exiting creation.");
                    map = null;
                    return;
                }
			}
			Debug.Log ("Inserting " + tile + " at: " + x + ", " + y);
			map [x, y] = tile;
			tile.positionX = x;
			tile.positionY = y;
		}
        
	}

    void CreatePlayer() {
        PlayerStartLocation start = GameObject.FindObjectOfType<PlayerStartLocation>();
        if (start == null)
        {
            Debug.LogError("No PlayerStartLocation found");
            return;
        }
        start.positionX = WorldToGridX(start.transform.position);
        start.positionY = WorldToGridY(start.transform.position);
        Debug.Log("Found player start at: " + start.positionX + ", " + start.positionY);
        playerStartLocation = start;
        AddOccupant(start.positionX, start.positionY, start);
        Snake.Instance.CreateWithLength(start.positionX, start.positionY, start.startSnakeLength);
        Debug.Log("Created snake at: " + start.positionX + ", " + start.positionY + " with length: " + start.startSnakeLength);
    }
}
