using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    public class CartSizeUpgrade : Upgrade
    {
        public List<int> sizeByLevel = new List<int>();


        public override void Activate()
        {
            CartController.Instance.Capacity = sizeByLevel[CurrentLevel];
            OnActivate();
        }
    }
}
