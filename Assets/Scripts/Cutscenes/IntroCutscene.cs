using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroCutscene : CutsceneController
{
    
    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject gameCamera;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CutsceneInput cutsceneInput;
    [SerializeField] private GameObject interfaceContainer;
    [SerializeField] private Talker farmerTalker;
    [SerializeField] private Talker reaperTalker;
    [SerializeField] private GameObject farmer;


    override public void Begin()
    {
        StartCoroutine(StartDialogue());
    }


    public void OnDialogueEnd()
    {
        interfaceContainer.SetActive(true);
        menuCamera.SetActive(false);
        gameCamera.SetActive(true);
        playerInput.EnableInput();
        cutsceneInput.DisableInput();
        PestManager.Instance.Enable();
    }


    private IEnumerator StartDialogue()
    {
        yield return StartCoroutine(reaperTalker.Say("Mortal! Tremble! I've come to haverst your miserable soul!"));
        reaperTalker.dialogueBoxContainer.SetActive(false);

        farmer.GetComponent<SpriteRenderer>().flipX = true;
        yield return StartCoroutine(farmerTalker.Say("What? Oh, harvest, yes, just in time! You must be the new guy they promised to send, finally!"));

        yield return StartCoroutine(farmerTalker.Say("I really have to take a break, so go ahead and start without me. Harvest those fields over there and bring your harvest here to this cart"));

        yield return StartCoroutine(farmerTalker.Say("I'll be back soon."));
        farmer.GetComponent<FarmerController>().MoveOut();
        farmerTalker.dialogueBoxContainer.SetActive(false);

        yield return StartCoroutine(reaperTalker.Say("But.. I've come.. To harvest your soul..."));

        yield return StartCoroutine(reaperTalker.Say("Uh..."));
        reaperTalker.dialogueBoxContainer.SetActive(false);

        OnDialogueEnd();
    }
}
