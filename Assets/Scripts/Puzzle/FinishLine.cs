using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public class FinishLine : Line
{
    //[Header(nameof(FinishLine))]

    public override void Start()
    {
        base.Start();
        EventManager.Instance.onVictory.AddListener(Victory);
    }

    public void CheckVictory(Vector2Int coord)
    {
        if (row.GetCell(coord.x, 0))
            EventManager.Instance.onVictoryCheck.Invoke(coord.x);
    }

    void Victory(int index)
    {
        FindObjectOfType<PawnController>().MoveTo(slots[index].position);
    }
}
