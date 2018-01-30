using UnityEngine;
using System.Collections;

public abstract class ALevel : MonoBehaviour
{
    public int Width;
    public int Height;

    public int Colors;
    public int Moves;

    public int[,] Tiles;
    public int[,] Spawners;
    public int[,] Gravity;
    public int[,] Piece;

    public LevelObjectives LevelObjective;
    public int LevelObjectiveAmount;
}