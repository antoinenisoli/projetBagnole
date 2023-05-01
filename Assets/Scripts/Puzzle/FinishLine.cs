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
        if (CanReach(coord))
            EventManager.Instance.onVictoryCheck.Invoke(coord.x);
    }

    public bool CanReach(Vector2Int coord)
    {
        return row.GetCell(coord.x, 0);
    }

    void Victory(int index)
    {
        FindObjectOfType<PawnController>().MoveTo(slots[index].position);
    }
}
