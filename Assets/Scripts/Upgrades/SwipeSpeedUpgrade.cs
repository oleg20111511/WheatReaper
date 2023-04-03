using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

