
namespace Upgrades
{
    public class FertilityUpgrade : Upgrade
    {
        public override void Activate()
        {
            WheatController.WheatPerField += 1;
            OnActivate();
        }
    }
}
