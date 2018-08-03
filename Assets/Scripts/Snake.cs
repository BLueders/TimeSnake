using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
	public SnakeHead headPrefab;
	public SnakeBodyPart bodypartPrefab;

	public static Snake Instance;

	void Awake(){
		Instance = this;
	}

	// position 0 is head
	List<SnakeBodyPart> bodyParts = new List<SnakeBodyPart>();

	public void CreateWithLength(int x, int y, int length){
		bodyParts.Clear ();

		GameTile originTile = TiledGameMap.Instance.GetTile (x, y);

		transform.position = originTile.transform.position;

		SnakeHead head = Instantiate<SnakeHead> (headPrefab, transform);
		head.positionX = x;
		head.positionY = y;
		bodyParts.Add (head);
		TiledGameMap.Instance.SetOccupant (x, y, head);
		for (int i = 1; i < length; i++) {
			SnakeBodyPart bodyPart = Instantiate<SnakeBodyPart> (bodypartPrefab, transform);
			bodyPart.positionX = x;
			bodyPart.positionY = y;
			bodyParts.Add (bodyPart);
			TiledGameMap.Instance.SetOccupant (x, y, bodyPart);
		}
	}

	float oldInputX;
	float oldInputY;
	void Update(){
		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw ("Vertical");
		if (inputX != 0 && oldInputX == 0) {
			if (inputX < 0) {
				Move (Direction.Left);
			}
			if (inputX > 0) {
				Move (Direction.Right);
			}
		}
		if (inputY != 0 && oldInputY == 0) {
			if (inputY < 0) {
				Move (Direction.Down);
			}
			if (inputY > 0) {
				Move (Direction.Up);
			}
		}
		oldInputX = inputX;
		oldInputY = inputY;
	}

	public bool Move(Direction dir){
		int posX = bodyParts [0].positionX;
		int posY = bodyParts [0].positionY;	

		int newPosX = posX;
		int newPosY = posY;

		switch (dir) {
		case Direction.Up:
			newPosY++;
			break;
		case Direction.Down:
			newPosY--;
			break;
		case Direction.Left:
			newPosX--;
			break;
		case Direction.Right:
			newPosX++;
			break;
		}

		if(!TiledGameMap.Instance.IsInBounds(newPosX, newPosY)){
			return false;
		}

		if (!TiledGameMap.Instance.IsFreeToOccupy (newPosX, newPosY)) {
			return false;
		}

		for (int i = 0; i < bodyParts.Count; i++) {
			posX = bodyParts [i].positionX;
			posY = bodyParts [i].positionY;	
			bodyParts [i].MoveToGridPosition (newPosX, newPosY);
			newPosX = posX;
			newPosY = posY;
		}

		return true;
	}
}

