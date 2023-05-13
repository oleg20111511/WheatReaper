using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public abstract class GameStateBase : MonoBehaviour
    {
        public virtual void EnterState(GameStateBase fromState)
        {
        }


        public virtual void UpdateState()
        {
        }


        public virtual void ExitState(GameStateBase toState)
        {
        }
    }
}
