using System.Collections;
using System.Text;
using System;
using UnityEngine;

public class MyUtils
{
    public static IEnumerator WaitFor(Func<bool> Action, float delay)
    {
        float time = 0.0f;
        while (time <= delay)
        {
            if (Action())
                time += Time.unscaledDeltaTime;
            else
                time = 0.0f;
            yield return 0;
        }
        yield break;
    }

    public static bool IsTileAdjacent(Tile tileA, Tile tileB)
    {
        bool isAdjacent = false;

        foreach (Tile tile in tileA.TileSides)
        {
            if (tileB == tile)
            {
                isAdjacent = true;
            }
        }

        return isAdjacent;
    }

    public static Tile GetGravityTargetTile(Tile tile)
    {
        Tile targetTile = null;

        switch (tile.GravityDirection)
        {
            case EGravityDirection.Down:
                targetTile = tile.Bottom;
                break;
            case EGravityDirection.Up:
                targetTile = tile.Top;
                break;
            case EGravityDirection.Left:
                targetTile = tile.Left;
                break;
            case EGravityDirection.Right:
                targetTile = tile.Right;
                break;
            default:
                break;
        }

        return targetTile;
    }

    public static Vector2 OffsetFromGravityDirection(EGravityDirection direction)
    {
        Vector2 offset = Vector2.zero;
        float tileSize = BoardCreator.Instance.TileSize;

        switch (direction)
        {
            case EGravityDirection.Down:
                offset = new Vector2(0f, tileSize);
                break;
            case EGravityDirection.Up:
                offset = new Vector2(0f, -tileSize);
                break;
            case EGravityDirection.Left:
                offset = new Vector2(tileSize, 0f);
                break;
            case EGravityDirection.Right:
                offset = new Vector2(-tileSize, 0f);
                break;
            default:
                break;
        }

        return offset;
    }
}