using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Cutscenes
{
    public class CutsceneUtilities
    {
        public static IEnumerator DrawText(TextMeshProUGUI textHolder, string text)
        {
            // Used to make sure previous skips won't affect this function
            yield return null;

            float letterDrawDurationSeconds = 0.01f;
            textHolder.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                // Fully display text if skip button was pressed
                if (CutsceneManager.Instance.CutsceneInput.skipInput)
                {
                    textHolder.text = text;
                    yield return null;
                    break;
                }


                textHolder.text += text[i];
                // A hack to execute coroutine in static
                yield return CutsceneManager.Instance.StartCoroutine(ClickOrWait(letterDrawDurationSeconds));

                // Additional delay when sentence ends.
                if ((text[i] == '!' || text[i] == '?' || text[i] == '.') && i != text.Length - 1)
                {
                    yield return CutsceneManager.Instance.StartCoroutine(ClickOrWait(letterDrawDurationSeconds * 100));
                }
            }
        }


        public static IEnumerator ClickOrWait(float waitTime)
        {
            float endTime = Time.time + waitTime;

            while (Time.time < endTime && !CutsceneManager.Instance.CutsceneInput.skipInput)
            {
                yield return null;
            }
        }


        public static IEnumerator WaitForCutscene()
        {
            while (CutsceneManager.Instance.CurrentCutscene != null)
            {
                yield return null;
            }
        }
    }
}
