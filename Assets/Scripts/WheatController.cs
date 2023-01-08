using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class WheatController : MonoBehaviour
{
    [SerializeField] private List<Sprite> growthStages;
    [SerializeField] private int minStageDurationMilliseconds = 10000;
    [SerializeField] private int maxStageDurationMilliseconds = 20000;
    [SerializeField] private GameObject hoverFrame;
    [SerializeField] private GameObject exclamationMark;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public static GameObject highlightedObject {get; private set;}  // Used to assert that only one wheat is highlighted at a time

    private int currentStage;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetStage(growthStages.Count - 1);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("HoverTrigger"))
        {
            // Also remove highlight from previously highlighted object
            HoverShow();
            if (highlightedObject != null)
            {
                highlightedObject.SendMessage("HoverHide");
            }
            highlightedObject = gameObject;
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("HoverTrigger"))
        {
            HoverHide();
            if (highlightedObject == gameObject)
            {
                highlightedObject = null;
            }
        }
    }


    IEnumerator Grow()
    {
        while(currentStage < growthStages.Count - 1)
        {
            float waitTime = Random.Range(minStageDurationMilliseconds, maxStageDurationMilliseconds);
            waitTime /= 1000f;
            yield return new WaitForSeconds(waitTime);
            SetStage(currentStage + 1);
        }
    }


    void SetStage(int newStage)
    {
        currentStage = newStage;
        spriteRenderer.sprite = growthStages[newStage];

        if (newStage == growthStages.Count - 1)
        {
            exclamationMark.SetActive(true);
        }
        else
        {
            exclamationMark.SetActive(false);
        }
    }


    void RestartGrowth()
    {
        SetStage(0);
        StartCoroutine(Grow());
    }


    public void HoverShow()
    {
        hoverFrame.SetActive(true);
    }


    public void HoverHide()
    {
        hoverFrame.SetActive(false);
    }


    public bool IsGrown()
    {
        return currentStage == growthStages.Count - 1;
    }


    public void Harvest()
    {
        audioSource.Play();
        RestartGrowth();
    }
}
