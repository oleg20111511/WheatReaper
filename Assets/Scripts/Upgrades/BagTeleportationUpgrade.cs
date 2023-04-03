using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagTeleportationUpgrade : Upgrade
{
    public override void Activate()
    {
        PlayerController.Instance.harvestController.EnableHarvestTeleportation();
        CartController.Instance.EnableHarvestTeleportation();
        OnActivate();
    }
}
