using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject menuContainer;
    
    [SerializeField] private PlayableDirector newGameTimeline;
    [SerializeField] private CutsceneController introDialogue;

    public void StartGame()
    {
        menuContainer.SetActive(false);
        newGameTimeline.Play();
    }


    public void OnIntroTimelineEnd()
    {
        introDialogue.Begin();
    }
}
