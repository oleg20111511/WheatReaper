using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerHarvestController))]
[RequireComponent(typeof(PlayerMovementController))]
public class PlayerController : MonoBehaviour
{
    public const string ANIMATION_IDLE = "ReaperIdle";
    public const string ANIMATION_WALK = "ReaperWalk";
    public const string ANIMATION_SWIPE = "ReaperSwipe";

    public PlayerHarvestController harvestController {get; private set;}
    public PlayerMovementController movementController {get; private set;}


    void Start()
    {  
        movementController = GetComponent<PlayerMovementController>();
        harvestController = GetComponent<PlayerHarvestController>();
    }    
}
