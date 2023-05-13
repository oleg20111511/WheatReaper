using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cutscenes;

namespace GameManagement
{
    public class StateMenu : GameStateBase
    {
        public void StartGame()
        {
            CutsceneManager.Instance.PlayAttachedCutscene<IntroCutscene>();
        }
    }
}
