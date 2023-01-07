using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class CartController : MonoBehaviour
{
    [SerializeField] private int capacity = 30;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<int> fullnessBreakpoints;  // Contains ints that act as references for sprite-change condition. Must match length of sprites
    [SerializeField] private GameObject keyPrompt;
    [SerializeField] private Transform outPosition;
    [SerializeField] private Transform inPosition;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private FarmerController farmer;

    public int wheatAmount {get; private set;} = 0;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;
    private Transform movementTarget;

    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        
    }


    void FixedUpdate()
    {
        // Check if cart is moving and reached target position
        if (movementTarget != null)
        {
            float distance = (movementTarget.position - transform.position).magnitude;
            if (distance < 0.5f)
            {
                if (movementTarget == outPosition)
                {
                    SetWheatAmount(0);
                    MoveIn();
                }
                else
                {
                    movementTarget = null;
                    rb2d.velocity = Vector2.zero;
                }
                
            }
        }
    }


    public void SetWheatAmount(int newAmount)
    {
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


    // Check if player is within wheat transferring range. Since that happens when player enters trigger,
    // and when that happens keyprompt is enabled, this function returns state of keyprompt
    public bool IsInRange()
    {
        return keyPrompt.activeSelf;
    }


    public int GetCapacity()
    {
        return capacity;
    }

    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            keyPrompt.SetActive(true);
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            keyPrompt.SetActive(false);
        }
    }


    public void MoveOut()
    {
        Vector2 direction = outPosition.position - transform.position;
        direction.Normalize();
        rb2d.velocity = direction * movementSpeed;
        movementTarget = outPosition;
    }


    public void MoveIn()
    {
        Vector2 direction = inPosition.position - transform.position;
        direction.Normalize();
        rb2d.velocity = direction * movementSpeed;
        movementTarget = inPosition;
    }
}
