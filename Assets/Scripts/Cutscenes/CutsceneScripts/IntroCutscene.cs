using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Player;

namespace Cutscenes
{
    public class IntroCutscene : CutsceneBase
    {
        [SerializeField] private PlayableDirector timeline;
        [SerializeField] private GameObject menuContainer;
        [SerializeField] private GameObject menuCamera;
        [SerializeField] private GameObject gameCamera;
        
        private Talker farmerTalker;
        private SpriteRenderer farmerSpriteRenderer;
        private Talker reaperTalker;
        
        private void Start()
        {
            farmerTalker = FarmerController.Instance.talker;
            farmerSpriteRenderer = FarmerController.Instance.gameObject.GetComponent<SpriteRenderer>();
            reaperTalker = PlayerController.Instance.talker;
        }

        protected override void OnBegin()
        {
            menuContainer.SetActive(false);
            if (Debug.isDebugBuild)
            {
                End();
                return;
            }
            timeline.Play();
        }


        protected override void OnEnd()
        {
            menuCamera.SetActive(false);
            gameCamera.SetActive(true);
        }


        public void OnIntroTimelineEnd()
        {
            StartCoroutine(StartDialogue());
        }


        private IEnumerator StartDialogue()
        {
            yield return StartCoroutine(reaperTalker.Say("Mortal! Tremble! I've come to haverst your miserable soul!"));
            reaperTalker.dialogueBoxContainer.SetActive(false);

            farmerSpriteRenderer.flipX = true;
            yield return StartCoroutine(farmerTalker.Say("What? Oh, harvest, yes, just in time! You must be the new guy they promised to send, finally!"));

            yield return StartCoroutine(farmerTalker.Say("I really have to take a break, so go ahead and start without me. Harvest those fields over there and bring your harvest here to this cart"));

            yield return StartCoroutine(farmerTalker.Say("I'll be back soon."));
            farmerSpriteRenderer.GetComponent<FarmerController>().MoveOut();
            farmerTalker.dialogueBoxContainer.SetActive(false);

            yield return StartCoroutine(reaperTalker.Say("But.. I've come.. To harvest your soul..."));

            yield return StartCoroutine(reaperTalker.Say("Uh..."));
            reaperTalker.dialogueBoxContainer.SetActive(false);

            End();
        }
    }
}
