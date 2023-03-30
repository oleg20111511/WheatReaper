using System.Collections;
using UnityEngine;
using TMPro;


public class Talker : MonoBehaviour
{
    public GameObject dialogueBoxContainer;
    public TextMeshProUGUI dialogueBoxText;

    private CutsceneInput playerInput;

    private void Start()
    {
        playerInput = GameController.Instance.PlayerController.cutsceneInput;
    }


    public IEnumerator Say(string text)
    {

        dialogueBoxContainer.SetActive(true);
        yield return StartCoroutine(CutsceneController.DrawText(dialogueBoxText, text));
        while (!playerInput.skipInput)
        {
            yield return null;
        }
    }
}
