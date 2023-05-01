using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Board board;
    [SerializeField] int positionOffset = 1, orthographicSizeOffset = 2;

    private void Start()
    {
        PlaceOnBoardCenter();
        print(board.BoardSize().x);
        if (board.BoardSize().x > 3)
            cam.orthographicSize += orthographicSizeOffset * (board.BoardSize().x - 3);
    }

    private void PlaceOnBoardCenter()
    {
        Vector3 middlePos = new Vector3();
        foreach (var item in board.Tiles)
            if (item)
                middlePos += item.transform.position;

        middlePos /= (float)board.Tiles.Length;
        middlePos.x += positionOffset;

        Vector3 newPos = cam.transform.position;
        newPos.x = middlePos.x;
        cam.transform.position = newPos;
    }
}
