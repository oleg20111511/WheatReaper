using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

[RequireComponent(typeof(WheatController))]
public class WheatTarget : MonoBehaviour
{
    public static List<WheatController> highlightedFields = new List<WheatController>();

    private static int upgradeLevel = 1;
    private static WheatController targetField;  // Used to assert that only one wheat is targeted at a time

    [SerializeField] private GameObject hoverFrame;

    private WheatController wheatController;


    public static int UpgradeLevel
    {
        get { return upgradeLevel; }
        set
        {
            if (value < 1 || value > 3)
            {
                throw new System.ArgumentException("Upgrade level must be 1-3");
            }
            upgradeLevel = value;
        }
    }


    private void Awake()
    {
        wheatController = GetComponent<WheatController>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!wheatController.IsInfested && col.gameObject.CompareTag("HoverTrigger"))
        {
            // Remove previous target if exists
            if (targetField && targetField.WheatTarget != this)
            {
                ResetTarget();
            }

            SetAsTarget();
        }
    }


    private void OnTriggerExit2D(Collider2D col)
    {
        if (targetField && targetField.WheatTarget == this && col.gameObject.CompareTag("HoverTrigger"))
        {
            ResetTarget();
        }
    }


    public void HoverShow()
    {
        hoverFrame.SetActive(true);
    }


    public void HoverHide()
    {
        hoverFrame.SetActive(false);
    }


    public List<WheatController> FindVerticalNeighbours()
    {
        List<WheatController> result = new List<WheatController>();

        Vector2[] directions = {Vector2.up, Vector2.down};
        foreach (Vector2 dir in directions)
        {
            WheatController field = FindNeighbourField(dir);
            if (field)
            {
                result.Add(field);
            }
        }

        return result;
    }


    public WheatController FindNeighbourField(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 2f);
        foreach (RaycastHit2D hit in hits)
        {
            WheatController controller = hit.collider.gameObject.GetComponent<WheatController>();
            if (controller && controller != wheatController)
            {
                return controller;
            }
        }
        return null;
    }


    private void SetAsTarget()
    {
        targetField = this.wheatController;
        highlightedFields.Add(this.wheatController);
        
        // Also select top and bottom fields
        if (upgradeLevel > 1)
        {
            highlightedFields.AddRange(FindVerticalNeighbours());
        }


        // Also select side three fields
        if (upgradeLevel > 2)
        {
            WheatController sideNeighbour = FindNeighbourField(PlayerController.Instance.movementController.lookDirection);
            if (sideNeighbour)
            {
                highlightedFields.Add(sideNeighbour);
                highlightedFields.AddRange(sideNeighbour.WheatTarget.FindVerticalNeighbours());
            }
        }

        foreach (WheatController controller in highlightedFields)
        {
            controller.WheatTarget.HoverShow();
        }
    }


    private void ResetTarget()
    {
        foreach (WheatController controller in highlightedFields)
        {
            controller.WheatTarget.HoverHide();
        }

        highlightedFields.Clear();
        targetField = null;
    }
}
