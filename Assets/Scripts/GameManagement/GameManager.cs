using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cutscenes;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public delegate void GameStateChangedHandler(GameStateBase newState);
        public event GameStateChangedHandler GameStateChanged;

        private Dictionary<Type, GameStateBase> allGameStates = new Dictionary<Type, GameStateBase>();
        private GameStateBase currentState;


        public static GameManager Instance
        {
            get { return instance; }
        }

        public GameStateBase CurrentState
        {
            get { return currentState; }
        }


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            GameStateBase[] states = GetComponents<GameStateBase>();
            foreach (GameStateBase state in states)
            {
                allGameStates[state.GetType()] = state;
            }

            currentState = states[0];
        }


        private void Update()
        {
            currentState.UpdateState();
        }


        private void FixedUpdate()
        {
            currentState.FixedUpdateState();
        }


        public T GetState<T>() where T : GameStateBase
        {
            Type key = typeof(T);
            if (allGameStates.ContainsKey(key))
            {
                return (T)allGameStates[key];
            }
            else
            {
                throw new ArgumentException("The requested game state does not exist.");
            }
        }


        public void ChangeState<T>() where T : GameStateBase
        {
            T state = GetState<T>();
            ChangeState(state);
        } 


        public void ChangeState(GameStateBase newState)
        {
            currentState.ExitState(newState);
            newState.EnterState(currentState);
            currentState = newState;
            GameStateChanged?.Invoke(newState);
        }
    }
}
