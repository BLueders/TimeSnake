using UnityEngine;
using System.Collections;

public class SnakeHead : SnakeBodyPart
{
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    public override void UpdateGraphics(Direction facing, int preX, int preY, int postX, int postY)
    {
        switch (facing)
        {
            case Direction.Down:
                sRenderer.sprite = spriteDown;
                break;
            case Direction.Up:
                sRenderer.sprite = spriteUp;
                break;
            case Direction.Left:
                sRenderer.sprite = spriteLeft;
                break;
            case Direction.Right:
                sRenderer.sprite = spriteRight;
                break;
        }
    }
}

