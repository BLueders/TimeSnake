using UnityEngine;
using System.Collections;

public abstract class SnakeBodyPart : TileObject
{
    public SpriteRenderer sRenderer;
    public Direction facing;
	public Snake snake;

    public void Init(Snake mySnake)
    {
        Register();
        sRenderer = GetComponent<SpriteRenderer>();
		snake = mySnake;
    }

    public abstract void UpdateGraphics(Direction facing, int preX, int preY, int postX, int postY);

    public override void Advance()
    {

    }

	public override void OnOverlap(TileObject other){

    }
}

