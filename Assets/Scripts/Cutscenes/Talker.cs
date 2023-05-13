using System.Collections;
using TMPro;
using UnityEngine;

namespace Cutscenes
{
    public class Talker : MonoBehaviour
    {
        public GameObject dialogueBoxContainer;
        public TextMeshProUGUI dialogueBoxText;

        private CutsceneInput cutsceneInput;


        private void Start()
        {
            cutsceneInput = CutsceneManager.Instance.CutsceneInput;
        }


        public IEnumerator Say(string text)
        {
            dialogueBoxContainer.SetActive(true);
            yield return StartCoroutine(CutsceneUtilities.DrawText(dialogueBoxText, text));

            while (!cutsceneInput.skipInput)
            {
                yield return null;
            }
        }
    }
}
