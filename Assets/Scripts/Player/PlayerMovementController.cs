using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private bool canMove = true;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private bool isFacingRight = true;

    private PlayerInput input;
    private Rigidbody2D rb2d;

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


    public void DisableMovement()
    {
        rb2d.velocity = Vector2.zero;
        canMove = false;
    }


    public void EnableMovement()
    {
        canMove = true;
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
                PlayerController.Instance.PlayAnimation(PlayerController.ANIMATION_WALK);
            }
            else
            {
                PlayerController.Instance.PlayAnimation(PlayerController.ANIMATION_IDLE);
            }
        }

        rb2d.velocity = direction * movementSpeed;
    }
}
