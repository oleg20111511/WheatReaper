using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class CutsceneController : MonoBehaviour
{
    public bool isPlaying {get; protected set;} = false;
    [SerializeField] protected float dialogueStayDurationSeconds = 3f;


    public static IEnumerator DrawText(TextMeshProUGUI textHolder, string text)
    {
        // Used to make sure previous skips won't affect this function
        yield return null;

        float letterDrawDurationSeconds = 0.01f;
        textHolder.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            // Fully display text if skip button was pressed
            if (PlayerController.Instance.cutsceneInput.skipInput)
            {
                textHolder.text = text;
                yield return null;
                break;
            }


            textHolder.text += text[i];
            // A hack to execute coroutine in static
            yield return GameController.Instance.StartCoroutine(ClickOrWait(letterDrawDurationSeconds));

            // Additional delay when sentence ends.
            if ((text[i] == '!' || text[i] == '?' || text[i] == '.') && i != text.Length - 1)
            {
                yield return GameController.Instance.StartCoroutine(ClickOrWait(letterDrawDurationSeconds * 100));
            }
        }
    }


    public static IEnumerator ClickOrWait(float waitTime)
    {
        float endTime = Time.time + waitTime;

        while (Time.time < endTime && !PlayerController.Instance.cutsceneInput.skipInput)
        {
            yield return null;
        }
    }


    public void Play()
    {
        isPlaying = true;
        Begin();
    }


    public abstract void Begin();


    public void Terminate()
    {
        gameObject.SetActive(false);
    }
}
