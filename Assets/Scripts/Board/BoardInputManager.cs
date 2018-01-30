using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// Handles input and feedback of what is selected 
public class BoardInputManager : MonoBehaviour
{
    public static BoardInputManager Instance;

    public List<APiece> SelectedPieces = new List<APiece>();

    public void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Mouse
        if (Input.GetMouseButtonUp(0))
        {
            HandleInteractionEnd();
        }

        // Touch
        if (Input.touchCount > 0)
        {
            Debug.Log("Touch");

            Touch touch = Input.GetTouch(0);

            TouchHandler(touch);

            if (touch.phase == TouchPhase.Ended)
            {
                HandleInteractionEnd();
            }
        }
    }

    private void TouchHandler(Touch touch)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit ;

            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = hit.transform.gameObject.GetComponent<Tile>();

                AddPieceToList(tile.Piece);
            }
        }
    }

    private void HandleInteractionEnd()
    {
        if (SelectedPieces.Count == 1 && SelectedPieces[0].GetComponent<PieceData>().Type == EPieceType.Bomb)
        {
            BoardManager.Instance.DestroyPieces(SelectedPieces);
        }
        if (SelectedPieces.Count >= 3)
        {
            BoardManager.Instance.DestroyPieces(SelectedPieces);
        }

        ClearList();
    }

    public void AddPieceToList(APiece piece)
    {
        if (SelectedPieces.Contains(piece))
        {
            int indexOfDuplicate = SelectedPieces.IndexOf(piece);
            SelectedPieces.RemoveRange(indexOfDuplicate + 1, SelectedPieces.Count - indexOfDuplicate - 1);
        }
        else
        {
            if (SelectedPieces.Count >= 1)
            {
                PieceData previousPieceData = SelectedPieces[0].GetComponent<PieceData>();
                PieceData currentPieceData = piece.GetComponent<PieceData>();

                if (previousPieceData.Color == currentPieceData.Color && MyUtils.IsTileAdjacent(SelectedPieces[SelectedPieces.Count - 1].ParentTile, piece.ParentTile) && previousPieceData.Type != EPieceType.Bomb)
                {
                    SelectedPieces.Add(piece);
                }
            }
            else
            {
                SelectedPieces.Add(piece);
            }
        }

        UpdateHighlight();
    }

    public void ClearList()
    {
        SelectedPieces.Clear();
        UpdateHighlight();
    }

    void UpdateHighlight()
    {
        ClearHighlight();

        for (int i = 0; i < SelectedPieces.Count; i++)
        {
            SelectedPieces[i].ParentTile.Highlight.SetActive(true);
        }
    }   
    
    void ClearHighlight()
    {
        Tile tile;

        for (int x = 0; x < BoardData.Instance.Width; x++)
        {
            for (int y = 0; y < BoardData.Instance.Height; y++)
            {
                tile = BoardData.Instance.GetTileAt(x, y);
                
                if (tile)
                {
                    tile.Highlight.SetActive(false);
                }                
            }
        }
    } 
}
