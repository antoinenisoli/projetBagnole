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
        EventManager.Instance.onVictory.AddListener(
            (int index) => 
            { 
                StartCoroutine(Victory(index)); 
            } 
            );
    }

    public void CheckVictory(Vector2Int coord)
    {
        if (row.GetCell(coord.x, 0))
            EventManager.Instance.onVictoryCheck.Invoke(coord.x);
    }

    IEnumerator Victory(int index)
    {
        FindObjectOfType<PawnController>().MoveTo(slots[index].position);
        yield return new WaitForSeconds(2f);
        EventManager.Instance.onNextLevel.Invoke();
    }
}
