using UnityEngine;

namespace Upgrades
{
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
}
