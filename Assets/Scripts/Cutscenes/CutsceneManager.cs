using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

namespace Cutscenes
{
    public class CutsceneManager : MonoBehaviour
    {
        private static CutsceneManager instance;

        [SerializeField] private CutsceneInput cutsceneInput;
        [SerializeField] private GameObject interfaceContainer;

        private CutsceneBase currentCutscene;

        public static CutsceneManager Instance
        {
            get { return instance; }
        }

        public CutsceneInput CutsceneInput
        {
            get { return cutsceneInput; }
        }

        public CutsceneBase CurrentCutscene
        {
            get { return currentCutscene; }
        }


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }


        public void PlayCutscene(CutsceneBase cutscene)
        {
            if (currentCutscene)
            {
                throw new System.Exception("Cutscene is already playing");
            }

            GameManager.Instance.ChangeState<StateAnimation>();
            interfaceContainer.SetActive(false);
            currentCutscene = cutscene;
            currentCutscene.CutsceneEnded += OnCutsceneEnd;
            currentCutscene.Begin();
        }


        public void PlayAttachedCutscene<T>() where T : CutsceneBase
        {
            CutsceneBase cutscene = GetComponent<T>();
            PlayCutscene(cutscene);
        }


        private void OnCutsceneEnd(CutsceneBase caller)
        {
            if (caller != currentCutscene)
            {
                throw new System.Exception("Cutscene ended from somewhere outside of currently playing cutscene");
            }

            currentCutscene.CutsceneEnded -= OnCutsceneEnd;

            currentCutscene = null;
            interfaceContainer.SetActive(true);

            GameManager.Instance.ChangeState<StateGameplay>();
        }
    }
}
