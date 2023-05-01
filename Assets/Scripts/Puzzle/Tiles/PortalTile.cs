using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTile : BoardTile
{
    PortalTile OtherPortal()
    {
        var v = FindObjectsOfType<PortalTile>();
        foreach (var item in v)
        {
            if (item != this)
                return item;
        }

        return null;
    }

    public override void Activate()
    {
        base.Activate();
        print(OtherPortal());
        FindObjectOfType<PawnController>().MoveTo(OtherPortal().SlotPosition());
    }
}
