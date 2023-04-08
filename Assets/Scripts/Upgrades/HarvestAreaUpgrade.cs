using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestAreaUpgrade : Upgrade
{
    public override void Activate()
    {
        WheatTarget.UpgradeLevel++;
        OnActivate();
    }
}
