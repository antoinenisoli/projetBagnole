using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Array2DEditor;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject[] helpers;

    [Header("GRID CREATION")]
    [SerializeField] Vector3 firstCellPosition = new Vector3(-3, 0, 2);
    [SerializeField] Transform container;
    [SerializeField] int cellSize;
    [SerializeField] GameObject[] tilePrefabs;
    [SerializeField] Array2DInt array;
    BoardTile[,] tiles;

    public BoardTile[,] Tiles { get => tiles; }

    private void Awake()
    {
        CreateGrid();
    }

    private void Start()
    {
        EventManager.Instance.onVictoryCheck.AddListener(CheckVictory);
        EventManager.Instance.onNewMove.AddListener(NewMove);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < array.GridSize.x; i++)
        {
            for (int j = 0; j < array.GridSize.y; j++)
            {
                int value = array.GetCell(i, j);
                if (value == 0)
                    return;

                if (value == 1)
                    Gizmos.color = Color.yellow;
                else if (value == 2)
                    Gizmos.color = Color.green;

                Vector3 spawnPos = firstCellPosition + (new Vector3(i, 0, j) * cellSize) + new Vector3(0.5f, 0, -0.5f);
                Gizmos.DrawWireCube(spawnPos, Vector3.one);
            }
        }
    }
#endif

    public void CheckVictory(int index)
    {
        if (AllCellsActivated())
        {
            print("victory");
            EventManager.Instance.onVictory.Invoke(index);
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
                if (value != 0)
                {
                    Vector3 spawnPos = firstCellPosition + (new Vector3(i, 0, j) * cellSize);
                    GameObject prefab = tilePrefabs[value - 1];
                    GameObject newTile = Instantiate(prefab, spawnPos, Quaternion.identity, container);
                    Vector2Int coordinate = new Vector2Int(i, j);
                    newTile.name = prefab.name + coordinate;

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

    public bool TargetTileIsNeighbour(Vector2Int baseCoord, Vector2Int targetCoord)
    {
        bool IsNeighbour(Vector2Int offset, Vector2Int baseCoord, Vector2Int targetCoord)
        {
            Vector2Int checkCoordinate = baseCoord + offset;
            if (checkCoordinate == targetCoord)
                return true;

            return false;
        }

        for (int x = -1; x < 2; x++) //check horizontal line
        {
            Vector2Int offset = new Vector2Int(x, 0);
            if (IsNeighbour(offset, baseCoord, targetCoord))
                return true;
        }

        for (int y = -1; y < 2; y++) //check vertical line
        {
            Vector2Int offset = new Vector2Int(0, y);
            if (IsNeighbour(offset, baseCoord, targetCoord))
                return true;
        }

        return false;
    }

    bool HasNeighbours(Vector2Int coordinate)
    {
        void Set(Vector2Int offset, List<BoardTile> neighbours)
        {
            Vector2Int newCoord = coordinate + offset;
            if (CoordinateIsValid(newCoord))
            {
                BoardTile tile = tiles[newCoord.x, newCoord.y];
                if (tile && !tile.Activated)
                    neighbours.Add(tile);
            }
        }

        List<BoardTile> neighbours = new List<BoardTile>();
        for (int x = -1; x < 2; x++) //check horizontal line
        {
            Vector2Int offset = new Vector2Int(x, 0);
            if (x != 0)
                Set(offset, neighbours);
        }

        for (int y = -1; y < 2; y++) //check vertical line
        {
            Vector2Int offset = new Vector2Int(0, y);
            if (y != 0)
                Set(offset, neighbours);
        }

        return neighbours.Count > 0;
    }

    public bool LastRow(Vector2Int coord)
    {
        return coord.y == 0;
    }

    public bool AllCellsActivated()
    {
        foreach (var item in tiles)
            if (!item.Activated)
                return false;

        return true;
    }

    bool OnFinishLine(Vector2Int coordinate)
    {
        return (coordinate.y == tiles.GetLength(1) - 1)
            && AllCellsActivated()
            && FindObjectOfType<FinishLine>().CanReach(coordinate);
    }

    void NewMove(Vector2Int coordinate)
    {
        foreach (var item in helpers)
            item.SetActive(false);

        if (HasNeighbours(coordinate))
            PlaceHelpers(coordinate);
        else if (!OnFinishLine(coordinate))
        {
            print("defeat");
            EventManager.Instance.onGameOver.Invoke();
        }
    }

    public void PlaceHelpers(Vector2Int coordinate)
    {
        void Set(Vector2Int newCoord, Queue<GameObject> helperQueue)
        {
            BoardTile tile = tiles[newCoord.x, newCoord.y];
            if (tile && !tile.Activated)
            {
                GameObject helper = helperQueue.Dequeue();
                helper.SetActive(true);
                helper.transform.position = tile.SlotPosition();
            }
        }

        Queue<GameObject> helperQueue = new Queue<GameObject>(helpers);
        for (int x = -1; x < 2; x++) //check horizontal line
        {
            Vector2Int offset = new Vector2Int(x, 0);
            Vector2Int newCoord = coordinate + offset;
            if (CoordinateIsValid(newCoord) && x != 0)
                Set(newCoord, helperQueue);
        }

        for (int y = -1; y < 2; y++) //check vertical line
        {
            Vector2Int offset = new Vector2Int(0, y);
            Vector2Int newCoord = coordinate + offset;
            if (CoordinateIsValid(newCoord) && y != 0)
                Set(newCoord, helperQueue);
        }
    }
}
