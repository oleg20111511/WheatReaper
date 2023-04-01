using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    [SerializeField] private GameObject menuContainer;
    [SerializeField] private PlayableDirector newGameTimeline;
    [SerializeField] private CutsceneController introDialogue;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // DontDestroyOnLoad(gameObject);
    }


    public static GameController Instance
    {
        get { return instance; }
    }


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
