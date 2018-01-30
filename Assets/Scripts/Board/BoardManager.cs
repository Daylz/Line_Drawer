using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    private List<Tile> Spawners = new List<Tile>();

    public void Awake()
    {
        Instance = this;
    }

    /**********************************************
    **                                           **
    **               GAME START                  **
    **                                           **
    **********************************************/

    public void StartGame()
    {
        CameraAlign.Instance.AlignToBoard();

        StopAllCoroutines();

        InitSpawnerList();
        BoardData.Instance.IsGameStarted = true;

        StartCoroutine(GameRoutine());
    }    

    public void Reset()
    {
        StopAllCoroutines();

        Spawners = new List<Tile>();
        BoardData.Instance.IsGameStarted = false;
    }

    /**********************************************
    **                                           **
    **              MAIN GAME LOOP               **
    **                                           **
    **********************************************/

    IEnumerator GameRoutine()
    {
        while (!BoardData.Instance.IsObjectiveReached)
        {
            yield return StartCoroutine(MyUtils.WaitFor(BoardData.Instance.CanPlayerInteract, 0.2f));
        }

        // Ending the session, showing win popup
        //ShowWinPopup();
        Debug.Log("You win!");
    }

    void Update()
    {
        // Only checks for refill if pieces are moving (the player cannot interact with the board while pieces are moving)
        if (!BoardData.Instance.CanPlayerInteract())
        {
            Refill();
        }
    }

    /**********************************************
    **                                           **
    **                  REFILL                   **
    **                                           **
    **********************************************/

    private void InitSpawnerList()
    {
        for (int x = 0; x < BoardData.Instance.Width; x++)
        {
            for (int y = 0; y < BoardData.Instance.Height; y++)
            {
                Tile tile = BoardData.Instance.GetTileAt(x, y);

                if (tile && tile.Spawner)
                {
                    Spawners.Add(tile);
                }
            }
        }
    }

    private void Refill()
    {
        Vector2 offset;

        foreach (Tile tile in Spawners)
        {
            if (tile && !tile.Piece && tile.Spawner && tile.HasGravity)
            {
                offset = MyUtils.OffsetFromGravityDirection(tile.GravityDirection);
                BoardCreator.Instance.CreatePieceAt(tile.Coordinates.x, tile.Coordinates.y, UnityEngine.Random.Range(0, BoardData.Instance.Colors), offset.x, offset.y);
            }
        }
    }

    /**********************************************
    **                                           **
    **                  PIECES                   **
    **                                           **
    **********************************************/

    public void DestroyPieces(List<APiece> selectedPieces)
    {
        Point coords = new Point(0, 0);

        foreach (APiece piece in selectedPieces)
        {
            coords.x = piece.ParentTile.Coordinates.x;
            coords.y = piece.ParentTile.Coordinates.y;
            piece.DestroyPiece();            
        }

        if (selectedPieces.Count > 4)
        {
            BoardCreator.Instance.CreateBomb(coords.x, coords.y);
        }

        Refill();
    }
}
