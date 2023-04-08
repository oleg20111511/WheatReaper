using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerController))]
public class PlayerHarvestController : MonoBehaviour
{
    [SerializeField] private bool swipeEnabled = true;
    [SerializeField] private Transform harvestPosition;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask harvestLayerMask;
    [SerializeField] private int maxHoldSize = 10;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private TextMeshProUGUI holdIndicator;
    [SerializeField] private float inventoryFullBlinkingDurationSeconds = 2;
    [SerializeField] private float inventoryFullSingleBlinkDurationSeconds = 0.2f;
    [SerializeField] private Color inventoryFullBlinkColor;
    [SerializeField] private bool harvestTeleportationEnabled = false;

    private PlayerInput input;
    private PlayerMovementController movementController;
    private List<WheatController> swipeHarvestTargets;
    private int wheatOnHand = 0;
    private bool swipeAvailable = true;

    public int WheatOnHand
    {
        get { return wheatOnHand; }
        set {
            wheatOnHand = value;
            holdIndicator.text = string.Format("{0}/{1}", value, maxHoldSize);
        }
    }

    public bool CanSwipe
    {
        get { return swipeEnabled && swipeAvailable; }
    }

    public int BagSize
    {
        get { return maxHoldSize; }
        set {
            maxHoldSize = value;
            holdIndicator.text = string.Format("{0}/{1}", WheatOnHand, value);
        }
    }


    private void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        input = GetComponent<PlayerInput>();
        WheatOnHand = 0;
    }


    private void Update()
    {
        if (CanSwipe && input.harvestInput)
        {
            Swipe();
        }
    }


    public void EnableSwipe()
    {
        swipeEnabled = true;
    }


    public void EnableHarvestTeleportation()
    {
        harvestTeleportationEnabled = true;
    }


    public void DisableSwipe()
    {
        swipeEnabled = false;
    }


    // Called from animator
    public void OnSwipeStart()
    {
        swipeAvailable = false;
        swipeHarvestTargets = new List<WheatController>(WheatTarget.highlightedFields);
        // If player is standing over a fully-grown field, consider swipe intention as harvest
        if (swipeHarvestTargets.Exists(w => w.WheatGrowth.IsGrown))
        {
            movementController.DisableMovement();
        }
    }


    // Called from animator. Happens on swipe slash frame
    public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, PestManager.Instance.PestsLayerMask);
        foreach (Collider2D col in colliders)
        {
            PlayerController.Instance.Balance += PestManager.Instance.KillReward;
            col.GetComponent<PestController>().GetHit();
        }
    }


    // Called from animator
    public void OnSwipeEnd()
    {
        Harvest();

        movementController.EnableMovement();
        PlayerController.Instance.PlayAnimation(PlayerController.ANIMATION_IDLE);
        swipeAvailable = true;
        swipeHarvestTargets.Clear();
    }


    private void Swipe()
    {
        PlayerController.Instance.PlayAnimation(PlayerController.ANIMATION_SWIPE);
    }


    private void Harvest()
    {
        swipeHarvestTargets.RemoveAll(w => !w.WheatGrowth.IsGrown);
        foreach (WheatController field in swipeHarvestTargets)
        {
            if (WheatOnHand == maxHoldSize)
            {
                StartCoroutine(BlinkWheatIndicator());
                return;
            }
            
            WheatOnHand += WheatController.WheatPerField;
            field.Harvest();
            if (harvestTeleportationEnabled)
            {
                CartController.Instance.Interact();
            }
        }
    }


    private IEnumerator BlinkWheatIndicator()
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
}
