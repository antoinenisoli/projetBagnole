using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Array2DEditor;

public class PawnController : MonoBehaviour
{
    [SerializeField] LayerMask tileMask, finishLineMask;
    [SerializeField] Line startRow;
    Vector2Int currentCoordinate;
    bool firstMove = true, isFreezed = false;
    Board board;
    Camera cam;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        cam = Camera.main;
    }

    private void Start()
    {
        EventManager.Instance.onGameFreeze.AddListener(Freeze);
    }

    void Freeze(bool value)
    {
        isFreezed = value;
    }

    public void Initialize(Line line)
    {
        startRow = line;
    }

    void RotateTowards(Vector3 position)
    {
        Vector3 newRot = new Vector3();
        if (transform.position.x > position.x)
            newRot = new Vector3(0, -90, 0);
        else if (transform.position.x < position.x)
            newRot = new Vector3(0, 90, 0);

        if (transform.position.z > position.z)
            newRot = new Vector3(0, -180, 0);
        else if (transform.position.z < position.z)
            newRot = new Vector3(0, 0, 0);

        float duration = 0.1f;
        transform.DORotate(newRot, duration);
    }

    public void Activate(BoardTile tile)
    {
        tile.Activate();
        currentCoordinate = tile.Coordinates;
        MoveTo(tile.SlotPosition());
        transform.parent = tile.transform;
        tile.PushDown();
        EventManager.Instance.onNewMove.Invoke(currentCoordinate);
    }

    public void MoveTo(Vector3 newPos)
    {
        transform.DOKill();
        RotateTowards(newPos);
        transform.DOMove(newPos, 0.25f);
    }

    void Update()
    {
        if (isFreezed)
            return;
        if (Input.GetMouseButtonDown(0))
            OnClick();
    }

    void OnClick()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileMask) && hit.transform != null)
        {
            BoardTile tile = hit.transform.GetComponentInChildren<BoardTile>();
            if (tile)
                SelectTile(tile);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, finishLineMask) && hit.transform != null)
        {
            FinishLine finishLine = hit.transform.GetComponentInParent<FinishLine>();
            if (finishLine)
                SelectFinishLine(finishLine);
        }
    }

    private void SelectFinishLine(FinishLine finishLine)
    {
        Vector2Int lineCoordinates = new Vector2Int(currentCoordinate.x, 0);
        if (board.TargetTileIsNeighbour(currentCoordinate, lineCoordinates))
            finishLine.CheckVictory(lineCoordinates);
    }

    private void SelectTile(BoardTile tile)
    {
        if (!tile.Activated)
        {
            if (firstMove)
            {
                if (board.LastRow(tile.Coordinates) && startRow.Row.GetCell(tile.Coordinates.x, 0))
                {
                    Activate(tile);
                    firstMove = false;
                }
                else
                    tile.Shake();
            }
            else
            {
                if (board.TargetTileIsNeighbour(currentCoordinate, tile.Coordinates))
                    Activate(tile);
                else
                    tile.Shake();
            }
        }
    }
}
