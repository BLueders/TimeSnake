using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : SnakeBodyPart
{

    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    public override void UpdateGraphics(Direction facing, int preX, int preY, int postX, int postY)
    {
        int deltaX = preX - positionX;
        int deltaY = preY - positionY;

        if (deltaX < 0)
        {
            sRenderer.sprite = spriteLeft;
        }
        else if (deltaX > 0)
        {
            sRenderer.sprite = spriteRight;
        }
        else if (deltaY < 0)
        {
            sRenderer.sprite = spriteDown;
        }
        else
        {
            sRenderer.sprite = spriteUp;
        }
    }
}
