using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameplayCamera : MonoBehaviour
{
    [SerializeField] private Camera attachedCamera;
    [SerializeField] private LayerMask gameplayLayerMask;
    
    private void OnEnable()
    {
        attachedCamera.cullingMask = gameplayLayerMask;
    }
}
