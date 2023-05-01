using Array2DEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] protected GameObject tilePrefab;
    [SerializeField] protected int spacing = 2;
    [SerializeField] protected Array2DBool row;
    protected BoxCollider col;
    protected List<Transform> slots = new List<Transform>();

    public Array2DBool Row => row; 


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + new Vector3(-spacing, 0, 0);
        for (int i = 0; i < row.GridSize.x; i++)
        {
            Color gizmoColor = row.GetCell(i, 0) ? Color.green : Color.red;
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

    public virtual void Start()
    {
        
    }

    private void BuildLine()
    {
        Vector3 pos = new Vector3(-spacing, 0, 0);
        for (int i = 0; i < row.GridSize.x; i++)
        {
            GameObject o = Instantiate(tilePrefab, transform);
            slots.Add(o.transform);
            o.transform.localPosition = pos;
            pos.x += spacing;
        }
    }
}
