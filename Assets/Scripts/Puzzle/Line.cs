using Array2DEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] protected GameObject tilePrefab;
    [SerializeField] protected int spacing = 2;
    [SerializeField] Color color = Color.green;
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] protected Array2DBool row;
    protected BoxCollider col;
    protected List<Transform> slots = new List<Transform>();
    protected Vector3 lastPosition;

    public Array2DBool Row => row; 


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + new Vector3(-spacing, 0, 0);
        for (int i = 0; i < row.GridSize.x; i++)
        {
            Color gizmoColor = row.GetCell(i, 0) ? color : Color.red;
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(pos, Vector3.one * spacing);

            gizmoColor.a = 0.5f;
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(pos, Vector3.one * spacing);
            pos.x += spacing;
        }
    }
#endif
    public virtual void Awake()
    {
        col = GetComponent<BoxCollider>();
        BuildLine();
    }

    public void BuildLine()
    {
        lastPosition = new Vector3(-spacing, 0, 0);
        for (int i = 0; i < row.GridSize.x; i++)
            CreateTile(i);
    }

    public virtual void CreateTile(int i)
    {
        GameObject lineTile = Instantiate(tilePrefab, transform);
        slots.Add(lineTile.transform);
        lineTile.transform.localPosition = lastPosition;
        lastPosition.x += spacing;

        if (!row.GetCell(i, 0))
        {
            GameObject random = GameDevHelper.RandomObject<GameObject>(obstaclePrefabs);
            GameObject obstacle = Instantiate(random, lineTile.transform);
            obstacle.transform.localPosition = new Vector3();
        }
    }
}
