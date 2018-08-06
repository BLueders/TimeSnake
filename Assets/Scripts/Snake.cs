using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Snake : MonoBehaviour
{
    private static Snake _instance;

    public static Snake Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Snake>();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public SnakeHead headPrefab;
	public SnakeMiddleBody middleBodyPrefab;
	public SnakeTail tailPrefab;

	float oldInputX;
	float oldInputY;
	Direction lastMovementDir;

	void Awake(){
		Instance = this;
	}

    public void Collect(Collectible collectible)
    {
        ExtendSnake(1);
    }

    // position 0 is head
    List<SnakeBodyPart> bodyParts = new List<SnakeBodyPart>();

	public void CreateWithLength(int x, int y, int length){
		foreach(SnakeBodyPart part in bodyParts){
			Destroy(part.gameObject);
		}

		bodyParts.Clear ();

		GameTile originTile = TiledGameMap.Instance.GetTile (x, y);

		transform.position = originTile.transform.position;

		SnakeHead head = Instantiate<SnakeHead> (headPrefab, transform);
		head.Init(this);
		head.positionX = x;
		head.positionY = y;
		bodyParts.Add (head);
		TiledGameMap.Instance.AddOccupant (x, y, head);
		ExtendSnake(length-1);
		UpdateSnakeGraphics(lastMovementDir);
	}

	void Update(){
        if (GameManager.state != GameState.Playing) return;

		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw ("Vertical");
		bool moveSucceeded = false;
		if (inputX != 0 && oldInputX == 0) {
			if (inputX < 0) {
				moveSucceeded = Move (Direction.Left);
			}
			if (inputX > 0) {
				moveSucceeded = Move (Direction.Right);
			}
		}
		if (inputY != 0 && oldInputY == 0) {
			if (inputY < 0) {
				moveSucceeded = Move (Direction.Down);
			}
			if (inputY > 0) {
				moveSucceeded = Move (Direction.Up);
			}
		}
		oldInputX = inputX;
		oldInputY = inputY;

		if(moveSucceeded){
			TimeStepManager.AdvanceTimeStep();
		}

		if(Input.GetKeyDown(KeyCode.E)){
			ExtendSnake(1);
			UpdateSnakeGraphics(lastMovementDir);
		}
		if(Input.GetKeyDown(KeyCode.R)){
			ReduceSnake(1);
			UpdateSnakeGraphics(lastMovementDir);
		}
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

        if (!TiledGameMap.Instance.IsWalkable(newPosX, newPosY))
        {
            return false;
        }

        if (TiledGameMap.Instance.IsBlocked (newPosX, newPosY)) {
			return false;
		}

		for (int i = 0; i < bodyParts.Count; i++) {
			posX = bodyParts [i].positionX;
			posY = bodyParts [i].positionY;	
			bodyParts [i].MoveToGridPosition (newPosX, newPosY);
			newPosX = posX;
			newPosY = posY;
		}
		lastMovementDir = dir;
		UpdateSnakeGraphics(lastMovementDir);
		return true;
	}
	
	void UpdateSnakeGraphics(Direction lastMoveDir){
		if(bodyParts.Count == 0) return;
		
		//update head
		bodyParts[0].UpdateGraphics(lastMoveDir, 0, 0, 0, 0);

		int preX = 0;
		int preY = 0;
		int postX = 0;
		int postY = 0;

		for (int i = 1; i < bodyParts.Count-1; i++) {
			preX = bodyParts[i-1].positionX;
			preY = bodyParts[i-1].positionY; 
			postX = bodyParts[i+1].positionX;
			postY = bodyParts[i+1].positionY;
			bodyParts[i].UpdateGraphics(0, preX, preY, postX, postY);
		}

		if(bodyParts.Count == 1) return;
		//update tail if longer than 1
		preX = bodyParts[bodyParts.Count-2].positionX;
		preY = bodyParts[bodyParts.Count-2].positionY; 
		bodyParts[bodyParts.Count-1].UpdateGraphics(0, preX, preY, 0, 0);
	}

	void UpdateSnakeLength(int length){
		int currentLength = bodyParts.Count;
		if(length > currentLength){
			ExtendSnake(length - currentLength);
		}
		if(length < currentLength){
			ReduceSnake(currentLength - length);
		}
	}

	void ExtendSnake(int count){
		// add body part if long enough
		if(bodyParts.Count > 1){
			SnakeMiddleBody bodyPart = Instantiate<SnakeMiddleBody> (middleBodyPrefab, transform);
			bodyPart.Init(this);
			bodyPart.transform.position = bodyParts[bodyParts.Count-1].transform.position;
			bodyPart.positionX = bodyParts[bodyParts.Count-1].positionX;
			bodyPart.positionY = bodyParts[bodyParts.Count-1].positionY;
			bodyParts.Insert (bodyParts.Count-1, bodyPart);
			TiledGameMap.Instance.AddOccupant (bodyPart.positionX, bodyPart.positionY, bodyPart);
			count--;
			if(count > 0){
				ExtendSnake(count);
			}
		}
		// add tail of not
		if(bodyParts.Count == 1){
			SnakeTail tail = Instantiate<SnakeTail> (tailPrefab, transform);
			tail.Init(this);
			tail.transform.position = bodyParts[0].transform.position;
			tail.positionX = bodyParts[0].positionX;
			tail.positionY = bodyParts[0].positionY;
			bodyParts.Add (tail);
			TiledGameMap.Instance.AddOccupant (tail.positionX, tail.positionY, tail);
			count--;
			if(count > 0){
				ExtendSnake(count);
			}
		}
	}

	void ReduceSnake(int count){

		// remove tail if short enough
		if(bodyParts.Count == 2){
            TiledGameMap.Instance.RemoveOccupant(bodyParts[1].positionX, bodyParts[1].positionY, bodyParts[1]);
			Destroy(bodyParts[1].gameObject);
			bodyParts.RemoveAt(1);
		}

		// remove body part if long enough
		if(bodyParts.Count > 2){
            bodyParts[bodyParts.Count-1].MoveToGridPosition(bodyParts[bodyParts.Count-2].positionX, bodyParts[bodyParts.Count-2].positionY);
            TiledGameMap.Instance.RemoveOccupant(bodyParts[bodyParts.Count - 2].positionX, bodyParts[bodyParts.Count - 2].positionY, bodyParts[bodyParts.Count - 2]);
			Destroy(bodyParts[bodyParts.Count-2].gameObject);
			bodyParts.RemoveAt(bodyParts.Count-2);
			count--;
			if(count > 0){
				ReduceSnake(count);
			}
		}

		// else do noghting, head has to stay
	}
}

