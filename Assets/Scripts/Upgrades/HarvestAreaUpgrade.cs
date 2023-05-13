
namespace Upgrades
{
    public class HarvestAreaUpgrade : Upgrade
    {
        public override void Activate()
        {
            WheatTarget.UpgradeLevel++;
            OnActivate();
        }
    }
}
