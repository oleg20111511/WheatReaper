using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilizerUpgrade : Upgrade
{
    [SerializeField] private int growthDurationDecrease = 1000;

    public override void Activate()
    {
        foreach (WheatController wheatController in WheatController.AllWheatControllers)
        {
            wheatController.WheatGrowth.ChangeGrowthDuration(-growthDurationDecrease);
        }
        OnActivate();
    }
}
