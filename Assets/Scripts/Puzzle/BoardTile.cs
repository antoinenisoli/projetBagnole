using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public bool Activated;
    [SerializeField] Transform visual;
    [SerializeField] Material activeMaterial, unactiveMaterial;
    MeshRenderer meshRenderer;
    Board board;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public Vector3 SlotPosition()
    {
        return visual.position;
    }

    private void OnMouseDown()
    {
        if (Activated)
            return;

        Activated = true;
        meshRenderer.material = Activated ? activeMaterial : unactiveMaterial;
        board.ActivateTile(this);
    }
}
