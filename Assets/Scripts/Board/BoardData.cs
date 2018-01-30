using UnityEngine;
using System.Collections;
using System;

public class BoardData : MonoBehaviour
{
    public static BoardData Instance = null;
    
    public int Width { get; set; }
    public int Height { get; set; }
    public int MaxWidth = 9;
    public int MaxHeight = 9;
    public int Colors { get; set; }
    public Tile[,] Tiles { get; set; }

    // Board State
    public int Moving;

    // Gravity
    public float Acceleration = 5.0f;
    public float MaxVelocity = 8.0f;

    // Game state
    public bool IsGameStarted { get; set; }
    public bool IsObjectiveReached { get; set; }

    public int Score { get; set; }
    public int Moves { get; set; }

    public void Awake()
    {
        Instance = this;
    }

    public void Reset()
    {
        Moving = 0;
        Score = 0;
        Moves = 0;
        IsGameStarted = false;
        IsObjectiveReached = false;
    }

    public Tile GetTileAt(int x, int y)
    {
        if (!(x >= 0 && x < this.Width && y >= 0 && y < this.Height))
        {
            return null;
        }

        return Tiles[y, x];
    }

    // Conditions for the player to input a move
    public bool CanPlayerInteract()
    {
        return Moving == 0 && IsGameStarted;
    }
}
