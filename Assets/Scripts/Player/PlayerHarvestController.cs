using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerController))]
public class PlayerHarvestController : MonoBehaviour
{
    [SerializeField] private bool canSwipe = true;
    [SerializeField] private Transform harvestPosition;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask harvestLayerMask;
    [SerializeField] private int maxHoldSize = 10;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private TextMeshProUGUI holdIndicator;
    [SerializeField] private float inventoryFullBlinkingDurationSeconds = 2;
    [SerializeField] private float inventoryFullSingleBlinkDurationSeconds = 0.2f;
    [SerializeField] private Color inventoryFullBlinkColor;
    [SerializeField] private CartController cart;

    private PlayerInput input;
    private PlayerMovementController movementController;
    private int wheatOnHand = 0;
    private WheatController swipeHarvestTarget;

    private int WheatOnHand
    {
        get { return wheatOnHand; }
        set {
            wheatOnHand = value;
            holdIndicator.text = string.Format("{0}/{1}", value, maxHoldSize);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        input = GetComponent<PlayerInput>();
        WheatOnHand = 0;
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
        PlayerController.Instance.PlayAnimation(PlayerController.ANIMATION_SWIPE);
    }


    public void OnSwipeStart()
    {
        canSwipe = false;
        swipeHarvestTarget = WheatController.Highlighted;
        // If player is standing over a fully-grown field, consider swipe intention as harvest
        if (swipeHarvestTarget && swipeHarvestTarget.IsGrown())
        {
            movementController.DisableMovement();
        }
    }


    // Happens on swipe slash frame
    public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, PestManager.Instance.PestsLayerMask);
        foreach (Collider2D col in colliders)
        {
            col.GetComponent<PestController>().GetHit();
        }
    }


    public void OnSwipeEnd()
    {
        Harvest();

        movementController.EnableMovement();
        PlayerController.Instance.PlayAnimation(PlayerController.ANIMATION_IDLE);
        canSwipe = true;
        swipeHarvestTarget = null;
    }


    private void Harvest()
    {
        if (wheatOnHand == maxHoldSize)
        {
            StartCoroutine(BlinkWheatIndicator());
            return;
        }
        if (swipeHarvestTarget && swipeHarvestTarget.IsGrown())
        {
            WheatOnHand += 1;
            swipeHarvestTarget.Harvest();
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


    private void TransferWheat()
    {
        // Check if cart is cart nearby
        if (cart.IsInRange()) {
            int cartSpace = cart.GetCapacity() - cart.wheatAmount;
            int transferAmount = Mathf.Min(wheatOnHand, cartSpace);

            WheatOnHand -= transferAmount;
            cart.SetWheatAmount(cart.wheatAmount + transferAmount);
        }
    }
}
