using UnityEngine;
using System.Collections;

public class BombPiece : APiece
{
    [SerializeField]
    private GameObject effect;

    override public void DestroyPiece()
    {
        foreach (Tile adjacentTile in ParentTile.TileSides)
        {
            if (adjacentTile && adjacentTile.Piece && !adjacentTile.Piece.IsDestroyed)
            {
                adjacentTile.Piece.IsDestroyed = true;
                adjacentTile.Piece.DestroyPiece();
            }
        }

        // instantiate effect
        if (effect)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }

        base.DestroyPiece();
    }
}
