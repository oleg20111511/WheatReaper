using UnityEngine;
using Player;

namespace Upgrades
{
    public class SwipeSpeedUpgrade : Upgrade
    {
        [SerializeField] private float speedIncrease = 0.25f;
        private float currentSpeed = 1f;

        public override void Activate()
        {
            speedIncrease += 0.25f;
            PlayerController.Instance.GetComponent<Animator>().SetFloat("SwipeSpeed", currentSpeed);
            
            OnActivate();
        }
    }
}
