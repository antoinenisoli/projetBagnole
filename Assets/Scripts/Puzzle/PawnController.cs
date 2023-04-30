using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Array2DEditor;

public class PawnController : MonoBehaviour
{
    [SerializeField] LayerMask tileMask, finishLineMask;
    [SerializeField] Array2DBool oui;
    Vector2Int currentCoordinate;
    bool firstMove = true;
    Board board;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    void MoveTo(BoardTile tile)
    {
        tile.Activate();
        transform.DOMove(tile.SlotPosition(), 0.25f);
        currentCoordinate = tile.Coordinates;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClick();
    }

    void OnClick()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileMask) && hit.transform != null)
        {
            BoardTile tile = hit.transform.GetComponentInParent<BoardTile>();
            if (tile)
                SelectTile(tile);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, finishLineMask) && hit.transform != null)
        {
            FinishLine finishLine = hit.transform.GetComponentInParent<FinishLine>();
            if (finishLine)
            {
                Vector2Int lineCoordinates = new Vector2Int(currentCoordinate.x, board.BoardSize().y);
                if (board.Adjacent(currentCoordinate, lineCoordinates))
                    finishLine.CheckVictory(lineCoordinates);
            }
        }
    }

    private void SelectTile(BoardTile tile)
    {
        if (!tile.Activated)
        {
            if (firstMove)
            {
                if (board.LastRow(tile.Coordinates))
                {
                    MoveTo(tile);
                    firstMove = false;
                }
            }
            else
            {
                if (board.Adjacent(currentCoordinate, tile.Coordinates))
                    MoveTo(tile);
            }
        }
    }
}
