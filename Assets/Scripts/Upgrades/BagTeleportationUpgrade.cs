using Player;

namespace Upgrades
{
    public class BagTeleportationUpgrade : Upgrade
    {
        public override void Activate()
        {
            PlayerController.Instance.harvestController.EnableHarvestTeleportation();
            CartController.Instance.EnableHarvestTeleportation();
            OnActivate();
        }
    }
}
