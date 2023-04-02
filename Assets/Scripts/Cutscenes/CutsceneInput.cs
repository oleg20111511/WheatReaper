using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInput : MonoBehaviour
{
    public bool skipInput {get; private set;}

    [SerializeField] private bool inputEnabled = true;


    public void EnableInput()
    {
        inputEnabled = true;
    }


    public void DisableInput()
    {
        skipInput = false;
        inputEnabled = false;
    }


    private void Update()
    {
        if (inputEnabled)
        {
            HandleInput();
        }
    }


    private void HandleInput()
    {
        skipInput = Input.GetMouseButtonDown(0);
    }    
}
