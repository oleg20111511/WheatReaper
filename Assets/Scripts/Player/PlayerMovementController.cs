using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerMovementController : MonoBehaviour
    {
        public Vector2 lookDirection {get; private set;}

        [SerializeField] private bool canMove = true;
        [SerializeField] private float movementSpeed = 10f;

        private PlayerInput input;
        private Rigidbody2D rb2d;
        private SpriteRenderer spriteRenderer;


        public float MovementSpeed
        {
            get { return movementSpeed; }
            set
            {
                if (value < 0.01f)
                {
                    throw new System.ArgumentException("Speed can't be lower than 0.01f");
                }
                movementSpeed = value;
            }
        }


        public bool IsFacingRight
        {
            get {
                return !spriteRenderer.flipX;
            }
            set {
                spriteRenderer.flipX = !value;
            }
        }


        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            rb2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void Start()
        {
            GameManager.Instance.GetState<StateGameplay>().StateUpdate += OnStateUpdate;
        }


        private void OnStateUpdate()
        {
            if (canMove)
            {
                Move();
            }
        }


        public void EnableMovement()
        {
            canMove = true;
        }


        public void DisableMovement()
        {
            rb2d.velocity = Vector2.zero;
            canMove = false;
        }


        private void Flip()
        {
            IsFacingRight = !IsFacingRight;
        }


        private void Move()
        {
            Vector2 direction = new Vector2(input.xMovement, input.yMovement);
            direction.Normalize();

            // Mirror sprite if player switches direction
            if ((IsFacingRight && direction.x < 0) || (!IsFacingRight && direction.x > 0))
            {
                Flip();
            }


            if (PlayerController.Instance.currentAnimation != PlayerController.ANIMATION_SWIPE)
            {
                if (direction.magnitude != 0)
                {
                    lookDirection = DirectionFromVector(direction);
                    PlayerController.Instance.PlayAnimation(PlayerController.ANIMATION_WALK);
                }
                else
                {
                    PlayerController.Instance.PlayAnimation(PlayerController.ANIMATION_IDLE);
                }
            }

            rb2d.velocity = direction * movementSpeed;
        }


        private Vector2 DirectionFromVector(Vector2 v)
        {
            if (v.x > 0)
            {
                return Vector2.right;
            }
            else
            {
                return Vector2.left;
            }
        }
    }
}