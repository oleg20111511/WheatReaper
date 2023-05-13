using UnityEngine;
using Player;

namespace Upgrades
{
    public class BagUpgrade : Upgrade
    {
        [SerializeField] private int[] bagSizes;  // index represents (level - 1), value represents size

        public override void Activate()
        {
            PlayerController.Instance.harvestController.BagSize = bagSizes[level];
            OnActivate();        
        }
    }
}
