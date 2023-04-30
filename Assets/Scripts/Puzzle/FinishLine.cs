using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public class FinishLine : MonoBehaviour
{
    [SerializeField] Array2DBool finishRow;

    public void CheckVictory(Vector2Int coord)
    {
        if (finishRow.GetCell(coord.x, 0))
            EventManager.Instance.onVictoryCheck.Invoke();
    }
}
