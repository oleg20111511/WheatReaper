using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInput : MonoBehaviour
{
    [SerializeField] private bool inputEnabled = true;
    public bool skipInput {get; private set;}

    void Update()
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


    public void EnableInput()
    {
        inputEnabled = true;
    }


    public void DisableInput()
    {
        skipInput = false;
        inputEnabled = false;
    }
}
