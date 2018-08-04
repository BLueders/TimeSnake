using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMiddleBody : SnakeBodyPart
{

    public Sprite spriteHorizontal;
    public Sprite spriteVertical;
    public Sprite spriteLeftUp;
    public Sprite spriteRightUp;
    public Sprite spriteLeftDown;
    public Sprite spriteRightDown;

    public override void UpdateGraphics(Direction facing, int preX, int preY, int postX, int postY)
    {
        int deltaPreX = preX - positionX;
        int deltaPreY = preY - positionY;

        int deltaPostX = postX - positionX;
        int deltaPostY = postY - positionY;

        if (deltaPreX < 0)
        {
            if (deltaPostY < 0)
            {
                sRenderer.sprite = spriteLeftDown;
            }
            else if (deltaPostY > 0)
            {
                sRenderer.sprite = spriteLeftUp;
            }
            else
            {
                sRenderer.sprite = spriteHorizontal;
            }
        }
        else if (deltaPreX > 0)
        {
            if (deltaPostY < 0)
            {
                sRenderer.sprite = spriteRightDown;
            }
            else if (deltaPostY > 0)
            {
                sRenderer.sprite = spriteRightUp;
            }
            else
            {
                sRenderer.sprite = spriteHorizontal;
            }
        }
        else if (deltaPreY < 0)
        {
            if (deltaPostX < 0)
            {
                sRenderer.sprite = spriteLeftDown;
            }
            else if (deltaPostX > 0)
            {
                sRenderer.sprite = spriteRightDown;
            }
            else
            {
                sRenderer.sprite = spriteVertical;
            }
        }
        else
        {
            if (deltaPostX < 0)
            {
                sRenderer.sprite = spriteLeftUp;
            }
            else if (deltaPostX > 0)
            {
                sRenderer.sprite = spriteRightUp;
            }
            else
            {
                sRenderer.sprite = spriteVertical;
            }
        }
    }
}
