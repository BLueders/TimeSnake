using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : TileObject
{
    public enum PatrolMode
    {
        Straight, PingPong
    }

    public int attackRange;
    public PatrolPath patrolPath;
    public PatrolMode patrolMode;
    public int targetPathIndex = 0;

    bool reverse;

    void Start()
    {
        Register();
        positionX = TiledGameMap.Instance.WorldToGridX(transform.position);
        positionY = TiledGameMap.Instance.WorldToGridY(transform.position);
    }

    public override void Advance()
    {
        Move();
    }

    public override void OnOverlap(TileObject other)
    {

    }

    public bool Move()
    {
        int dirX = patrolPath.nodes[targetPathIndex].x - positionX;
        int dirY = patrolPath.nodes[targetPathIndex].y - positionY;

        if (dirX == 0 && dirY == 0)
        {
            IncrementTarget();
            dirX = patrolPath.nodes[targetPathIndex].x - positionX;
            dirY = patrolPath.nodes[targetPathIndex].y - positionY;
        }

        dirX = Mathf.Clamp(dirX, -1, 1);
        dirY = Mathf.Clamp(dirY, -1, 1);

        bool moveX = false;
        bool moveY = false;
        if (dirX != 0)
        {
            moveX = AttemptMove(dirX, 0);
        }
        if (!moveX && dirY != 0)
        {
            moveY = AttemptMove(0, dirY);
        }

        if (!moveX && !moveY)
        {
            IncrementTarget();
            return false;
        }

        return true;
    }

    bool AttemptMove(int x, int y)
    {
        int newPosX = positionX + x;
        int newPosY = positionY + y;

        bool canMove = TiledGameMap.Instance.IsInBounds(newPosX, newPosY) && TiledGameMap.Instance.IsWalkable(newPosX, newPosY);
        if (!canMove)
        {
            return false;
        }

        MoveToGridPosition(newPosX, newPosY);
        return true;
    }

    void IncrementTarget()
    {
        switch (patrolMode)
        {
            case PatrolMode.Straight:
                targetPathIndex++;
                targetPathIndex %= patrolPath.nodes.Count;
                break;

            case PatrolMode.PingPong:
                if (reverse)
                {
                    targetPathIndex--;
                    targetPathIndex += patrolPath.nodes.Count;
                    targetPathIndex %= patrolPath.nodes.Count;
                } else {
                    targetPathIndex++;
                    targetPathIndex %= patrolPath.nodes.Count;
                }
                if (targetPathIndex == 0 || targetPathIndex == patrolPath.nodes.Count - 1)
                {
                    reverse = !reverse;
                }
                break;
        }

    }

    void OnDrawGizmosSelected()
    {
        if (TiledGameMap.Instance != null && TiledGameMap.Instance.hasMap() && patrolPath.nodes.Count > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(TiledGameMap.Instance.GridToWorld(patrolPath.nodes[0].x, patrolPath.nodes[0].y), 0.5f);
            for (int i = 1; i < patrolPath.nodes.Count; i++)
            {
                Gizmos.DrawWireSphere(TiledGameMap.Instance.GridToWorld(patrolPath.nodes[i].x, patrolPath.nodes[i].y), 0.5f);
                Gizmos.DrawLine(TiledGameMap.Instance.GridToWorld(patrolPath.nodes[i - 1].x, patrolPath.nodes[i - 1].y), TiledGameMap.Instance.GridToWorld(patrolPath.nodes[i].x, patrolPath.nodes[i].y));
            }
        }
    }
}
