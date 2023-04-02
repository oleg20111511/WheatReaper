using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class FarmerController : MonoBehaviour
{
    public const string ANIMATION_IDLE = "FarmerIdle";
    public const string ANIMATION_WALK = "FarmerWalk";

    [SerializeField] private Transform outPosition;
    [SerializeField] private Transform inPosition;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private CartController cart;

    private Talker talker;
    private Transform movementTarget;
    private Animator animator;
    private Rigidbody2D rb2d;

    private List<string> comments;
    private int commentIndex = 0;

    private void Awake()
    {
        talker = GetComponent<Talker>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        
        comments = new List<string>();
        comments.Add("Wow, you're really good at this! Keep up at it!");
        comments.Add("You're quite fast, huh?");
        comments.Add("You look really thin, all skin and bones. Come to canteen after you're finished, the food there is awesome");
        comments.Add("That scythe of yours looks nice. Wonder how you managed to find such a good tool in messy our warehouse.");
        comments.Add("Whoa, looks heavy");
        comments.Add("And here I thought those HR people were useless. Guess they actually can find useful people when they feel like it.");
        comments.Add("Working hard? Guess I'll have to add a little bonus to your pay");
        comments.Add("It's like you were born with this thing");
        comments.Add("You sure know how to handle that scythe. It looks like it's an extension of your own arm.");
    }


    private void FixedUpdate()
    {
        // Check if farmer is moving and reached target position
        if (movementTarget != null && Utils.IsClose(movementTarget, transform))
        {
            movementTarget = null;
            rb2d.velocity = Vector2.zero;
            animator.Play(ANIMATION_IDLE);
        }
    }


    public void MoveOut()
    {
        MoveTo(outPosition);
    }


    public void TakeCart()
    {
        MoveTo(inPosition);
        StartCoroutine(CommentOnProgress());
    }


    private void MoveTo(Transform newTarget)
    {
        movementTarget = newTarget;
        Vector2 direction = (newTarget.position - transform.position).normalized;

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        rb2d.velocity = direction * movementSpeed;
        animator.Play(ANIMATION_WALK);
    }


    private IEnumerator CommentOnProgress()
    {
        // Wait until farmer enters the frame
        while (movementTarget != null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        talker.dialogueBoxContainer.SetActive(true);
        StartCoroutine(CutsceneController.DrawText(talker.dialogueBoxText, comments[commentIndex]));
        if (commentIndex < comments.Count - 1) {
            commentIndex++;
        }
        yield return new WaitForSeconds(5f);

        talker.dialogueBoxContainer.SetActive(false);

        MoveTo(outPosition);
        cart.MoveOut();
    }
}
