using UnityEngine;
using System.Collections;
using System;

public class Tile : MonoBehaviour
{
    private APiece piece;
    public Point Coordinates;

    // Stores all the adjacent tiles
    public Tile Left { get; set; }
    public Tile Right { get; set; }
    public Tile Top { get; set; }
    public Tile TopLeft { get; set; }
    public Tile TopRight { get; set; }
    public Tile Bottom { get; set; }
    public Tile BottomLeft { get; set; }
    public Tile BottomRight { get; set; }

    public Tile[] TileSides;

    public bool HasGravity { get; set; }
    public EGravityDirection GravityDirection { get; set; }
    public Tile GravityTargetTile { get; set; }
    public bool Spawner { get; set; }

    public GameObject Highlight;

    public Transform thisTransform;

    public void Awake()
    {
        Highlight.SetActive(false);
    }

    public void Start()
    {
        TileSides = new Tile[8] { Left, Right, Top, TopLeft, TopRight, Bottom, BottomLeft, BottomRight };
        GravityTargetTile = MyUtils.GetGravityTargetTile(this);
        thisTransform = this.transform;
    }

    void Update()
    {
        GravityHandler();
    }

    public void SetCoords(int x, int y)
    {
        this.Coordinates = new Point(x, y);
    }

    /**********************************************
    **                                           **
    **                  PIECE                    **
    **                                           **
    **********************************************/

    public APiece Piece
    {
        get
        {
            return piece;
        }
        set
        {
            if (!value)
            {
                piece = null;
            }
            else
            {
                if (piece)
                {
                    piece.ParentTile = null;
                }

                piece = value;

                if (this.piece.ParentTile)
                {
                    this.piece.ParentTile.Piece = null;
                }
                piece.ParentTile = this;
            }
        }
    }

    /**********************************************
    **                                           **
    **            PLAYER INTERACTION             **
    **                                           **
    **********************************************/

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && BoardData.Instance.CanPlayerInteract())
        {
            BoardInputManager.Instance.AddPieceToList(Piece);
        }
    }

    void OnMouseEnter()
    {
        if (Input.GetMouseButton(0) && BoardData.Instance.CanPlayerInteract())
        {
            BoardInputManager.Instance.AddPieceToList(Piece);
        }
    }


    /**********************************************
    **                                           **
    **             TILE GRAVITY                  **
    **                                           **
    **********************************************/

    /* Switches the pieces from tile to tile recursively
     * example: 0,1,2 and x are tiles
     *      0 x x
     *      1 x x
     *      2 x x
     * the piece in 0 will only go to 1 if 1 has no pieces, has gravity and if the piece in 0 is centered on 0
     * same will happen from 1 to 2
     */

    public void GravityHandler()
    {
        if (!Piece)
            return;

        // Only checks for gravity on the target tile if the piece has settled on its tile
        if (transform.position != Piece.transform.position)
            return;

        if (!GravityTargetTile || !GravityTargetTile.HasGravity)
            return;

        if (!GravityTargetTile.Piece)
        {
            GravityTargetTile.Piece = Piece;
            GravityHandler();
            return;
        }
    }
}
