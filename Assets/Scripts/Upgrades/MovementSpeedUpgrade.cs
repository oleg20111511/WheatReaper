using UnityEngine;
using Player;

namespace Upgrades
{
    public class MovementSpeedUpgrade : Upgrade
    {
        [SerializeField] private float speedIncrease;

        public override void Activate()
        {
            PlayerController.Instance.movementController.MovementSpeed += speedIncrease;
            OnActivate();
        }
    }
}
