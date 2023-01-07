using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerController))]
public class PlayerHarvestController : MonoBehaviour
{
    [SerializeField] private bool canSwipe = true;
    [SerializeField] private Transform harvestPosition;
    [SerializeField] private LayerMask harvestLayerMask;
    [SerializeField] private int maxHoldSize = 10;
    [SerializeField] private TextMeshProUGUI holdIndicator;
    [SerializeField] private float inventoryFullBlinkingDurationSeconds = 2;
    [SerializeField] private float inventoryFullSingleBlinkDurationSeconds = 0.2f;
    [SerializeField] private Color inventoryFullBlinkColor;
    [SerializeField] private CartController cart;

    private PlayerInput input;
    private PlayerMovementController movementController;
    private Animator animator;
    private int wheatOnHand = 0;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        SetWheatOnHandValue(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (canSwipe && input.harvestInput)
        {
            Swipe();
        }

        if (input.interactionInput)
        {
            TransferWheat();
        }
    }


    private void Swipe()
    {
        movementController.DisableMovement();
        animator.Play(PlayerController.ANIMATION_SWIPE);
        canSwipe = false;
    }


    public void OnSwipeEnd()
    {
        Harvest();

        movementController.EnableMovement();
        animator.Play(PlayerController.ANIMATION_IDLE);
        canSwipe = true;
    }


    private void Harvest()
    {
        if (wheatOnHand == maxHoldSize)
        {
            StartCoroutine(BlinkWheatIndicator());
            return;
        }
        if (WheatController.highlightedObject != null)
        {
            WheatController wheatController = WheatController.highlightedObject.GetComponent<WheatController>();
            if (wheatController.IsGrown())
            {
                wheatController.Harvest();
                SetWheatOnHandValue(wheatOnHand + 1);
            }
        }
    }


    IEnumerator BlinkWheatIndicator()
    {
        float timeElapsed = 0;
        Color nextColor = inventoryFullBlinkColor;
        while (timeElapsed < inventoryFullBlinkingDurationSeconds)
        {
            holdIndicator.color = nextColor;

            if (nextColor == inventoryFullBlinkColor)
            {
                nextColor = Color.white;
            }
            else
            {
                nextColor = inventoryFullBlinkColor;
            }

            yield return new WaitForSeconds(inventoryFullSingleBlinkDurationSeconds);
            timeElapsed += inventoryFullSingleBlinkDurationSeconds;
        }

        holdIndicator.color = Color.white;
    }


    private void SetWheatOnHandValue(int newValue)
    {
        wheatOnHand = newValue;
        holdIndicator.text = string.Format("{0}/{1}", newValue, maxHoldSize);
    }


    private void TransferWheat()
    {
        // Check if cart is cart nearby
        if (cart.IsInRange()) {
            int cartSpace = cart.GetCapacity() - cart.wheatAmount;
            int transferAmount = Mathf.Min(wheatOnHand, cartSpace);

            SetWheatOnHandValue(wheatOnHand - transferAmount);
            cart.SetWheatAmount(cart.wheatAmount + transferAmount);
        }
    }
}
