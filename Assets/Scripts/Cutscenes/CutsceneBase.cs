using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscenes
{
    public abstract class CutsceneBase : MonoBehaviour
    {
        public delegate void CutsceneEndedHandler(CutsceneBase caller);
        public event CutsceneEndedHandler CutsceneEnded;


        public void Begin()
        {
            OnBegin();
        }


        public void End()
        {
            OnEnd();
            CutsceneEnded?.Invoke(this);
        }


        protected virtual void OnBegin()
        {
        }

        protected virtual void OnEnd()
        {
        }
    }
}