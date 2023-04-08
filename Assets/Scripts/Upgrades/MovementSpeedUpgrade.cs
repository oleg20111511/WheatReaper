using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedUpgrade : Upgrade
{
    [SerializeField] private float speedIncrease;

    public override void Activate()
    {
        PlayerController.Instance.movementController.MovementSpeed += speedIncrease;
        OnActivate();
    }
}
