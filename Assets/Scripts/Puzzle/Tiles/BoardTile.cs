using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public bool Activated;
    private Vector2Int coordinates;
    [SerializeField] Transform visual;
    [SerializeField] Material activeMaterial, unactiveMaterial;
    MeshRenderer meshRenderer;

    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;
        style.fontSize = 20;
        UnityEditor.Handles.Label(transform.position, $"{coordinates.x},{coordinates.y}", style);
    }
#endif

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public Vector3 SlotPosition()
    {
        return visual.position;
    }

    public void Shake()
    {
        transform.DOShakePosition(0.2f, 0.25f, 90);
        transform.DOShakeScale(0.2f, 0.25f, 90);
    }

    public virtual void Activate()
    {
        if (Activated)
            return;

        Activated = true;
        meshRenderer.material = Activated ? activeMaterial : unactiveMaterial;
    }
}
