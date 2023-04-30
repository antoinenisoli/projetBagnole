using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public class Board : MonoBehaviour
{
    [SerializeField] PawnController pawn;

    [Header("GRID CREATION")]
    [SerializeField] Vector3 firstCellPosition = new Vector3(-3, 0, 2);
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int cellSize;
    [SerializeField] Array2DInt array;
    BoardTile[,] tiles;

    private void Awake()
    {
        CreateGrid();
    }

    private void Start()
    {
        EventManager.Instance.onVictoryCheck.AddListener(CheckVictory);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(firstCellPosition, 0.5f);
    }
#endif

    public void CheckVictory()
    {
        if (AllCellsActivated())
        {
            print("victory");
            EventManager.Instance.onVictory.Invoke();
        }
    }

    public Vector2Int BoardSize()
    {
        return new Vector2Int(tiles.GetLength(0), tiles.GetLength(1));
    }

    void CreateGrid()
    {
        tiles = new BoardTile[array.GridSize.x, array.GridSize.y];
        for (int i = 0; i < array.GridSize.x; i++)
        {
            for (int j = 0; j < array.GridSize.y; j++)
            {
                int value = array.GetCell(i, j);
                if (value == 1)
                {
                    Vector3 spawnPos = firstCellPosition + (new Vector3(i, 0, j) * cellSize);
                    GameObject newTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                    Vector2Int coordinate = new Vector2Int(i, j);
                    newTile.name = tilePrefab.name + coordinate;

                    BoardTile boardTile = newTile.GetComponent<BoardTile>();
                    boardTile.Coordinates = coordinate;
                    tiles[i, j] = boardTile;
                }
            }
        }
    }

    public bool CoordinateIsValid(Vector2Int coordinate)
    {
        if (coordinate.x > tiles.GetLength(0) - 1 || coordinate.x < 0)
            return false;

        if (coordinate.y > tiles.GetLength(1) - 1 || coordinate.y < 0)
            return false;

        return true;
    }

    public bool Adjacent(Vector2Int baseCoord, Vector2Int targetCoord)
    {
        for (int x = -1; x < 2; x++) //check horizontal line
        {
            Vector2Int offset = new Vector2Int(x, 0);
            if (Check(offset, baseCoord, targetCoord))
                return true;
        }

        for (int y = -1; y < 2; y++) //check vertical line
        {
            Vector2Int offset = new Vector2Int(0, y);
            if (Check(offset, baseCoord, targetCoord))
                return true;
        }

        return false;
    }

    bool Check(Vector2Int offset, Vector2Int baseCoord, Vector2Int targetCoord)
    {
        Vector2Int checkCoordinate = baseCoord + offset;
        if (checkCoordinate == targetCoord)
            return true;

        return false;
    }

    public bool LastRow(Vector2Int coord)
    {
        return coord.y == 0;
    }

    public bool AllCellsActivated()
    {
        foreach (var item in tiles)
        {
            if (!item.Activated)
                return false;
        }

        return true;
    }

    public void ActivateTile(BoardTile tile)
    {
        pawn.transform.position = tile.SlotPosition();
    }
}
