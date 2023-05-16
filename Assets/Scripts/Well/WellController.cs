using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;
using Player;

[RequireComponent(typeof(InteractionTarget))]
public class WellController : MonoBehaviour
{
    [SerializeField] private int waterIncrease = 2;

    private InteractionTarget interactionTarget;

    private void Awake()
    {
        interactionTarget = GetComponent<InteractionTarget>();
    }


    private void Start()
    {
        interactionTarget.Interacted += Interact;
    }


    private void Interact()
    {
        PlayerController.Instance.playerWater.WaterAmount += waterIncrease;
    }
}
