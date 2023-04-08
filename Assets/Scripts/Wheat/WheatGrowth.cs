using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WheatController))]
public class WheatGrowth : MonoBehaviour
{
    [SerializeField] private List<Sprite> growthStages;
    [SerializeField] private int stageDurationMilliseconds = 15000;
    [SerializeField] private float stageDurationRandomizationValue = 0.5f;
    [SerializeField] private GameObject exclamationMark;

    private SpriteRenderer spriteRenderer;
    private WheatController wheatController;
    private int currentStage;
    private Coroutine growthTask;


    public bool IsGrown
    {
        get { return currentStage == growthStages.Count - 1; }
    }


    private void Awake()
    {
        wheatController = GetComponent<WheatController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        SetStage(growthStages.Count - 1);
    }


    public void ChangeGrowthDuration(int change)
    {
        stageDurationMilliseconds += change;
    }


    public void StartGrowth()
    {
        SetStage(0);
        growthTask = StartCoroutine(Grow());
    }


    public void GetDamaged()
    {
        if (growthTask != null)
        {
            StopCoroutine(growthTask);
        }
        
        SetStage(0);
        spriteRenderer.enabled = false;
    }


    public void Recover()
    {
        spriteRenderer.enabled = true;
        StartGrowth();
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
            float delta = stageDurationMilliseconds * stageDurationRandomizationValue;
            float waitTime = Random.Range(stageDurationMilliseconds - delta, stageDurationMilliseconds + delta);
            waitTime /= 1000f;
            yield return new WaitForSeconds(waitTime);
            SetStage(currentStage + 1);
        }
        growthTask = null;
    }
}
