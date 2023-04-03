using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilityUpgrade : Upgrade
{
    public override void Activate()
    {
        WheatController.WheatPerField += 1;
        OnActivate();
    }
}
