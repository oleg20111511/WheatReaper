using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool InteractionEnabled { get; }

    public void Approach();
    public void Leave();
    public void Interact();
}
