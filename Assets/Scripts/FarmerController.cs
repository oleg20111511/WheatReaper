using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum FarmerState
{
    Idle,
    Moving,
    WaitingForInteraction,
}


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class FarmerController : MonoBehaviour, IInteractable
{
    public const string ANIMATION_IDLE = "FarmerIdle";
    public const string ANIMATION_WALK = "FarmerWalk";

    [SerializeField] private Transform outPosition;
    [SerializeField] private Transform inPosition;
    [SerializeField] private GameObject keyPrompt;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private CartController cart;

    private Talker talker;
    private Transform movementTarget;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

    private FarmerState state = FarmerState.Idle;
    private List<string> comments;
    private int commentIndex = 0;
    private bool upgradesExplained = false;
    private bool interationFlag = false;


    public bool InteractionEnabled
    {
        get { return state == FarmerState.WaitingForInteraction; }
    }


    private void Awake()
    {
        talker = GetComponent<Talker>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
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
            state = FarmerState.Idle;
        }
    }


    public void Approach()
    {
        if (InteractionEnabled)
        {
            keyPrompt.SetActive(true);
        }
    }


    public void Leave()
    {
        keyPrompt.SetActive(false);
    }


    public void Interact()
    {
        interationFlag = true;
        keyPrompt.SetActive(false);
    }


    public void TakeCart()
    {
        MoveTo(inPosition);
        StartCoroutine(CommentOnProgress());
    }


    public void MoveOut()
    {
        MoveTo(outPosition);
    }


    private void MoveTo(Transform newTarget)
    {
        state = FarmerState.Moving;
        movementTarget = newTarget;
        Vector2 direction = (newTarget.position - transform.position).normalized;

        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
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
        
        if (PlayerController.Instance.totalEarnings >= 30)
        {
            yield return StartCoroutine(OfferUpgrades());
        }

        yield return StartCoroutine(CutsceneController.DrawText(talker.dialogueBoxText, comments[commentIndex]));
        if (commentIndex < comments.Count - 1) {
            commentIndex++;
        }
        else {
            commentIndex = 0;
        }
        yield return new WaitForSeconds(5f);

        talker.dialogueBoxContainer.SetActive(false);

        MoveTo(outPosition);
        cart.MoveOut();
    }


    private IEnumerator OfferUpgrades()
    {
        if (UpgradeManager.Instance.availableUpgrades.Count == 0)
        {
            yield break;
        }
        state = FarmerState.WaitingForInteraction;

        string comment;
        if (upgradesExplained)
        {
            comment = "I'm here with some new tools! Come here to see.";
        }
        else
        {
            comment = "Hey, I've got something for you! Could you come over here?";
        }
        yield return StartCoroutine(CutsceneController.DrawText(talker.dialogueBoxText, comment));

        // There are 2 possible situations to handle:
        // 1) Player doesn't interact and farmer leaves after waiting for too long
        // 2) Player interacts this waits for the interaction to end
        
        // Give player a limited time to approach
        float startTime = Time.time;
        float endTime = startTime + 15f;
        
        while (Time.time < endTime)
        {
            if (interationFlag)
            {
                state = FarmerState.Idle;
                if (!upgradesExplained)
                {
                    yield return StartCoroutine(ExplainUpgrades());
                }
                UpgradeManager.Instance.OpenMenu();
                break;
            }
            yield return null;
        }

        // If interation happened, wait for it to end
        while (UpgradeManager.Instance.IsMenuOpen)
        {
            yield return new WaitForSeconds(0.5f);
        }

        interationFlag = false;
    }


    private IEnumerator ExplainUpgrades()
    {
        upgradesExplained = true;
        PlayerController.Instance.Freeze();
        PlayerController.Instance.cutsceneInput.EnableInput();
        
        yield return StartCoroutine(talker.Say("Upgrades!!!"));

        PlayerController.Instance.cutsceneInput.DisableInput();
    }
}
