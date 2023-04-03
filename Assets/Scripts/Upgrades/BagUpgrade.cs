using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagUpgrade : Upgrade
{
    [SerializeField] private int[] bagSizes;  // index represents (level - 1), value represents size

    public override void Activate()
    {
        PlayerController.Instance.harvestController.BagSize = bagSizes[level];
        OnActivate();        
    }
}
