using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameUpdrade : Upgrade
{
    [SerializeField] private GameObject endGameCanvas;

    public override void Activate()
    {
        PlayerController.Instance.movementController.DisableMovement();
        endGameCanvas.SetActive(true);
    }
}
