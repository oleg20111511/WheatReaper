using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class CutsceneController : MonoBehaviour
{
    public bool isPlaying {get; protected set;} = false;
    [SerializeField] protected float dialogueStayDurationSeconds = 3f;

    public void Play()
    {
        isPlaying = true;
        Begin();
    }


    public abstract void Begin();


    public static IEnumerator DrawText(TextMeshProUGUI textHolder, string text)
    {
        float letterDrawDurationSeconds = 0.01f;
        textHolder.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            textHolder.text += text[i];
            yield return new WaitForSeconds(letterDrawDurationSeconds);

            // Additional delay when sentence ends.
            if (text[i] == '!' || text[i] == '?' || text[i] == '.')
            {
                yield return new WaitForSeconds(letterDrawDurationSeconds * 100);
            }
        }
    }


    
    public void Terminate()
    {
        gameObject.SetActive(false);
    }
}
