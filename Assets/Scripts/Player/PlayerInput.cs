using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool inputEnabled = true;

    public float xMovement {get; private set;}
    public float yMovement {get; private set;}
    public bool harvestInput {get; private set;}
    public bool interactionInput {get; private set;}
    
    private KeyCode moveLeftKey = KeyCode.A;
    private KeyCode moveRightKey = KeyCode.D;
    private KeyCode moveUpKey = KeyCode.W;
    private KeyCode moveDownKey = KeyCode.S;
    private KeyCode interactionKey = KeyCode.E;

    void Update()
    {
        if (inputEnabled)
        {
            HandleInput();
        }
    }


    private void HandleInput()
    {
        // Handle movement input in a way so that when both keys for an axis are pressed, movement is set to 0
        // xMovement:
        if (Input.GetKey(moveRightKey) && !Input.GetKey(moveLeftKey))
        {
            xMovement = 1;
        }
        else if (Input.GetKey(moveLeftKey) && !Input.GetKey(moveRightKey))
        {
            xMovement = -1;
        }
        else
        {
            xMovement = 0;
        }

        // yMovement:
        if (Input.GetKey(moveUpKey) && !Input.GetKey(moveDownKey))
        {
            yMovement = 1;
        }
        else if (Input.GetKey(moveDownKey) && !Input.GetKey(moveUpKey))
        {
            yMovement = -1;
        }
        else
        {
            yMovement = 0;
        }

        harvestInput = Input.GetMouseButtonDown(0);
        interactionInput = Input.GetKeyDown(interactionKey);
    }


    public void EnableInput()
    {
        inputEnabled = true;
    }


    public void DisableInput()
    {
        inputEnabled = false;
    }
}
