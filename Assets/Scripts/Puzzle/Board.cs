using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    [SerializeField] PawnController pawn;

    [Header("GRID CREATION")]
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int cellSize;
    BoardTile[,] tiles;

    private void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        GameObject tile = Instantiate(tilePrefab);
    }

    public void ActivateTile(BoardTile tile)
    {
        pawn.transform.position = tile.SlotPosition();
    }
}
