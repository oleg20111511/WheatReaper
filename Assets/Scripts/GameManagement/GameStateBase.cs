using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public abstract class GameStateBase : MonoBehaviour
    {
        public delegate void StateUpdateHandler();
        public event StateUpdateHandler StateUpdate;

        public delegate void StateFixedUpdateHandler();
        public event StateFixedUpdateHandler StateFixedUpdate;

        public virtual void EnterState(GameStateBase fromState)
        {
        }


        public void UpdateState()
        {
            StateUpdate?.Invoke();
        }


        public void FixedUpdateState()
        {
            StateFixedUpdate?.Invoke();
        }


        public virtual void ExitState(GameStateBase toState)
        {
        }
    }
}
