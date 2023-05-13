using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Upgrades
{
    public class EndGameUpdrade : Upgrade
    {
        [SerializeField] private GameObject endGameCanvas;

        public override void Activate()
        {
            PlayerController.Instance.movementController.DisableMovement();
            endGameCanvas.SetActive(true);
        }
    }
}