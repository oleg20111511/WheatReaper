using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class StateGameplay : GameStateBase
    {
        private void Awake()
        {
            Physics2D.simulationMode = SimulationMode2D.Script;
        }


        public override void EnterState(GameStateBase fromState)
        {
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        }


        public override void ExitState(GameStateBase toState)
        {
            Physics2D.simulationMode = SimulationMode2D.Script;
        }
    }
}