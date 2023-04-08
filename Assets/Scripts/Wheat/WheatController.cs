using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(WheatTarget))]
[RequireComponent(typeof(WheatGrowth))]
public class WheatController : MonoBehaviour, IPestVulnerability
{
    public static List<WheatController> AllWheatControllers {get; private set;} = new List<WheatController>();

    private static int wheatPerField = 1;
    [SerializeField] private float infestationRecoveryDurationSeconds = 30f;
    [SerializeField] private Image recoveryProgressDisplay;

    private WheatTarget wheatTarget;
    private WheatGrowth wheatGrowth;
    private AudioSource audioSource;
    private bool isInfested = false;

    
    public static int WheatPerField
    {
        get { return wheatPerField; }
        set { wheatPerField = value; }
    }

    public WheatTarget WheatTarget
    {
        get { return wheatTarget; }
    }

    public WheatGrowth WheatGrowth
    {
        get { return wheatGrowth; }
    }

    public bool CanBeDamagedByPest
    {
        get { return !isInfested && wheatGrowth.IsGrown; }
    }

    public bool IsInfested
    {
        get { return isInfested; }
    }


    private void Awake()
    {
        if (!AllWheatControllers.Contains(this))
        {
            AllWheatControllers.Add(this);
        }

        wheatTarget = GetComponent<WheatTarget>();
        wheatGrowth = GetComponent<WheatGrowth>();
    }


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void DamageByPest()
    {
        isInfested = true;
        wheatGrowth.GetDamaged();

        StartCoroutine(Recover());
    }


    public void Harvest()
    {
        audioSource.Play();
        wheatGrowth.StartGrowth();
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
        recoveryProgressDisplay.enabled = false;
        wheatGrowth.Recover();
    }
}
