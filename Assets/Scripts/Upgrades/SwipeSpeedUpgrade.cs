using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSpeedUpgrade : Upgrade
{
    private float currentSpeed = 1f;


    public override void Activate()
    {
        currentSpeed += 0.25f;
        PlayerController.Instance.GetComponent<Animator>().SetFloat("SwipeSpeed", currentSpeed);
        level += 1;
    }
}

