using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Cutscenes
{
    public class UpgradesExplanationCutscene : CutsceneBase
    {
        private Talker talker;

        private void Start()
        {
            talker = FarmerController.Instance.talker;
        }


        protected override void OnBegin()
        {
            StartCoroutine(Dialogue());
        }


        private IEnumerator Dialogue()
        {
            yield return StartCoroutine(talker.Say("You've been working quite hard to harvest those crops."));
            yield return StartCoroutine(talker.Say("I have your pay right here."));
            yield return StartCoroutine(talker.Say("Oh, and management guys said that you can pay to order some improvements to your work environment."));
            yield return StartCoroutine(talker.Say("Kind of a weird decision on their end to make you pay for it though, if you ask me."));
            yield return StartCoroutine(talker.Say("But anyway, tell me if you want to upgrade anything here and I'll pass it on."));
            yield return StartCoroutine(talker.Say("And you can do that only when I come here, obviously, so plan your upgrades carefully."));
            End();
        }
    }
}
