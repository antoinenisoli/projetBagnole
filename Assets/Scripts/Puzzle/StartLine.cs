using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : Line
{
    [Header(nameof(StartLine))]
    [SerializeField] int startIndex = 2;
    [SerializeField] GameObject playerPrefab;

    private void OnValidate()
    {
        if (startIndex > row.GridSize.x - 1)
            startIndex = row.GridSize.x - 1;
        else if (startIndex < 0)
            startIndex = 0;
    }

    public void Start()
    {
        var pawnObj = Instantiate(playerPrefab, slots[startIndex].position, Quaternion.identity);
        var pawn = pawnObj.GetComponent<PawnController>();
        pawn.Initialize(this);
    }
}
