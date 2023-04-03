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
    private static IInteractable interactable;

    public const string ANIMATION_IDLE = "ReaperIdle";
    public const string ANIMATION_WALK = "ReaperWalk";
    public const string ANIMATION_SWIPE = "ReaperSwipe";

    public PlayerHarvestController harvestController {get; private set;}
    public PlayerMovementController movementController {get; private set;}
    public PlayerInput playerInput {get; private set;}
    public CutsceneInput cutsceneInput {get; private set;}

    public string currentAnimation {get; private set;}
    public int totalEarnings {get; private set;} = 0;

    private Animator animator;
    private int balance = 0;


    public static PlayerController Instance
    {
        get { return instance; }
    }


    public int Balance
    {
        get { return balance; }
        set {
            if (value < 0)
            {
                throw new System.ArgumentException("Balance can't be negative");
            }
            if (balance < value)
            {
                totalEarnings += value - balance;
            }
            balance = value;
        }
    }


    private void Awake()
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

        animator = GetComponent<Animator>();
    }


    public void Update()
    {
        if (playerInput.interactionInput && interactable != null && interactable.InteractionEnabled)
        {
            interactable.Interact();
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        IInteractable interactableComponent = col.GetComponent<IInteractable>();
        if (interactableComponent != null && interactableComponent.InteractionEnabled && interactable == null)
        {
            interactable = interactableComponent;
            interactableComponent.Approach();
        }
    }

    
    private void OnTriggerExit2D(Collider2D col)
    {
        IInteractable interactableComponent = col.GetComponent<IInteractable>();
        if (interactableComponent != null && interactableComponent == interactable)
        {
            interactable = null;
            interactableComponent.Leave();
        }
    }


    public void PlayAnimation(string animationName)
    {
        currentAnimation = animationName;
        animator.Play(animationName);
    }


    public void Freeze()
    {
        movementController.DisableMovement();
        harvestController.DisableSwipe();
    }


    public void Unfreeze()
    {
        movementController.EnableMovement();
        harvestController.EnableSwipe();
    }
}
