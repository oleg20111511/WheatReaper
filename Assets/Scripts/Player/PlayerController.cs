using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CutsceneInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerHarvestController))]
[RequireComponent(typeof(PlayerMovementController))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    public const string ANIMATION_IDLE = "ReaperIdle";
    public const string ANIMATION_WALK = "ReaperWalk";
    public const string ANIMATION_SWIPE = "ReaperSwipe";

    public PlayerHarvestController harvestController {get; private set;}
    public PlayerMovementController movementController {get; private set;}
    public PlayerInput playerInput {get; private set;}
    public CutsceneInput cutsceneInput {get; private set;}


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        movementController = GetComponent<PlayerMovementController>();
        harvestController = GetComponent<PlayerHarvestController>();
        playerInput = GetComponent<PlayerInput>();
        cutsceneInput = GetComponent<CutsceneInput>();
    }


    public static PlayerController Instance
    {
        get { return instance; }
    }
}
