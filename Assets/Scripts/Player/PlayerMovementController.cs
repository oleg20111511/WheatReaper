using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovementController : MonoBehaviour
    {
        public Vector2 lookDirection {get; private set;}

        [SerializeField] private bool canMove = true;
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private bool isFacingRight = true;

        private PlayerInput input;
        private Rigidbody2D rb2d;


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


        private void Start()
        {
            input = GetComponent<PlayerInput>();
            rb2d = GetComponent<Rigidbody2D>();
        }


        private void Update()
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
            isFacingRight = !isFacingRight;
            if (isFacingRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }


        private void Move()
        {
            Vector2 direction = new Vector2(input.xMovement, input.yMovement);
            direction.Normalize();

            // Mirror sprite if player switches direction
            if ((isFacingRight && direction.x < 0) || (!isFacingRight && direction.x > 0))
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