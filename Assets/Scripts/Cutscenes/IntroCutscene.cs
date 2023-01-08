using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroCutscene : CutsceneController
{
    
    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject gameCamera;
    

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject interfaceContainer;
    [SerializeField] private GameObject farmerDialogueBoxContainer;
    [SerializeField] private TextMeshProUGUI farmerDialogueText;
    [SerializeField] private GameObject reaperDialogueBoxContainer;
    [SerializeField] private TextMeshProUGUI reaperDialogueText;
    [SerializeField] private GameObject farmer;


    override public void Begin()
    {
        StartCoroutine(StartDialogue());
    }


    private IEnumerator StartDialogue()
    {
        SetDialogueBoxVisibility(false, true);
        StartCoroutine(DrawText(reaperDialogueText, "Mortal! Tremble! I've come to haverst your miserable soul!"));
        yield return new WaitForSeconds(dialogueStayDurationSeconds * 2);

        SetDialogueBoxVisibility(false, false);
        farmer.transform.localScale = new Vector3(-1, 1, 1);
        yield return new WaitForSeconds(dialogueStayDurationSeconds / 4f);

        SetDialogueBoxVisibility(true, false);
        StartCoroutine(DrawText(farmerDialogueText, "What? Oh, harvest, yes, just in time! You must be the new guy they promised to send, finally!"));
        yield return new WaitForSeconds(dialogueStayDurationSeconds * 4);

        StartCoroutine(DrawText(farmerDialogueText, "I really have to take a break, so go ahead and start without me. Harvest those fields over there and bring your harvest here to this cart"));
        yield return new WaitForSeconds(dialogueStayDurationSeconds * 3);
        
        StartCoroutine(DrawText(farmerDialogueText, "I'll be back soon."));
        yield return new WaitForSeconds(dialogueStayDurationSeconds);

        farmer.GetComponent<FarmerController>().MoveOut();

        SetDialogueBoxVisibility(false, true);
        StartCoroutine(DrawText(reaperDialogueText, "But.. I've come.. To harvest your soul..."));
        yield return new WaitForSeconds(dialogueStayDurationSeconds * 3);

        StartCoroutine(DrawText(reaperDialogueText, "Uh..."));
        yield return new WaitForSeconds(dialogueStayDurationSeconds * 2);

        SetDialogueBoxVisibility(false, false);
        OnDialogueEnd();
    }


    private void SetDialogueBoxVisibility(bool farmer, bool reaper)
    {
        farmerDialogueBoxContainer.SetActive(farmer);
        reaperDialogueBoxContainer.SetActive(reaper);
    }


    public void OnDialogueEnd()
    {
        interfaceContainer.SetActive(true);
        menuCamera.SetActive(false);
        gameCamera.SetActive(true);
        playerInput.EnableInput();
    }
}
