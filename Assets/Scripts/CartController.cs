using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class CartController : MonoBehaviour, IInteractable
{
    private static CartController instance;

    [SerializeField] private int capacity = 30;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<int> fullnessBreakpoints;  // Contains ints that act as references for sprite-change condition. Must match length of sprites
    [SerializeField] private GameObject keyPrompt;
    [SerializeField] private Transform outPosition;
    [SerializeField] private Transform inPosition;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private FarmerController farmer;
    [SerializeField] private AudioClip wheatTransferSound;
    [SerializeField] private AudioClip movementSound;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;
    private AudioSource audioSource;
    private Transform movementTarget;
    private int wheatAmount = 0;

    
    public static CartController Instance
    {
        get { return instance; }
    }


    public int Capacity
    {
        get { return capacity; }
    }


    public int WheatAmount
    {
        get { return wheatAmount; }
        set { SetWheatAmount(value); }
    }


    public bool InteractionEnabled
    {
        get { return !movementTarget; }
    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }


    private void FixedUpdate()
    {
        // Check if cart is moving and reached target position
        if (movementTarget != null && Utils.IsClose(movementTarget, transform))
        {
            if (movementTarget == outPosition)
            {
                PlayerController.Instance.Balance += WheatAmount;
                WheatAmount = 0;
                MoveTo(inPosition);
            }
            else
            {
                movementTarget = null;
                rb2d.velocity = Vector2.zero;
                audioSource.Stop();
            }
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
        if (WheatAmount == capacity)
        {
            return;
        }
        // Transfer wheat from player to cart
        PlayerHarvestController harvestController = PlayerController.Instance.harvestController;

        int cartSpace = Capacity - WheatAmount;
        int transferAmount = Mathf.Min(harvestController.WheatOnHand, cartSpace);

        harvestController.WheatOnHand -= transferAmount;
        WheatAmount += transferAmount;
    }


    public void EnableHarvestTeleportation()
    {
        wheatTransferSound = null;
    }


    public void MoveOut()
    {
        audioSource.clip = movementSound;
        audioSource.loop = true;
        audioSource.Play();

        MoveTo(outPosition);
    }


    private void SetWheatAmount(int newAmount)
    {
        if (newAmount > wheatAmount)
        {
            audioSource.clip = wheatTransferSound;
            audioSource.loop = false;
            audioSource.Play();
        }

        wheatAmount = newAmount;
        int fullnessState = 0;

        // Find breakpoint that is closest to, yet lower than, new wheat amount
        // Index of that breakpoint will correspond to index of sprite
        for (int i = 0; i < fullnessBreakpoints.Count; i++)
        {
            if (fullnessBreakpoints[i] > newAmount)
            {
                break;
            }
            fullnessState = i;
        }

        spriteRenderer.sprite = sprites[fullnessState];

        if (newAmount == capacity)
        {
            farmer.TakeCart();
        }
    }


    private void MoveTo(Transform newTarget)
    {
        movementTarget = newTarget;
        Vector2 direction = (newTarget.position - transform.position).normalized;
        rb2d.velocity = direction * movementSpeed;
    }
}
