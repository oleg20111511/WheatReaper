using System;
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

    
    public int MaxStage
    {
        get { return growthStages.Count - 1; }
    }


    public int Stage
    {
        get { return currentStage; }
        set {
            currentStage = value;

            if (currentStage < 0)
            {
                currentStage = 0;
            }
            else if (currentStage > MaxStage)
            {
                currentStage = MaxStage;
            }

            spriteRenderer.sprite = growthStages[currentStage];

            if (currentStage == MaxStage)
            {
                exclamationMark.SetActive(true);
            }
            else
            {
                exclamationMark.SetActive(false);
            }
        }
    }


    private void Awake()
    {
        wheatController = GetComponent<WheatController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        Stage = MaxStage;
    }


    public void ChangeGrowthDuration(int change)
    {
        stageDurationMilliseconds += change;
    }


    public void StartGrowth()
    {
        Stage = 0;
        growthTask = StartCoroutine(Grow());
    }


    public void GetDamaged()
    {
        if (growthTask != null)
        {
            StopCoroutine(growthTask);
        }
        
        Stage = 0;
        spriteRenderer.enabled = false;
    }


    public void Recover()
    {
        spriteRenderer.enabled = true;
        StartGrowth();
    }


    private IEnumerator Grow()
    {
        while(currentStage < growthStages.Count - 1)
        {
            float delta = stageDurationMilliseconds * stageDurationRandomizationValue;
            float waitTime = UnityEngine.Random.Range(stageDurationMilliseconds - delta, stageDurationMilliseconds + delta);
            waitTime /= 1000f;
            yield return new WaitForSeconds(waitTime);
            Stage += 1;
        }
        growthTask = null;
    }
}
