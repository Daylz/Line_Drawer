using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    public static BoardCreator Instance;

    private BoardData BoardData;

    public Text LevelName;

    public int LevelIndex = 0;
    public ALevel[] Levels;
    public ALevel CurrentLevel;

    // Tile creation settings
    public float TileSize = 0.75f;
    public GameObject TilePrefab;
    private GameObject tileHolder;
    private Transform tileHolderTransform;

    // Piece
    public GameObject[] PiecePrefabs;

    // Bombs
    public GameObject[] bombPrefabs;
    public Dictionary<string, GameObject> bombs = new Dictionary<string, GameObject>();

    // Pool
    public ObjectPooler PiecePooler;

    void Awake()
    {
        if (Levels.Length <= 0)
        {
            throw new SystemException("You forgot to pass the levels in the level parameter in the BoardCreator!");
        }
        if (PiecePrefabs.Length <= 0)
        {
            throw new SystemException("You forgot to the pieces to the piecePrefabs parameter in the BoardCreator!");
        }

        Instance = this;
    }
    
    void Start()
    {
        CreateBombDictionary();
        Init();
    }

    private void Init()
    {
        BoardData = BoardData.Instance;
        CurrentLevel = Levels[LevelIndex];
        LevelName.text = "Level Name: " + CurrentLevel.gameObject.name;
        SetData();        
        CreateTiles();
        CreatePieces();

        BoardManager.Instance.StartGame();
    }   

    public void ResetLevel()
    {
        Destroy(tileHolder);
        PiecePooler.DeactivateAllObjects();
        BoardData.Reset();
        BoardManager.Instance.Reset();
        Init();
    }

    public void SwitchLevel()
    {
        if (LevelIndex >= Levels.Length - 1)
        {
            LevelIndex = 0;
        }
        else
        {
            LevelIndex++;
        }

        ResetLevel();
    }

    private void SetData()
    {
        BoardData.Width = CurrentLevel.Width;
        BoardData.Height = CurrentLevel.Height;
        BoardData.Colors = CurrentLevel.Colors;
        BoardData.Moves = CurrentLevel.Moves;
    }

    /**********************************************
    **                                           **
    **                  TILES                    **
    **                                           **
    **********************************************/

    private void CreateTiles()
    {
        CreateTileHolder();

        BoardData.Tiles = new Tile[BoardData.Height, BoardData.Width];

        for (int y = 0; y < BoardData.Height; y++)
        {
            for (int x = 0; x < BoardData.Width; x++)
            {
                if (CurrentLevel.Tiles[y, x] > 0)
                {
                    CreateTileAt(x, y);
                }
            }
        }

        UpdateTileNeighbours();
    }

    private void CreateTileHolder()
    {
        tileHolder = new GameObject("Tiles");
        tileHolderTransform = tileHolder.transform;
        tileHolderTransform.position = Vector3.zero;
        tileHolderTransform.parent = this.transform;
    }

    private void CreateTileAt(int x, int y)
    {
        Vector3 position = new Vector3(this.transform.position.x + x * TileSize, this.transform.position.y - y * TileSize, 0);
        GameObject obj = Instantiate(TilePrefab, position, Quaternion.identity) as GameObject;
        Tile tile = obj.GetComponent<Tile>();

        BoardData.Tiles[y, x] = tile;
        BoardData.Tiles[y, x].SetCoords(x, y);
        obj.transform.parent = tileHolderTransform;
        obj.name = "Tile" + x + "x" + y;

        if (CurrentLevel.Spawners[y, x] == 1)
        {
            BoardData.Tiles[y, x].Spawner = true;
        }

        BoardData.Tiles[y, x].HasGravity = true;
        BoardData.Tiles[y, x].GravityDirection = (EGravityDirection) CurrentLevel.Gravity[y, x];
    }

    private void UpdateTileNeighbours()
    {
        int x, y = 0;

        foreach (Tile tile in BoardData.Tiles)
        {
            if (!tile)
            {
                continue;
            }

            x = tile.Coordinates.x;
            y = tile.Coordinates.y;

            tile.TopLeft = BoardData.GetTileAt(x - 1, y - 1);
            tile.Top = BoardData.GetTileAt(x, y - 1);
            tile.TopRight = BoardData.GetTileAt(x + 1, y - 1);
            tile.Right = BoardData.GetTileAt(x + 1, y);
            tile.BottomRight = BoardData.GetTileAt(x + 1, y + 1);
            tile.Bottom = BoardData.GetTileAt(x, y + 1);
            tile.BottomLeft = BoardData.GetTileAt(x - 1, y + 1);
            tile.Left = BoardData.GetTileAt(x - 1, y);
        }
    }

    /**********************************************
    **                                           **
    **                  PIECES                   **
    **                                           **
    **********************************************/

    private void CreatePieces()
    {
        for (int y = 0; y < CurrentLevel.Height; y++)
        {
            for (int x = 0; x < CurrentLevel.Width; x++)
            {
                if (BoardData.Tiles[y, x])
                {
                    CreatePieceAt(x, y, UnityEngine.Random.Range(0, BoardData.Colors));
                }
            }
        }
    }

    public void CreatePieceAt(int x, int y, int color, float offsetX = 0f, float offsetY = 0f)
    {
        Tile tile = BoardData.GetTileAt(x, y);

        if (tile && tile.Piece == null)
        {
            Transform tileTransform = tile.transform;
            Vector3 tilePosition = new Vector3(tileTransform.position.x + offsetX, tileTransform.position.y + offsetY, tileTransform.position.z);
            GameObject obj = PiecePooler.GetObjectOfType(PiecePrefabs[color].name);
            obj.transform.position = tilePosition;
            obj.SetActive(true);
            APiece piece = obj.GetComponent<APiece>();
            tile.Piece = piece;
        }
    }

    /**********************************************
    **                                           **
    **                  BOMBS                    **
    **                                           **
    **********************************************/

    private void CreateBombDictionary()
    {
        bombs = new Dictionary<string, GameObject>();

        foreach (GameObject bomb in bombPrefabs)
        {
            bombs.Add(bomb.name, bomb);
        }
    }

    public void CreateBomb(int x, int y)
    {
        Tile tile = BoardData.GetTileAt(x, y);

        GameObject bomb = PiecePooler.GetObjectOfType("Piece_Bomb");
        bomb.transform.position = tile.transform.position;
        bomb.SetActive(true);
        APiece piece = bomb.GetComponent<APiece>();
        tile.Piece = piece;
    }
}
