using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CartController : MonoBehaviour
{
    [SerializeField] private int capacity = 30;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<int> fullnessBreakpoints;  // Contains ints that act as references for sprite-change condition. Must match length of sprites
    [SerializeField] private GameObject keyPrompt;

    public int wheatAmount {get; private set;} = 0;

    private SpriteRenderer spriteRenderer;

    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
}
