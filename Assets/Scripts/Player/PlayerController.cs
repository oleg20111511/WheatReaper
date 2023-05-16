using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;
using Cutscenes;
using Interaction;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerHarvestController))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(Talker))]
    public class PlayerController : MonoBehaviour
    {
        // STATIC
        private static PlayerController instance;

        // ANIMATIONS
        public const string ANIMATION_IDLE = "ReaperIdle";
        public const string ANIMATION_WALK = "ReaperWalk";
        public const string ANIMATION_SWIPE = "ReaperSwipe";

        // PUBLIC COMPONENTS
        public PlayerHarvestController harvestController { get; private set; }
        public PlayerMovementController movementController { get; private set; }
        public PlayerInput playerInput { get; private set; }
        public Talker talker { get; private set; }

        // PUBLIC STATUS VARIABLES
        public string currentAnimation { get; private set; }
        public int totalEarnings { get; private set; } = 0;

        // PRIVATE COMPONENTS
        private Animator animator;

        // PRIVATE STATUS VARIABLES
        private int balance = 0;


        // PROPERTIES
        public static PlayerController Instance
        {
            get { return instance; }
        }


        public int Balance
        {
            get { return balance; }
            set
            {
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


        // METHODS
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
            talker = GetComponent<Talker>();

            animator = GetComponent<Animator>();
        }


        private void Start()
        {
            GameManager.Instance.GetState<StateGameplay>().StateUpdate += OnStateUpdate;
        }


        public void PlayAnimation(string animationName)
        {
            currentAnimation = animationName;
            animator.Play(animationName);
        }


        private void OnStateUpdate()
        {
            if (playerInput.interactionInput)
            {
                AttemptInteraction();
            }
        }


        private void AttemptInteraction()
        {
            if (InteractionTarget.currentTarget != null && InteractionTarget.currentTarget.IsInteractionEnabled())
            {
                InteractionTarget.currentTarget.Interact();
            }
        }
    }
}