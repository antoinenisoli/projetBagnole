using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTile : BoardTile
{
    [SerializeField] float delayDuration = 2f;
    BoardTile targetPortal;

    public void SetTarget(BoardTile target)
    {
        targetPortal = target;
    }

    public override void Activate()
    {
        base.Activate();
        StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        EventManager.Instance.onGameFreeze.Invoke(true);
        yield return new WaitForSeconds(delayDuration);
        FindObjectOfType<PawnController>().Activate(targetPortal);
        yield return new WaitForSeconds(0.5f);
        EventManager.Instance.onGameFreeze.Invoke(false);
    }
}
