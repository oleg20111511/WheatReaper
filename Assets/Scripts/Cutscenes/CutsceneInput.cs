using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

public class CutsceneInput : MonoBehaviour
{
    public bool skipInput { get; private set; }

    [SerializeField] private bool inputEnabled = true;


    private void Start()
    {
        GameManager.Instance.GameStateChanged += OnGameStateChanged;
    }


    private void Update()
    {
        if (inputEnabled)
        {
            HandleInput();
        }
    }


    public void EnableInput()
    {
        inputEnabled = true;
    }


    public void DisableInput()
    {
        inputEnabled = false;
        // Reset inputs
        skipInput = false;
    }


    private void OnGameStateChanged(GameStateBase newState)
    {
        if (newState.GetType() == typeof(StateAnimation))
        {
            EnableInput();
        }
        else
        {
            DisableInput();
        }
    }


    private void HandleInput()
    {
        skipInput = Input.GetMouseButtonDown(0);
    }
}
