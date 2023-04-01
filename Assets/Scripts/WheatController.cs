using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class WheatController : MonoBehaviour
{
    public static WheatController Highlighted {get; private set;}  // Used to assert that only one wheat is highlighted at a time
    public static List<WheatController> AllWheatControllers {get; private set;} = new List<WheatController>();

    [SerializeField] private List<Sprite> growthStages;
    [SerializeField] private int minStageDurationMilliseconds = 10000;
    [SerializeField] private int maxStageDurationMilliseconds = 20000;
    [SerializeField] private GameObject hoverFrame;
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private float infestationRecoveryDurationSeconds = 30f;
    [SerializeField] private Image recoveryProgressDisplay;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private int currentStage;
    private Coroutine growthTask;
    private bool isInfested = false;

    
    private void Awake()
    {
        if (!AllWheatControllers.Contains(this))
        {
            AllWheatControllers.Add(this);
        }
    }


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetStage(growthStages.Count - 1);
    }


    public void Harvest()
    {
        audioSource.Play();
        RestartGrowth();
    }


    private void RestartGrowth()
    {
        SetStage(0);
        growthTask = StartCoroutine(Grow());
    }


    private void SetStage(int newStage)
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


    private IEnumerator Grow()
    {
        while(currentStage < growthStages.Count - 1)
        {
            float waitTime = Random.Range(minStageDurationMilliseconds, maxStageDurationMilliseconds);
            waitTime /= 1000f;
            yield return new WaitForSeconds(waitTime);
            SetStage(currentStage + 1);
        }
        growthTask = null;
    }


    public void Infest()
    {
        if (growthTask != null)
        {
            StopCoroutine(growthTask);
        }
        
        SetStage(0);
        isInfested = true;
        spriteRenderer.enabled = false;
        StartCoroutine(Recover());
    }


    private IEnumerator Recover()
    {
        recoveryProgressDisplay.enabled = true;
        recoveryProgressDisplay.fillAmount = 0;
        
        float startTime = Time.time;
        float endTime = startTime + infestationRecoveryDurationSeconds;

        while (Time.time < endTime)
        {
            recoveryProgressDisplay.fillAmount = (Time.time - startTime) / infestationRecoveryDurationSeconds;
            yield return null;
        }

        isInfested = false;
        spriteRenderer.enabled = true;
        recoveryProgressDisplay.enabled = false;
        RestartGrowth();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isInfested && col.gameObject.CompareTag("HoverTrigger"))
        {
            // Also remove highlight from previously highlighted object
            HoverShow();
            if (Highlighted != null)
            {
                Highlighted.HoverHide();
            }
            Highlighted = this;
        }
    }


    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("HoverTrigger"))
        {
            HoverHide();
            if (Highlighted == this)
            {
                Highlighted = null;
            }
        }
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
}
